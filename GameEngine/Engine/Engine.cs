using System;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace GameEngine
{
	public class Engine
	{
		private Window window;

		private EntityManager entityManager;
		private List<EntitySystem> systems;

		public Engine()
		{
			window = new Window();
		}

		public void Initialize()
		{
			window.Initialize();
			window.WindowInstance.Load += OnLoad;
			window.WindowInstance.Render += OnRender;
			window.WindowInstance.Update += OnUpdate;
			window.WindowInstance.Closing += OnClosing;
		}

		private void OnLoad()
		{
			var gl = window.GLContext;
			gl.ClearColor(0.45f, 0.85f, 1.0f, 1.0f);

			gl.Enable(GLEnum.DepthTest);
			gl.DepthFunc(GLEnum.Less);

			Managers.shaderManager = new ShaderManager(gl);
			Managers.entityManager = new EntityManager();
			Managers.cameraSystem = new CameraSystem();
			Managers.meshSystem = new MeshSystem(gl);
			Managers.renderingSystem = new RenderingSystem(gl);
			Managers.resourceManager = new ResourceManager(gl);

			systems = new List<EntitySystem> {
				Managers.cameraSystem,
				Managers.meshSystem,
				Managers.renderingSystem
			};

			Managers.inputHandler = new InputHandler(window.inputContext);
			
			var scriptEntity = Managers.entityManager.CreateEntity();
			Managers.resourceManager.Import_Script("test", Utils.FromAssets("./Scripts/Camera.lua"));
			Managers.entityManager.AddComponent(scriptEntity, Managers.resourceManager.Get_Script("test"));

			Managers.entityManager.OnLoad();
			InitializeEntities();
			Managers.inputHandler.Initialize();
		}

		private void InitializeEntities()
		{
			Console.WriteLine("InitializeEntities called");

			var cameraEntity = Managers.entityManager.CreateEntity();
			var cameraComponent = new CameraComponent();
			var transformComponent = new TransformComponent
			{
				Position = new Vector3D<float>(0.0f, 0.0f, 3.0f),
				Rotation = new Vector3D<float>(0.0f),
				Scale = new Vector3D<float>(1.0f)
			};

			cameraComponent.InitInput(transformComponent);
			Managers.inputHandler.inputStates.Add(cameraComponent.editor_state);

			Managers.entityManager.AddComponent(cameraEntity, cameraComponent);
			Managers.entityManager.AddComponent(cameraEntity, transformComponent);

			Managers.resourceManager.Import_Shader(
				"standardShader",
				Utils.FromAssets("./Shaders/vertex.glsl"),
				Utils.FromAssets("./Shaders/fragment.glsl")
			);
			Managers.resourceManager.Import_Texture("minecraft_dirt", Utils.FromAssets("./minecraft_dirt.png"));

			int location = window.GLContext.GetUniformLocation(Managers.resourceManager.Get_Shader("standardShader").ShaderProgramId, "texture1");
			window.GLContext.Uniform1(location, Managers.resourceManager.Get_Texture("minecraft_dirt").Id);
			
			Managers.meshSystem.BindMesh(Managers.resourceManager.models["testMesh"]);

			Console.WriteLine("Mesh bound");
		}

		private void OnRender(double deltaTime)
		{
			Console.WriteLine("OnRender called");
			var gl = window.GLContext;
			gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Time.DeltaTime = deltaTime;

			foreach (var system in systems)
			{
				if (system is RenderingSystem renderingSystem)
				{
					renderingSystem.Update(Managers.entityManager);
				}
			}

			CheckForErrors();
		}

		private void CheckForErrors()
		{
			var gl = window.GLContext;
			var error = gl.GetError();
			if (error != GLEnum.NoError)
			{
				Console.WriteLine($"OpenGL Error: {error}");
			}
		}

		private void OnUpdate(double deltaTime)
		{
			Time.DeltaTime = deltaTime;
			Time.TimeElapsed += deltaTime;

			foreach (var system in systems)
			{
				if (!(system is RenderingSystem))
				{
					system.Update(Managers.entityManager);
				}
			}

			Managers.entityManager.Update();
			Managers.inputHandler.Update();
		}

		private void OnClosing()
		{
			Managers.resourceManager.Detach_All();
		}

		public void Run()
		{
			window.Run();
		}

		public void Shutdown()
		{
		}
	}
}