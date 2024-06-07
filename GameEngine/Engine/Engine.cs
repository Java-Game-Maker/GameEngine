using System;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace GameEngine
{
	public class Engine
	{
		private static IWindow _window;
		private static IInputContext input;

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
			input = _window.CreateInput();
			for(int i = 0; i < input.Keyboards.Count; i++)
			{
				input.Keyboards[i].KeyDown += KeyDown;
			}
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

		private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
		{
			if(key == Key.Escape)
			{
				_window.Close();
			}
		}
	}
}