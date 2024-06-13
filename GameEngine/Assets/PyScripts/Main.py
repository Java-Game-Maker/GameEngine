
go = None

def Start():
	global go
	camEntity = Managers.entityManager.CreateEntity()
	camComponent = CameraComponent()
	transformComponent = TransformComponent()

	camComponent.InitInput(transformComponent)
	Managers.inputHandler.inputStates.Add(camComponent.editor_state)

	Managers.entityManager.AddComponent(camEntity, camComponent)
	Managers.entityManager.AddComponent(camEntity, transformComponent)

	# Load Shader
	Managers.resourceManager.Import_Shader(
		"standardShader",
		Utils.FromAssets("./Shaders/vertex.glsl"),
		Utils.FromAssets("./Shaders/fragment.glsl")
	)
	Managers.resourceManager.Import_Texture("minecraft_dirt", Utils.FromAssets("./minecraft_dirt.png"))

	location = Managers._GL.GetUniformLocation(Managers.resourceManager.Get_Shader("standardShader").ShaderProgramId, "texture1")
	Managers._GL.Uniform1(location, Managers.resourceManager.Get_Texture("minecraft_dirt").Id)

	# Create GameObject
	go = GameObject("./Models/cube.obj", "cube")
	go2 = GameObject("./Models/cube.obj", "cube2")
	go2.Translate(0, 5, 0)
	Managers.meshSystem.BindMesh(Managers.resourceManager.models["cube"])
	Managers.meshSystem.BindMesh(Managers.resourceManager.models["cube2"])

def Update():
	go.Translate(0.02,0,0)