
namespace GameEngine
{
	public class ImageData
	{
		public int Width { get; }
		public int Height { get; }
		public byte[] Data { get; }

		public ImageData(int width, int height, byte[] data)
		{
			Width = width;
			Height = height;
			Data = data;
		}
	}
}