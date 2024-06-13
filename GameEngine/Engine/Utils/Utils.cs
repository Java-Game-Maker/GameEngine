using System;

namespace GameEngine
{
	public static class Utils
	{
		public static string FromAssets(string path)
		{
			if(Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				return AppDomain.CurrentDomain.BaseDirectory + "../../../Assets/" + path;
			}
			else
			{
				return "./Assets/" + path;
			}
		}
	}
}