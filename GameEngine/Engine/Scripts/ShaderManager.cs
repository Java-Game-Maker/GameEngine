
using Silk.NET.OpenGL;

namespace GameEngine
{
	public class ShaderManager
	{
		private GL gl;
		private Dictionary<string, uint> shaderPrograms = new Dictionary<string, uint>();

		public ShaderManager(GL _gl)
		{
			this.gl = _gl;
		}

		public uint CreateShaderProgram(string vertexShaderSource, string fragmentShaderSource)
		{
			var vertexShader = CompileShader(vertexShaderSource, ShaderType.VertexShader);
			var fragmentShader = CompileShader(fragmentShaderSource, ShaderType.FragmentShader);

			var shaderProgram = gl.CreateProgram();
			gl.AttachShader(shaderProgram, vertexShader);
			gl.AttachShader(shaderProgram, fragmentShader);
			gl.LinkProgram(shaderProgram);

			gl.GetProgram(shaderProgram, GLEnum.LinkStatus, out var status);
			if(status == 0)
			{
				var infoLog = gl.GetProgramInfoLog(shaderProgram);
				throw new Exception($"Program linking failed: {infoLog}");
			}

			gl.DeleteShader(vertexShader);
			gl.DeleteShader(fragmentShader);
			return shaderProgram;
		}

		private uint CompileShader(string source, ShaderType type)
		{
			var shader = gl.CreateShader(type);
			gl.ShaderSource(shader, source);
			gl.CompileShader(shader);

			gl.GetShader(shader, ShaderParameterName.CompileStatus, out var status);
			if(status == 0)
			{
				var infoLog = gl.GetShaderInfoLog(shader);
				throw new Exception($"{type} compilation failed: {infoLog}");
			}
			return shader;
		}

		public uint GetShaderProgram(string _name)
		{
			return shaderPrograms[_name];
		}

		public void AddShaderProgram(string _name, uint _shaderProgramId)
		{
			shaderPrograms[_name] = _shaderProgramId;
		}
	}
}