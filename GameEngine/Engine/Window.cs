using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using System.Windows;

namespace GameEngine
{
	public class Window
	{
		public IWindow WindowInstance {get; private set;}
		public GL GLContext {get; private set;}
		public IInputContext inputContext;

		public void Initialize()
		{
			var options = WindowOptions.Default;
			options.Title = "GameEngine";
			options.Size = new Silk.NET.Maths.Vector2D<int>(1920, 1080);
			options.WindowState = WindowState.Fullscreen;
			options.WindowBorder = WindowBorder.Hidden;

			WindowInstance = Silk.NET.Windowing.Window.Create(options);
			WindowInstance.Load += OnLoad;
			WindowInstance.Closing += OnClosing;

			WindowInstance.WindowState = WindowState.Fullscreen;
		}

		private void OnLoad()
		{
			inputContext = WindowInstance.CreateInput();
			inputContext.Mice[0].Cursor.CursorMode = CursorMode.Disabled;
			GLContext = GL.GetApi(WindowInstance);
		}

		private void OnClosing()
		{
			inputContext.Dispose();
		}

		public void Run()
		{
			WindowInstance.Run();
		}
	}
}