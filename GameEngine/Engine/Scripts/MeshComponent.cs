
namespace GameEngine
{
	public class MeshComponent : EntityComponent
	{
		public uint VAO { get; set; }
		public uint VBO { get; set; }
		public uint EBO { get; set; }
		public uint IndexCount { get; set; }

		public MeshComponent(uint vao, uint vbo, uint ebo, uint indexCount)
		{
			VAO = vao;
			VBO = vbo;
			EBO = ebo;
			IndexCount = indexCount;
		}
	}
}