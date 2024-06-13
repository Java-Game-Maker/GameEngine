
local entityId = _createEntity()

function onLoad()
	_addComponent(entityId, 'Mesh', {name = "testMesh", path = "./Models/cube.obj"})
	_addComponent(entityId, 'Shader', {name = "testMesh", vertPath = "./Shaders/vertex.glsl", fragPath = "./Shaders/fragment.glsl"})
	_addComponent(entityId, 'Transform', {})
end

local count = 0

function Update()
	_updateTransform(entityId, count , 0, 0)
	count = count + 0.01
end

registerOnLoad(onLoad)
registerUpdate(Update)