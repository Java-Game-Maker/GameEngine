
using Silk.NET.OpenGL;

namespace GameEngine
{
	public class MeshComponent : EntityComponent
	{
		public float[] vertices { get; set; }
		public uint[] indices { get; set; }

		public uint VAO { get; set; }
		public uint VBO { get; set; }
		public uint EBO { get; set; }
		public uint IndexCount { get; set; }

		public MeshComponent(float[] _vertices, uint[] _indices)
		{
			vertices = _vertices;
			indices = _indices;
			IndexCount = (uint)_indices.Length;
			/* VAO = vao;
			VBO = vbo;
			EBO = ebo;
			IndexCount = indexCount; */
		}
	}
}