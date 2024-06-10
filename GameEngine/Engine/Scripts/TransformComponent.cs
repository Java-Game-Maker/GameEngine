
using Silk.NET.Maths;

namespace GameEngine
{
	public class TransformComponent : EntityComponent
	{
		public Vector3D<float> Position { get; set; }
		public Vector3D<float> Rotation { get; set; }
		public Vector3D<float> Scale { get; set; }
	}
}