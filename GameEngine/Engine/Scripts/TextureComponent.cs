
namespace GameEngine
{
	public class Texture : EntityComponent
	{
		public uint Id { get; }
		public int Width { get; }
		public int Height { get; }

		public Texture(uint id, int width, int height)
		{
			Id = id;
			Width = width;
			Height = height;
		}
	}
}