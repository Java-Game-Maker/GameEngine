
using Silk.NET.OpenGL;

namespace GameEngine
{
	public static class Managers
	{
		public static GL _GL;
		public static ShaderManager shaderManager;
		public static MeshSystem meshSystem;
		public static ScriptLuaSystem scriptLuaSystem;
		public static TransformSystem transformSystem;
		public static CameraSystem cameraSystem;
		public static EntityManager entityManager;
		public static RenderingSystem renderingSystem;
		public static ResourceManager resourceManager;
		public static InputHandler inputHandler;
	}
}