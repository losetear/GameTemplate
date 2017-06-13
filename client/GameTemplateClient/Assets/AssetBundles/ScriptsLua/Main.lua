--print('hello world')

local mainGo = CS.UnityEngine.GameObject("GameManager")
local mainScript = mainGo:AddComponent(typeof(CS.GameTemplate.FullLuaBehaviour))
mainScript.LuaScriptName = "manager/GameManager"

--print("测试mainScript null:",mainGo:GetComponent(typeof(CS.GameTemplate.FullLuaBehaviour)):IsNull())