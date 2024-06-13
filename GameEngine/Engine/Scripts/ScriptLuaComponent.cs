
using System.Collections;
using Silk.NET.Maths;
using Assimp;
using NLua;

namespace GameEngine
{
	public class ScriptLuaComponent : EntityComponent
	{
		public string _Path;
		private Lua _lua;
		private List<LuaFunction> _onLoadFunctions;
		private List<LuaFunction> _onUpdateFunctions;

		public ScriptLuaComponent(string path)
		{
			_lua = new Lua();
			
			_onLoadFunctions = new List<LuaFunction>();
			_onUpdateFunctions = new List<LuaFunction>();

			RegisterFunctions();
			LoadStandardLibrary();
			_Path = path;
			ExecuteScriptFile(_Path);
		}

		private void RegisterFunctions()
		{
			_lua["entityManager"] = Managers.entityManager;
			_lua["DeltaTime"] = (float)Time.DeltaTime;
			_lua.RegisterFunction("registerOnLoad", this, GetType().GetMethod("RegisterOnLoad"));
			_lua.RegisterFunction("registerUpdate", this, GetType().GetMethod("RegisterUpdate"));

			_lua.RegisterFunction("_createEntity", this, GetType().GetMethod("CreateEntity"));
			_lua.RegisterFunction("_addComponent", this, GetType().GetMethod("AddComponent"));
			_lua.RegisterFunction("_updateTransform", this, GetType().GetMethod("UpdateTransform"));
		}

		public void UpdateTransform(int _id, float x, float y, float z)
		{
			var entity = new Entity{ Id = _id };
			var transform = Managers.entityManager.GetComponent<TransformComponent>(entity);
			transform.Position = new Vector3D<float>(x, y, z);
		}

		private void LoadStandardLibrary()
		{
			var standardLibrary = File.ReadAllText(Utils.FromAssets("./Scripts/StandardLibrary/standard.lua"));
			_lua.DoString(standardLibrary);
		}

		public void LoadScript(string scriptPath)
		{
			_lua.DoString(scriptPath);
		}

		public void ExecuteScriptFile(string FilePath)
		{
			_lua.DoFile(FilePath);
		}

		public void RegisterOnLoad(LuaFunction function)
		{
			_onLoadFunctions.Add(function);
		}

		public void RegisterUpdate(LuaFunction function)
		{
			_onUpdateFunctions.Add(function);
		}

		public void OnLoad()
		{
			foreach (var _func in _onLoadFunctions)
			{
				_func.Call();
			}
		}

		public void Update()
		{
			_lua["DeltaTime"] = (float)Time.DeltaTime;
			foreach (var _func in _onUpdateFunctions)
			{
				_func.Call();
			}
		}

		public int CreateEntity()
		{
			var entity = Managers.entityManager.CreateEntity();
			return entity.Id;
		}

		public void AddComponent(int entityId, string componentType, LuaTable parameters)
		{
			var entity = new Entity {Id = entityId};

			switch(componentType)
			{
				case "Camera": 
					Managers.entityManager.AddComponent(entity, new CameraComponent());
					break;
				case "Mesh":
					Managers.resourceManager.Import_Model(
						parameters["name"].ToString(),
						Utils.FromAssets(parameters["path"].ToString())
					);
					Managers.entityManager.AddComponent(entity, Managers.resourceManager.Get_Model(parameters["name"].ToString()));
					break;
				case "Shader":
					Managers.resourceManager.Import_Shader(
						parameters["name"].ToString(),
						Utils.FromAssets(parameters["vertPath"].ToString()),
						Utils.FromAssets(parameters["fragPath"].ToString())
					);
					Managers.entityManager.AddComponent(entity, Managers.resourceManager.Get_Shader(parameters["name"].ToString()));
					break;
				case "Transform":
					Managers.entityManager.AddComponent(entity, new TransformComponent
					{
						Position = new Vector3D<float>(0, 5, 0),
						Rotation = new Vector3D<float>(0.0f),
						Scale = new Vector3D<float>(1)
					});
					break;
				default: throw new ArgumentException("Invalid component type");
			};
		}
	}
}