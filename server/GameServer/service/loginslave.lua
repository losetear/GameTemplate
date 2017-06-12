local skynet = require "skynet"
local socket = require "socket"

local syslog = require "syslog"
local protoloader = require "protoloader"
local srp = require "srp"
local aes = require "aes"
local uuid = require "uuid"

local traceback = debug.traceback


local master
local database
local host
local auth_timeout
local session_expire_time
local session_expire_time_in_second
local connection = {}
local saved_session = {}

local slaved = {}

local CMD = {}

local LOGIN_SALT = "rpg_salt_i3J)@ks"

function CMD.init (m, id, conf)
	master = m
	database = skynet.uniqueservice ("database")
	host = protoloader.load (protoloader.LOGIN)
	auth_timeout = conf.auth_timeout * 100
	session_expire_time = conf.session_expire_time * 100
	session_expire_time_in_second = conf.session_expire_time
end

local function close (fd)
	if connection[fd] then
		socket.close (fd)
		connection[fd] = nil
	end
end

local function read (fd, size)
	return socket.read (fd, size) or error ()
end

local function read_msg (fd)
	local s = read (fd, 2)
	local size = s:byte(1) * 256 + s:byte(2)
	local msg = read (fd, size)
	return host:dispatch (msg, size)
end

local function send_msg (fd, msg)
	local package = string.pack (">s2", msg)
	socket.write (fd, package)
end

function CMD.auth (fd, addr)

	connection[fd] = addr
	skynet.timeout (auth_timeout, function ()
		if connection[fd] == addr then
			syslog.warningf ("connection %d from %s auth timeout!", fd, addr)
			close (fd)
		end
	end)

	socket.start (fd)
	socket.limit (fd, 8192)

	local type, name, args, response = read_msg (fd)
	assert (type == "REQUEST")

	syslog.noticef ("CMD.auth %s:%s", name, args)

	if name == "handshake" then
		assert (args and args.name and args.client_pub, "invalid handshake request")
		syslog.noticef ("CMD.handshake %s:%s", args.name, args.client_pub)

		local account = skynet.call (database, "lua", "account", "load", args.name) or error ("load account " .. args.name .. " failed")

		syslog.noticef ("handshake load account %s", account)

		--local session_key, _, pkey = srp.create_server_session_key (account.verifier, args.client_pub)
		local challenge = srp.random ()
		local msg = response {
					user_exists = (account.id ~= nil),
					salt = LOGIN_SALT,--account.salt,
					--server_pub = pkey,
					--challenge = challenge,
				}

		--syslog.noticef ("handshake gen salt:%s challenge:%s", account.salt,challenge)

		send_msg (fd, msg)

		type, name, args, response = read_msg (fd)

		assert (type == "REQUEST" and name == "myauth" and args and args.password, "invalid auth request")

		--local text = aes.decrypt (args.challenge, session_key)
		--assert (challenge == text, "auth challenge failed")

		local id = tonumber (account.id)
		if not id then
			syslog.notice ("auth create account")

			assert (args.password)
			id = uuid.gen ()
			local password = args.password --aes.decrypt (args.password, session_key)
			account.id = skynet.call (database, "lua", "account", "create", id, account.name, password) or error (string.format ("create account %s/%d failed", account.name, id))
		else
			assert (args.password == account.password ,"invalid auth pwd")
		end
		
		local session = skynet.call (master, "lua", "save_session", id, session_key, challenge)

		--token = genToken(session)
		token = skynet.call (master, "lua", "gen_token", session)

		syslog.noticef ("auth session:%s tokenlength:%d token:%s ", session,#token,token)

		msg = response {
				session = session,
				expire = session_expire_time_in_second,
				token = token,
			}
		send_msg (fd, msg)
		
		close (fd)
	end
end

function CMD.save_session (session, account, key, challenge)
	saved_session[session] = { account = account, key = key, challenge = challenge }
	skynet.timeout (session_expire_time, function ()
		local t = saved_session[session]
		if t and t.key == key then
			saved_session[session] = nil
		end
	end)
end

function CMD.challenge (session, secret)
	local t = saved_session[session] or error ()

	local text = aes.decrypt (secret, t.key) or error ()
	assert (text == t.challenge)

	t.token = srp.random ()
	t.challenge = srp.random ()

	return t.token, t.challenge
end

local random = math.random
local function uuid()
    local template ='xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    return string.gsub(template, '[xy]', function (c)
        local v = (c == 'x') and random(0, 0xf) or random(8, 0xb)
        return string.format('%x', v)
    end)
end

function CMD.gen_token (session)
	local t = saved_session[session] or error ()

	t.token = uuid()--srp.random ()
	t.challenge = srp.random ()

	return t.token
end

function CMD.verify (session, token)
	local t = saved_session[session] or error ()
	syslog.noticef ("loginslave CMD.session:%s c:%d_%s|s:%d_%s", session,#token,token,#t.token,t.token)

	assert (token == t.token)
	t.token = nil

	return t.account
end

skynet.start (function ()
	skynet.dispatch ("lua", function (_, _, command, ...)
		syslog.noticef ("loginslave cmd:%s", command)

		local function pret (ok, ...)
			if not ok then 
				syslog.warningf (...)
				skynet.ret ()
			else
				skynet.retpack (...)
			end
		end

		local f = assert (CMD[command])
		pret (xpcall (f, traceback, ...))
	end)
end)

