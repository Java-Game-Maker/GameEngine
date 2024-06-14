
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

		public GameObject(string meshPath, string meshName)
		{
			Managers.resourceManager.Import_Model(meshName, Utils.FromAssets(meshPath));

			entity = Managers.entityManager.CreateEntity();
			meshComponent = Managers.resourceManager.Get_Model(meshName);
			shaderComponent = Managers.resourceManager.Get_Shader("standardShader");
			transformComponent = new TransformComponent{
				Position = new Vector3D<float>(0.0f, 0.0f, 0.0f),
				Rotation = new Vector3D<float>(0.0f, 0.0f, 0.0f),
				Scale = new Vector3D<float>(1.0f, 1.0f, 1.0f)
			};

			Managers.entityManager.AddComponent(entity, meshComponent);
			Managers.entityManager.AddComponent(entity, shaderComponent);
			Managers.entityManager.AddComponent(entity, transformComponent);
		}

		public void Translate(float x, float y, float z)
		{
			transformComponent.Position += new Vector3D<float>(x,y,z);
		}

		public void SetPosition(float x, float y, float z)
		{
			transformComponent.Position = new Vector3D<float>(x, y, z);
		}
	}
}