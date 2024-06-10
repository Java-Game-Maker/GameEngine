
namespace GameEngine
{
	public class ShaderComponent : EntityComponent
	{
		public uint ShaderProgramId { get; set; }

		public ShaderComponent(uint _shaderProgramId)
		{
			ShaderProgramId = _shaderProgramId;
		}
	}
}