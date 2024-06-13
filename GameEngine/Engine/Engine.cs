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
			Managers._GL = window.GLContext;

			systems = new List<EntitySystem> {
				Managers.cameraSystem,
				Managers.meshSystem,
				Managers.renderingSystem
			};

			Managers.inputHandler = new InputHandler(window.inputContext);
			
			var scriptEntity = Managers.entityManager.CreateEntity();
			Managers.resourceManager.Import_PyScript("main", "./PyScripts/Main.py");
			Managers.entityManager.AddComponent(scriptEntity, Managers.resourceManager.Get_PyScript("main"));

			Managers.resourceManager.Get_PyScript("main").ExecuteScript();

			Managers.entityManager.OnLoad();
			Managers.inputHandler.Initialize();
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