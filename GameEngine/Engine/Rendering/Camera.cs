using System;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace GameEngine
{
	public class Camera
	{
		public Vector3 Position { get; set; }
        public Vector3 Front { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        public Vector3 WorldUp { get; private set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Speed { get; set; }
        public float Sensitivity { get; set; }
        public float Zoom { get; set; }

        private IMouse mouse;
        private Vector2 lastMousePosition;
        private bool firstMouse = true;

        public Camera(Vector3 position)
        {
            Position = position;
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            Up = Vector3.UnitY;
            Right = Vector3.UnitX;
            WorldUp = Vector3.UnitY;
            Yaw = -90.0f; // Yaw set to -90.0 degrees to initially look forward
            Pitch = 0.0f;
            Speed = 2.5f;
            Sensitivity = 0.1f;
            Zoom = 45.0f;
            UpdateCameraVectors();
        }

        public void Initialize(IMouse mouse)
        {
            this.mouse = mouse;
            this.mouse.MouseMove += OnMouseMove;
        }

        private void OnMouseMove(IMouse mouse, Vector2 position)
        {
            if (firstMouse)
            {
                lastMousePosition = position;
                firstMouse = false;
            }

            var xoffset = (position.X - lastMousePosition.X) * Sensitivity;
            var yoffset = (lastMousePosition.Y - position.Y) * Sensitivity; // Reversed since y-coordinates go from bottom to top

            lastMousePosition = position;

            Yaw += xoffset;
            Pitch += yoffset;

            if (Pitch > 89.0f)
                Pitch = 89.0f;
            if (Pitch < -89.0f)
                Pitch = -89.0f;

            UpdateCameraVectors();
        }

        public Matrix4X4<float> GetViewMatrix()
        {
            var lookAt = Matrix4X4.CreateLookAt(
                new Vector3D<float>(Position.X, Position.Y, Position.Z),
                new Vector3D<float>(Position.X + Front.X, Position.Y + Front.Y, Position.Z + Front.Z),
                new Vector3D<float>(Up.X, Up.Y, Up.Z)
            );
            return lookAt;
        }

        public Matrix4X4<float> GetProjectionMatrix(float aspectRatio)
        {
            var projection = Matrix4X4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(Zoom),
                aspectRatio,
                0.1f,
                100.0f
            );
            return projection;
        }

        public void ProcessInput(IInputContext input, float deltaTime)
        {
            var keyboard = input.Keyboards[0];
            float velocity = Speed * deltaTime;

            if (keyboard.IsKeyPressed(Key.W))
                Position += Front * velocity;
            if (keyboard.IsKeyPressed(Key.S))
                Position -= Front * velocity;
            if (keyboard.IsKeyPressed(Key.A))
                Position -= Right * velocity;
            if (keyboard.IsKeyPressed(Key.D))
                Position += Right * velocity;

            UpdateCameraVectors();
        }

        private void UpdateCameraVectors()
        {
            Vector3 front;
            front.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            front.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            Front = Vector3.Normalize(front);
            // Recalculate Right and Up vectors
            Right = Vector3.Normalize(Vector3.Cross(Front, WorldUp));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }
	}

	public static class MathHelper
    {
        public static float DegreesToRadians(float degrees)
        {
            return degrees * (MathF.PI / 180.0f);
        }
    }
}