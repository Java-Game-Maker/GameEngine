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
				Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f)
			};

			cameraComponent.InitInput(transformComponent);
			inputHandler.inputStates.Add(cameraComponent.editor_state);

			entityManager.AddComponent(cameraEntity, cameraComponent);
			entityManager.AddComponent(cameraEntity, transformComponent);

			string vertexShaderSource = File.ReadAllText("./Shaders/vertex.glsl");
			string fragmentShaderSource = File.ReadAllText("./Shaders/fragment.glsl");
			uint shaderProgram = shaderManager.CreateShaderProgram(vertexShaderSource, fragmentShaderSource);
			Console.WriteLine($"Shader program created: {shaderProgram}");

			var meshEntity = entityManager.CreateEntity();

			float[] vertices = {
				// positions          // colors
				0.5f,  0.5f, 0.0f,
				0.5f, -0.5f, 0.0f,
				-0.5f, -0.5f, 0.0f,
				-0.5f,  0.5f, 0.0f,
			};
			uint[] indices = {
				0, 1, 3,
				1, 2, 3
			};

			var meshComponent = new MeshComponent(vertices, indices);
			meshComponent.Normals = meshComponent.CalculateNormals(meshComponent.vertices, meshComponent.indices);
			var shaderComponent = new ShaderComponent(shaderProgram);
			var meshTransformComponent = new TransformComponent
			{
				Position = new Vector3D<float>(-10.0f, 0.0f, 0.0f),
				Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f),
				Scale = new Vector3D<float>(1.0f, 1.0f, 1.0f)
			};

			entityManager.AddComponent(meshEntity, meshComponent);
			entityManager.AddComponent(meshEntity, shaderComponent);
			entityManager.AddComponent(meshEntity, meshTransformComponent);

			meshSystem.BindMesh(meshComponent);

			// NEW MESH //

			var meshEntity2 = entityManager.CreateEntity();

			var mesh2Component = RM_Obj.LoadOBJ("./week2.obj");
			var shader2Component = new ShaderComponent(shaderProgram);
			var mesh2TransformComponent = new TransformComponent
			{
				Position = new Vector3D<float>(0.0f, 0.0f, 0.0f),
				Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f),
				Scale = new Vector3D<float>(1.0f, 1.0f, 1.0f)
			};

			entityManager.AddComponent(meshEntity2, mesh2Component);
			entityManager.AddComponent(meshEntity2, shader2Component);
			entityManager.AddComponent(meshEntity2, mesh2TransformComponent);

			meshSystem.BindMesh(mesh2Component);

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