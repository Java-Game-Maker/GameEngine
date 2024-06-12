
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace GameEngine
{
	public class GameObject
	{
		public Entity entity;
		public MeshComponent meshComponent;
		public ShaderComponent shaderComponent;
		public TransformComponent transformComponent;

		public GameObject(ResourceManager resourceManager, EntityManager entityManager, ShaderComponent _shaderComponent, string meshPath, string meshName)
		{
			resourceManager.Import_Model(meshName, meshPath);

			entity = entityManager.CreateEntity();
			meshComponent = resourceManager.Get_Model(meshName);
			shaderComponent = _shaderComponent;
			transformComponent = new TransformComponent{
				Position = new Vector3D<float>(-5.0f, 0.0f, 0.0f),
				Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f),
				Scale = new Vector3D<float>(1.0f, 1.0f, 1.0f)
			};

			entityManager.AddComponent(entity, meshComponent);
			entityManager.AddComponent(entity, shaderComponent);
			entityManager.AddComponent(entity, transformComponent);
		}
	}
}