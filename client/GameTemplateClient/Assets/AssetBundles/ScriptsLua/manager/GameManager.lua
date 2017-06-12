local beholder = require("manager/beholder")
-----------------------------------------Base-----------------------------------------

function start()
	print("lua start...")

	CS.io.ui:ShowUI('Text')
	-- Test
	CS.io.res:LoadAssetbundle("Test", function() 
		CS.io.res:LoadModel('test.lijv', 'Dog', 0, function(id,go) end)
	end)
end

function update()
	-- print("lua update...")

	-- TickInputMgr()

	-- TickSoundMgr()
	
end

function fixedupdate()
	-- print("lua fixedupdate...")
end

function onenable()
	print("gm onenable...")
end

function ondisable()
	print("gm ondisable...")
	-- --CS.io.ui:HideUI('Common/Fps')
	-- ReleaseModuleMgr()
	-- ReleaseInputMgr()
	-- ReleaseSoundMgr()
end

function ondestroy()
	print("lua ondestroy...")
end
-----------------------------------------Base-----------------------------------------