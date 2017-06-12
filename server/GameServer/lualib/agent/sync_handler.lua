local skynet = require "skynet"

local syslog = require "syslog"
local handler = require "agent.handler"


local REQUEST = {}
local user
handler = handler.new (REQUEST)

handler:init (function (u)
	user = u
end)

function REQUEST.syncclientlag (args)
	local ctime = args.clienttime or error ()
	return { clienttime = ctime }
end

function REQUEST.syncserverlag (args)
	syslog.noticef ("sync syncserverlag delta:%s",args.ping)
	return true
end

return handler
