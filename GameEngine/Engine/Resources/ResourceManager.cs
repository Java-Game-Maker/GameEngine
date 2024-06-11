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

	public enum ResourceState
	{
		Detached,
		Loading,
		Loaded
	}

	public class ResourceManager
	{
		private readonly GL gl;

		private readonly Dictionary<string, MeshComponent> objects = new Dictionary<string, MeshComponent>();
		private readonly Dictionary<string, ShaderComponent> shaders = new Dictionary<string, ShaderComponent>();

		public ResourceManager(GL _gl)
		{
			gl = _gl;
		}
	}
}