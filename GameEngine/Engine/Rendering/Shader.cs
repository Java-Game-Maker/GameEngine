using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;
using System.IO;

namespace GameEngine
{
	public class Shader
	{
		private readonly GL _gl;
		public uint Handle {get; private set;}

		public Shader(GL gl, string vertexPath, string fragmentPath)
		{
			_gl = gl;
			string vertexSource = File.ReadAllText(vertexPath);
			string fragmentSource = File.ReadAllText(fragmentPath);

			uint vertexShader = CompileShader(ShaderType.VertexShader, vertexSource);
			uint fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentSource);

			Handle = _gl.CreateProgram();
			_gl.AttachShader(Handle, vertexShader);
			_gl.AttachShader(Handle, fragmentShader);
			_gl.LinkProgram(Handle);

			_gl.GetProgram(Handle, GLEnum.LinkStatus, out var success);
			if(success == 0)
			{
				var infoLog = _gl.GetProgramInfoLog(Handle);
				Console.WriteLine($"ERROR::SHADER::PROGRAM::LINKING_FAILED\n{infoLog}");
			}

			_gl.DeleteShader(vertexShader);
			_gl.DeleteShader(fragmentShader);
		}

		private uint CompileShader(ShaderType type, string source)
		{
			uint shader = _gl.CreateShader(type);
			_gl.ShaderSource(shader, source);
			_gl.CompileShader(shader);

			_gl.GetShader(shader, GLEnum.CompileStatus, out var success);
			if(success == 0)
			{
				var infoLog = _gl.GetShaderInfoLog(shader);
				Console.WriteLine($"ERROR::SHADER::COMPILATION_FAILED\n{infoLog}");
			}
			return shader;
		}

		public void Use()
		{
			_gl.UseProgram(Handle);
		}

		public void SetUniform(string name, float value)
		{
			int location = _gl.GetUniformLocation(Handle, name);
			_gl.Uniform1(location, value);
		}

		public void SetUniform(string name, int value)
		{
			int location = _gl.GetUniformLocation(Handle, name);
			_gl.Uniform1(location, value);
		}

		public unsafe void SetUniform(string name, Matrix4X4<float> value)
		{
			int location = _gl.GetUniformLocation(Handle, name);
			_gl.UniformMatrix4(location, 1, false, (float*)&value);
		}
	}
}