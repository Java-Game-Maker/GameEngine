using System.Runtime.InteropServices;
using System.Collections.Generic;
using FreeImageAPI;
using System;


namespace GameEngine
{
	public class RM_Texture
	{
		public static unsafe ImageData LoadImage(string path)
		{
			FIBITMAP dib = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_UNKNOWN, path, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			if(dib.IsNull)
			{
				throw new Exception("Failed to load image: " + path);
			}

			FIBITMAP converted = FreeImage.ConvertTo32Bits(dib);
			FreeImage.Unload(dib);

			byte[] data = new byte[4 * FreeImage.GetWidth(converted) * FreeImage.GetHeight(converted)];
			Marshal.Copy(FreeImage.GetBits(converted), data, 0, data.Length);

			uint width = FreeImage.GetWidth(converted);
			uint height = FreeImage.GetHeight(converted);
			FreeImage.Unload(converted);

			return new ImageData((int)width, (int)height, data);
		}
	}
}