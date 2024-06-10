
using Silk.NET.OpenGL;

namespace GameEngine
{
	public enum ResourceType
	{
		Texture,
		Sound,
		Shader,
		Obj
	}

	public class ResourceManager
	{
		private readonly GL gl;

		private readonly Dictionary<string, MeshComponent> objects = new Dictionary<string, MeshComponent>();
		private readonly Dictionary<string, ShaderComponent> shaders = new Dictionary<string, ShaderComponent>();
	}
}