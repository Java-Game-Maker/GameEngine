using System;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace GameEngine
{
	public class Engine
	{
		private static IWindow _window;

		public Engine()
		{
		}

		public void Initialize()
		{
			WindowOptions options = WindowOptions.Default with
			{
				Size = new Vector2D<int>(800, 600),
				Title = "GameEngine"
			};
			_window = Window.Create(options);

			_window.Load += OnLoad;
			_window.Render += OnRender;
			_window.Update += OnUpdate;
			_window.Closing += OnClosing;
		}

		private void OnLoad()
		{
		}

		private void OnRender(double deltaTime)
		{
		}

		private void OnUpdate(double deltaTime)
		{
		}

		private void OnClosing()
		{
		}

		public void Run()
		{
			_window.Run();
		}

		public void Shutdown()
		{
		}
	}
}