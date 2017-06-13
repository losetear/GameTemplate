local beholder = require("manager/beholder")
local testnet = {}
-----------------------------------------Base-----------------------------------------

function start()
	print("testnet start...")
	local camGo = CS.UnityEngine.GameObject("TestNet")
	local cam = camGo:AddComponent(typeof(CS.UnityEngine.Camera))
	
	testnet:Connect()
end

function update()
	-- print("lua update...")
	
end

function fixedupdate()
	-- print("lua fixedupdate...")
end

function onenable()
	print("testnet onenable...")
end

function ondisable()
	print("testnet ondisable...")
end

function ondestroy()
	print("testnet ondestroy...")
end
-----------------------------------------Base-----------------------------------------


-----------------------------------------Net-----------------------------------------
local ServerIp =  "192.168.1.113";
local Login_port =  "9777";
function testnet:Connect( ... )
    CS.NetCore.Connect(ServerIp, Login_port, function() 
    	testnet:HandShake()
    end);
end
function testnet:HandShake( ... )
end
-----------------------------------------Net-----------------------------------------