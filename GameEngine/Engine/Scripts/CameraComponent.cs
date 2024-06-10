using Silk.NET.Maths;

namespace GameEngine
{
	public class CameraComponent : EntityComponent
	{
		public Vector3D<float> Front	{ get; set; }
		public Vector3D<float> Up		{ get; set; }
		public Vector3D<float> Right	{ get; set; }
		public Vector3D<float> WorldUp	{ get; set; }
		public float Speed			{ get; set; }
		public float Sensitivity	{ get; set; }
		public float Zoom			{ get; set; }
	}
}