using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using System.Windows;

namespace GameEngine
{
	public class WindowSettings
	{
		public string title { get; set; }
		public Silk.NET.Maths.Vector2D<int> size { get; set; }
		public WindowState windowState { get; set; }
		public WindowBorder windowBorder { get; set; }

		public void SyncSettings(IWindow _window)
		{
			_window.Title = title;
			_window.Size = size;
			_window.WindowState = windowState;
			_window.WindowBorder = windowBorder;
		}
	}

	public class Window
	{
		public IWindow WindowInstance {get; private set;}
		public GL GLContext {get; private set;}
		public IInputContext inputContext;
		public WindowSettings windowSettings;

		public void Initialize()
		{
			var options = WindowOptions.Default;
			windowSettings = new WindowSettings();
			windowSettings.title = "GameEngine";
			windowSettings.size = new Silk.NET.Maths.Vector2D<int>(1920, 1080);
			windowSettings.windowState = WindowState.Fullscreen;
			windowSettings.windowBorder = WindowBorder.Hidden;

			WindowInstance = Silk.NET.Windowing.Window.Create(options);
			WindowInstance.Load += OnLoad;
			WindowInstance.Closing += OnClosing;
			windowSettings.SyncSettings(WindowInstance);
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