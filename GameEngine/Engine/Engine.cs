using System;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace GameEngine
{
	public class Engine
	{
		private Window window;
		public InputHandler inputHandler;
		private Renderer renderer;
		private Camera camera;
		private bool running;

		public Engine()
		{
			window = new Window();
			camera = new Camera(new System.Numerics.Vector3(0.0f, 0.0f, 3.0f));
			renderer = new Renderer(camera);
		}

		public void Initialize()
		{
			window.Initialize();
			window.WindowInstance.Load += OnLoad;
			window.WindowInstance.Render += OnRender;
			window.WindowInstance.Update += OnUpdate;
		}

		private void OnLoad()
		{
			renderer.Initialize(window.GLContext);
			inputHandler = new InputHandler(window.inputContext);

			var input = window.inputContext;
			var mouse = input.Mice[0];
			camera.Initialize(mouse, inputHandler);
			inputHandler.Initialize();
		}

		private void OnRender(double deltaTime)
		{
			Time.DeltaTime = deltaTime;

			float aspectRatio = (float)window.WindowInstance.Size.X / (float)window.WindowInstance.Size.Y;
			renderer.Render(aspectRatio);
		}

		private void OnUpdate(double deltaTime)
		{
			Time.DeltaTime = deltaTime;
			Time.TimeElapsed += deltaTime;
			inputHandler.Update();
			camera.ProcessInput(window.inputContext, (float)deltaTime);
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