
using System;
using IronPython;
using IronPython.Hosting;
using IronPython.Runtime.Operations;
using IronPython.Runtime.Types;
using Microsoft.Scripting.Hosting;

namespace GameEngine
{
	public class ScriptPythonComponent : EntityComponent
	{
		private string _path;
		private ScriptEngine _engine;
		private ScriptScope _scope;

		private dynamic _onLoad;
		private dynamic _update;

		public ScriptPythonComponent(string _path)
		{
			this._path = Utils.FromAssets(_path);
			_engine = Python.CreateEngine();
			_scope = _engine.CreateScope();

			var paths = _engine.GetSearchPaths();
			paths.Add("T:\\githubRepos\\GameEngine\\GameEngine\\Assets\\PyScripts\\");
			_engine.SetSearchPaths(paths);
			foreach (var item in paths)
			{
				Console.WriteLine(item);
			}

			_scope.SetVariable("Managers", DynamicHelpers.GetPythonTypeFromType(typeof(Managers)));
			_scope.SetVariable("GameObject", DynamicHelpers.GetPythonTypeFromType(typeof(GameObject)));
			_scope.SetVariable("CameraComponent", DynamicHelpers.GetPythonTypeFromType(typeof(CameraComponent)));
			_scope.SetVariable("TransformComponent", DynamicHelpers.GetPythonTypeFromType(typeof(TransformComponent)));
			_scope.SetVariable("Utils", DynamicHelpers.GetPythonTypeFromType(typeof(Utils)));
			_scope.SetVariable("Time", DynamicHelpers.GetPythonTypeFromType(typeof(Time)));
		}

		public void ExecuteScript()
		{
			try
			{
				var scriptSource = _engine.CreateScriptSourceFromFile(_path);
				scriptSource.Execute(_scope);

				_onLoad = _scope.GetVariable("Start");
				_update = _scope.GetVariable("Update");
			}
			catch (System.Exception ex)
			{
				Console.WriteLine($"Script executed failed: {ex.Message}");
				Console.WriteLine(ex.StackTrace);
			}
		}

		public void OnLoad()
		{
			try
			{
				_onLoad();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"onLoad execution failed: {ex.Message}");
			}
		}

		public void Update()
		{
			try
			{
				_update();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Update execution failed: {ex.Message}");
			}
		}
	}
}