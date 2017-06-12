local quality = require('manager/Quality')

local EnvUtil = {}

-----------------------------------------light-----------------------------------------

local lightGo
function EnvUtil:ShowLight()
	print('创建光源')
	lightGo = CS.UnityEngine.GameObject("light")
	lightGo.transform.rotation = CS.UnityEngine.Quaternion.Euler(0,-30,0);
	local light =  lightGo:AddComponent(typeof(CS.UnityEngine.Light))
	light.type = CS.UnityEngine.LightType.Directional;
	light.shadows = quality:GetShadow();
	light.shadowResolution = quality:GetResolution()
	light.shadowStrength = 0.35
end

function EnvUtil:DestroyLight()
	CS.UnityEngine.GameObject.Destroy(lightGo)
end

-----------------------------------------light-----------------------------------------


-----------------------------------------Shader-----------------------------------------
local ModelLayerName = "ARModel"
local ModelOriShaders = {}
local FadeShader = nil
local AFKColor = CS.UnityEngine.Color(1,1,1,1)
function EnvUtil:UnitAFK(mrs,worldPos,progress)
	if FadeShader == nil then
		FadeShader = CS.UnityEngine.Shader.Find("MatrixGame/Fade");
	end

	local modelLayer = CS.UnityEngine.LayerMask.NameToLayer(ModelLayerName);
	for i=1,mrs.Length do
		if mrs[i-1].gameObject.layer == modelLayer then
			local matNum = mrs[i-1].materials.Length
			for j=0,matNum-1 do
				local mat = mrs[i-1].materials[j]
				if ModelOriShaders[mat.name] == nil then
					ModelOriShaders[mat.name] = mat.shader 
				end
				mat.shader = FadeShader
				mat:SetFloat("_HeightDis",progress)
				mat:SetVector("_Position",worldPos)
				mat:SetColor("_OccColor",AFKColor)
			end
		end
	end
end

function EnvUtil:UnitBack(mrs)
	for i=1,mrs.Length do
		local mat = mrs[i-1].material
		local matNum = mrs[i-1].materials.Length
		for j=0,matNum-1 do
			local mat = mrs[i-1].materials[j]
			if ModelOriShaders[mat.name] ~= nil then
				mat.shader = ModelOriShaders[mat.name]
			end
		end
	end
end
-----------------------------------------Shader-----------------------------------------


-----------------------------------------Layer-----------------------------------------


function EnvUtil:ChangeLayer(gameObject,layerName)
	local layer = CS.UnityEngine.LayerMask.NameToLayer(layerName);
    gameObject.layer = layer

	local trans = gameObject:GetComponentsInChildren(typeof(CS.UnityEngine.Transform));
	for i=1,trans.Length do
		local go = trans[i-1].gameObject
    	go.layer = layer
	end
end

-----------------------------------------Layer-----------------------------------------


-----------------------------------------Shadow-----------------------------------------


function EnvUtil:AddShadow(gameObject)
	print('创建阴影')
	local plane = CS.UnityEngine.GameObject.CreatePrimitive(CS.UnityEngine.PrimitiveType.Plane);
	material = CS.UnityEngine.GameObject.Instantiate(CS.UnityEngine.Resources.Load("Material/arshadow"));
	plane:GetComponent("MeshRenderer").material = material;
	plane:GetComponent("MeshRenderer").shadowCastingMode = CS.UnityEngine.Rendering.ShadowCastingMode.Off;
	plane.transform.parent = gameObject.transform;
	plane.transform.localScale = CS.UnityEngine.Vector3(0.3,1,0.3);
	plane.transform.localPosition = CS.UnityEngine.Vector3(0.1,0.01,-0.2);
	plane.transform.localRotation = CS.UnityEngine.Quaternion.identity;
	-- EnvUtil:ChangeLayer(plane,"ARModel")
end

function EnvUtil:RemoveShadow(gameObject)
	print('移除阴影接收板')
	local plane = gameObject.transform:Find('Plane')
	plane.gameObject:SetActive(false)
end

function EnvUtil:AddBornBox(gameObject)
	print('创建出生盒子')
	local boxParent = CS.UnityEngine.GameObject(gameObject.name.."_box")
	local box = CS.UnityEngine.GameObject.CreatePrimitive(CS.UnityEngine.PrimitiveType.Cube);
	material = CS.UnityEngine.GameObject.Instantiate(CS.UnityEngine.Resources.Load("Material/BornBox"));
	box:GetComponent("MeshRenderer").material = material;
	box:GetComponent("MeshRenderer").shadowCastingMode = CS.UnityEngine.Rendering.ShadowCastingMode.Off;
	box.transform.parent = boxParent.transform;
	box.transform.localScale = CS.UnityEngine.Vector3(5,3,5);
	box.transform.localPosition = CS.UnityEngine.Vector3(0,-1.51,-0.45);
	box.transform.localRotation = CS.UnityEngine.Quaternion.identity;
	box.name = gameObject.name
	return boxParent
end

-----------------------------------------Shadow-----------------------------------------

-----------------------------------------CombineMesh------------------------------------

function EnvUtil:CombineMesh(go)
	if CS.io.wrap:IsNull(go) then
		return
	end
		local targetSmr
		local materials = {}       --存material
		local bones = {} --要更换的骨骼信息
		local CombineInstance = {} --存mesh
		local hips = {} --存所有骨骼信息
		local smrTable = {}

		
		hips = go.gameObject:GetComponentsInChildren(typeof(CS.UnityEngine.Transform))
		local smrs = go.gameObject:GetComponentsInChildren(typeof(CS.UnityEngine.SkinnedMeshRenderer))
		for i=1,smrs.Length do
			table.insert(smrTable,smrs[i-1])
		end
		targetSmr = go.gameObject:GetComponent(typeof(CS.UnityEngine.SkinnedMeshRenderer))
		if CS.io.wrap:IsNull(targetSmr) then
			targetSmr = go.gameObject:AddComponent(typeof(CS.UnityEngine.SkinnedMeshRenderer))
		else
			table.remove(smrTable,1)
		end
		for k,v in pairs(smrTable) do
			--需要合并的网格--
			for a=1,v.sharedMesh.subMeshCount do
				local ci = CS.UnityEngine.CombineInstance()
				ci.mesh = v.sharedMesh
				ci.subMeshIndex = a-1
				table.insert(CombineInstance,ci)
			end
			--需要合并的材质--
			table.insert(materials,v.materials[0])
			--需要合并的个骨骼--
			for i=1,v.bones.Length do
				if CS.io.wrap:IsNull(v.bones[i-1]) then
					print("没有Bones")
					return
				else
				for n=1,hips.Length do
					if v.bones[i-1].name == hips[n-1].name then
						table.insert(bones,hips[n-1])
					end
				end
				end
			end
		end
	
	targetSmr.sharedMesh = CS.UnityEngine.Mesh()
	targetSmr.sharedMesh:CombineMeshes(CombineInstance,false,false)
	targetSmr.materials = materials
	targetSmr.bones = bones

	--删除已经被合并的go--
	for n,s in pairs(smrTable) do
		CS.UnityEngine.GameObject.DestroyImmediate(smrTable[n].gameObject)
	end

	return go
end
-----------------------------------------CombineMesh------------------------------------


return EnvUtil