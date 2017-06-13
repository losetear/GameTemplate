--print('hello world')

-- local mainGo = CS.UnityEngine.GameObject("GameManager")
-- local mainScript = mainGo:AddComponent(typeof(CS.GameTemplate.FullLuaBehaviour))
-- mainScript.LuaScriptName = "manager/GameManager"

--print("测试mainScript null:",mainGo:GetComponent(typeof(CS.GameTemplate.FullLuaBehaviour)):IsNull())

local cam = CS.UnityEngine.GameObject("camera")
cam:AddComponent(typeof(CS.UnityEngine.Camera))

local mainGo = CS.UnityEngine.GameObject("test")
local mainScript = mainGo:AddComponent(typeof(CS.GameTemplate.FullLuaBehaviour))
mainScript.LuaScriptName = "test/testnet"