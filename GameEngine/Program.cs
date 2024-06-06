using System;

namespace GameEngine
{
	class Program
	{
		static void Main(string[] args)
		{
			Engine engine = new Engine();
			engine.Initialize();
			engine.Run();
		}
	}
}