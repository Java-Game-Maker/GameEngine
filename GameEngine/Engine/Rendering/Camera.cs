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
			Yaw = -90.0f;
			Pitch = 0.0f;
			Speed = 2.5f;
			Sensitivity = 0.1f;
			Zoom = 90.0f;
			UpdateCameraVectors();
		}

		public void Initialize(IMouse mouse, InputHandler inputHandler)
		{
			this.mouse = mouse;
			this.mouse.MouseMove += OnMouseMove;

			float velocity(double td) => (Speed * (float)td);
			
			InputState editor_state = new InputState();
			editor_state.Bind_OnKeyHeld.Add(Key.W		 , () => {Position += Front * velocity(Time.DeltaTime);});
			editor_state.Bind_OnKeyHeld.Add(Key.S		 , () => {Position -= Front * velocity(Time.DeltaTime);});
			editor_state.Bind_OnKeyHeld.Add(Key.A		 , () => {Position -= Right * velocity(Time.DeltaTime);});
			editor_state.Bind_OnKeyHeld.Add(Key.D		 , () => {Position += Right * velocity(Time.DeltaTime);});
			editor_state.Bind_OnKeyHeld.Add(Key.ShiftLeft, () => {Position -= Up * 	  velocity(Time.DeltaTime);});
			editor_state.Bind_OnKeyHeld.Add(Key.Space	 , () => {Position += Up * 	  velocity(Time.DeltaTime);});
			inputHandler.inputStates.Add(editor_state);
		}

		private void OnMouseMove(IMouse mouse, Vector2 position)
		{
			if (firstMouse)
			{
				lastMousePosition = position;
				firstMouse = false;
			}

			var xoffset = (position.X - lastMousePosition.X) * Sensitivity;
			var yoffset = (lastMousePosition.Y - position.Y) * Sensitivity;

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
			UpdateCameraVectors();
		}

		private void UpdateCameraVectors()
		{
			Vector3 front;
			front.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
			front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
			front.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
			Front = Vector3.Normalize(front);

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