using Silk.NET.Maths;

namespace GameEngine
{
	public class CameraSystem : EntitySystem
	{
		private Matrix4X4<float> viewMatrix;
		private Matrix4X4<float> projectionMatrix;

		public override void Update(EntityManager entityManager)
		{
			var entities = entityManager.GetAllEntitiesWithComponent<CameraComponent>();

			if(entities.Count > 0)
			{
				var activeCameraEntity = entities[0];
				var cameraComponent = entityManager.GetComponent<CameraComponent>(activeCameraEntity);
				var TransformComponent = entityManager.GetComponent<TransformComponent>(activeCameraEntity);

				UpdateCameraVectors(cameraComponent, TransformComponent);

				viewMatrix = CalculateViewMatrix(cameraComponent, TransformComponent);
				projectionMatrix = CalculateProjectionMatrix(1920/1080, cameraComponent); // NOTE: Pass an aspect ratio later somehow.
			}
			
		}

		public Matrix4X4<float> CalculateViewMatrix(CameraComponent cameraComponent, TransformComponent transform)
		{
			var lookAt = Matrix4X4.CreateLookAt(
				new Vector3D<float>(transform.Position.X, transform.Position.Y, transform.Position.Z),
				new Vector3D<float>(transform.Position.X + cameraComponent.Front.X, transform.Position.Y + cameraComponent.Front.Y, transform.Position.Z + cameraComponent.Front.Z),
				new Vector3D<float>(cameraComponent.Up.X, cameraComponent.Up.Y, cameraComponent.Up.Z)
			);
			return lookAt;
		}

		public Matrix4X4<float> CalculateProjectionMatrix(float aspectRatio, CameraComponent cameraComponent)
		{
			var projection = Matrix4X4.CreatePerspectiveFieldOfView(
				MathHelper.DegreesToRadians(cameraComponent.Zoom),
				aspectRatio,
				0.1f,
				100.0f
			);
			return projection;
		}

		private void UpdateCameraVectors(CameraComponent cameraComponent, TransformComponent transform)
		{
			Vector3D<float> front;
			front.X = MathF.Cos(float.DegreesToRadians(transform.Rotation.Y)) * MathF.Cos(float.DegreesToRadians(transform.Rotation.X));
			front.Y = MathF.Sin(float.DegreesToRadians(transform.Rotation.X));
			front.Z = MathF.Sin(float.DegreesToRadians(transform.Rotation.Y)) * MathF.Cos(float.DegreesToRadians(transform.Rotation.X));
			cameraComponent.Front = Vector3D.Normalize<float>(front);

			cameraComponent.Right	= Vector3D.Normalize<float>(Vector3D.Cross<float>(cameraComponent.Front, cameraComponent.WorldUp));
			cameraComponent.Up		= Vector3D.Normalize<float>(Vector3D.Cross<float>(cameraComponent.Right, cameraComponent.Front));
		}

		public Matrix4X4<float> GetViewMatrix() => viewMatrix;
		public Matrix4X4<float> GetProjectionMatrix() => projectionMatrix;
	}
}