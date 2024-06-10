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

            if (entities.Count > 0)
            {
                var activeCameraEntity = entities[0];
                var cameraComponent = entityManager.GetComponent<CameraComponent>(activeCameraEntity);
                var transformComponent = entityManager.GetComponent<TransformComponent>(activeCameraEntity);

                UpdateCameraVectors(cameraComponent);

                viewMatrix = CalculateViewMatrix(cameraComponent, transformComponent);
                projectionMatrix = CalculateProjectionMatrix(1920f / 1080f, cameraComponent); // NOTE: Pass an aspect ratio later somehow.
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
                float.DegreesToRadians(cameraComponent.Zoom),
                aspectRatio,
                0.1f,
                100.0f
            );
            return projection;
        }

        private void UpdateCameraVectors(CameraComponent cameraComponent)
        {
            Vector3D<float> front;
            front.X = MathF.Cos(float.DegreesToRadians(cameraComponent.Yaw)) * MathF.Cos(float.DegreesToRadians(cameraComponent.Pitch));
            front.Y = MathF.Sin(float.DegreesToRadians(cameraComponent.Pitch));
            front.Z = MathF.Sin(float.DegreesToRadians(cameraComponent.Yaw)) * MathF.Cos(float.DegreesToRadians(cameraComponent.Pitch));
            cameraComponent.Front = Vector3D.Normalize<float>(front);

            cameraComponent.Right = Vector3D.Normalize<float>(Vector3D.Cross<float>(cameraComponent.Front, cameraComponent.WorldUp));
            cameraComponent.Up = Vector3D.Normalize<float>(Vector3D.Cross<float>(cameraComponent.Right, cameraComponent.Front));

            // Debug output
            Console.WriteLine($"Updated Camera Vectors: Front={cameraComponent.Front}, Right={cameraComponent.Right}, Up={cameraComponent.Up}");
        }

        public Matrix4X4<float> GetViewMatrix() => viewMatrix;
        public Matrix4X4<float> GetProjectionMatrix() => projectionMatrix;
    }
}