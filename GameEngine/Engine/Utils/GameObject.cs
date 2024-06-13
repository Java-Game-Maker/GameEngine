
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace GameEngine
{
	public class GameObject
	{
		public Entity entity;

		public GameObject(string meshPath, string meshName)
		{
			Managers.resourceManager.Import_Model(meshName, meshPath);

			entity = Managers.entityManager.CreateEntity();
			var meshComponent = Managers.resourceManager.Get_Model(meshName);
			var shaderComponent = Managers.resourceManager.Get_Shader("standardShader");;
			var transformComponent = new TransformComponent{
				Position = new Vector3D<float>(-5.0f, 0.0f, 0.0f),
				Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f),
				Scale = new Vector3D<float>(1.0f, 1.0f, 1.0f)
			};

			Managers.entityManager.AddComponent(entity, meshComponent);
			Managers.entityManager.AddComponent(entity, shaderComponent);
			Managers.entityManager.AddComponent(entity, transformComponent);
		}
	}
}