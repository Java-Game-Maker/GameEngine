using Silk.NET.Maths;
using Silk.NET.Input;

namespace GameEngine
{
    public class CameraComponent : EntityComponent
    {
        public Vector3D<float> Front { get; set; } = new Vector3D<float>(0.0f, 0.0f, -1.0f);
        public Vector3D<float> Up { get; set; } = new Vector3D<float>(0.0f, 1.0f, 0.0f);
        public Vector3D<float> Right { get; set; } = new Vector3D<float>(1.0f, 0.0f, 0.0f);
        public Vector3D<float> WorldUp { get; set; } = new Vector3D<float>(0.0f, 1.0f, 0.0f);
        public float Speed { get; set; } = 2.5f;
        public float walkSpeed { get; set; } = 2.5f;
        public float runSpeed { get; set; } = 2.5f;
        public float Sensitivity { get; set; } = 0.1f; // Adjust sensitivity to a reasonable value
        public float Zoom { get; set; } = 90.0f;

        public InputState editor_state { get; set; }

        public float Yaw { get; set; } = -90.0f; // Initialized to look along the negative z-axis
        public float Pitch { get; set; } = 0.0f;

        private bool firstMouse = true;
        private float lastX = 0.0f;
        private float lastY = 0.0f;

        public void InitInput(TransformComponent transformComponent)
        {
            float velocity(double td) => Speed * (float)td;

            editor_state = new InputState();
            editor_state.Bind_OnKeyHeld.Add(Key.W, () => { transformComponent.Position += Front * velocity(Time.DeltaTime); });
            editor_state.Bind_OnKeyHeld.Add(Key.S, () => { transformComponent.Position -= Front * velocity(Time.DeltaTime); });
            editor_state.Bind_OnKeyHeld.Add(Key.A, () => { transformComponent.Position -= Right * velocity(Time.DeltaTime); });
            editor_state.Bind_OnKeyHeld.Add(Key.D, () => { transformComponent.Position += Right * velocity(Time.DeltaTime); });
            editor_state.Bind_OnKeyHeld.Add(Key.ShiftLeft, () => { transformComponent.Position -= Up * velocity(Time.DeltaTime); });
            editor_state.Bind_OnKeyHeld.Add(Key.Space, () => { transformComponent.Position += Up * velocity(Time.DeltaTime); });

            editor_state.Bind_OnKeyDown.Add(Key.Number1, () => {Managers.renderingSystem.world_time -= 1;});
            editor_state.Bind_OnKeyDown.Add(Key.Number2, () => {Managers.renderingSystem.world_time += 1;});

            editor_state.Bind_OnKeyHeld.Add(Key.ControlLeft, () => { Speed = (Speed == 2.5f) ? 10 : 2.5f; });

            editor_state.Bind_OnMouseMove = (xOffset, yOffset) =>
            {
                if (firstMouse)
                {
                    lastX = xOffset;
                    lastY = yOffset;
                    firstMouse = false;
                }

                float xDiff = xOffset - lastX;
                float yDiff = lastY - yOffset;

                lastX = xOffset;
                lastY = yOffset;

                xDiff *= Sensitivity;
                yDiff *= Sensitivity;

                Yaw = (Yaw + xDiff) % 360.0f;
                Pitch = Math.Clamp(Pitch + yDiff, -89.0f, 89.0f);

                // Recalculate Front vector directly from Yaw and Pitch
                Vector3D<float> front;
                front.X = MathF.Cos(float.DegreesToRadians(Yaw)) * MathF.Cos(/* float.DegreesToRadians(Pitch) */0);
                front.Y = MathF.Sin(/* float.DegreesToRadians(Pitch) */0);
                front.Z = MathF.Sin(float.DegreesToRadians(Yaw)) * MathF.Cos(/* float.DegreesToRadians(Pitch) */0);
                Front = Vector3D.Normalize(front);

                Right = Vector3D.Normalize(Vector3D.Cross(Front, WorldUp));
                Up = Vector3D.Normalize(Vector3D.Cross(Right, Front));
            };
        }
    }
}