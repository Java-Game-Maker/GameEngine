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
		private ShaderManager shaderManager;
		private CameraSystem cameraSystem;
		private MeshSystem meshSystem;
		private RenderingSystem renderingSystem;
		private ResourceManager resourceManager;

		public InputHandler inputHandler;

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

			shaderManager = new ShaderManager(gl);
			entityManager = new EntityManager();
			cameraSystem = new CameraSystem();
			meshSystem = new MeshSystem(gl);
			renderingSystem = new RenderingSystem(gl, shaderManager, cameraSystem, meshSystem);
			resourceManager = new ResourceManager(gl);

			systems = new List<EntitySystem> {
				cameraSystem,
				meshSystem,
				renderingSystem
			};

			inputHandler = new InputHandler(window.inputContext);
			InitializeEntities();
			inputHandler.Initialize();
		}

		private void InitializeEntities()
		{
			Console.WriteLine("InitializeEntities called");

			var cameraEntity = entityManager.CreateEntity();
			var cameraComponent = new CameraComponent();
			var transformComponent = new TransformComponent
			{
				Position = new Vector3D<float>(0.0f, 0.0f, 3.0f),
				Rotation = new Vector3D<float>(0.0f),
				Scale = new Vector3D<float>(1.0f)
			};

			cameraComponent.InitInput(transformComponent);
			inputHandler.inputStates.Add(cameraComponent.editor_state);

			entityManager.AddComponent(cameraEntity, cameraComponent);
			entityManager.AddComponent(cameraEntity, transformComponent);

			resourceManager.Import_Shader(shaderManager, "standardShader", "./Shaders/vertex.glsl", "./Shaders/fragment.glsl");
			resourceManager.Import_Texture("minecraft_dirt", "./Assets/minecraft_dirt.png");

			int location = window.GLContext.GetUniformLocation(resourceManager.Get_Shader("standardShader").ShaderProgramId, "texture1");
			window.GLContext.Uniform1(location, resourceManager.Get_Texture("minecraft_dirt").Id);

			GameObject go1 = new GameObject(resourceManager, entityManager, resourceManager.Get_Shader("standardShader"), "./cube.obj", "cube2");
			GameObject go2 = new GameObject(resourceManager, entityManager, resourceManager.Get_Shader("standardShader"), "./cube.obj", "cube");
			go2.transformComponent.Position = new Vector3D<float>(3.0f, 0.0f, 0.0f);
			go2.transformComponent.Scale = new Vector3D<float>(0.5f, 0.5f, 0.5f);

			meshSystem.BindMesh(go1.meshComponent);
			meshSystem.BindMesh(go2.meshComponent);

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
					renderingSystem.Update(entityManager);
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
					system.Update(entityManager);
				}
			}

			inputHandler.Update();
		}

		private void OnClosing()
		{
			resourceManager.Detach_All();
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