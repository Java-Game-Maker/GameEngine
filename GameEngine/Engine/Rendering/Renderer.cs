using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.IO;
using Silk.NET.Input;

namespace GameEngine
{
	public class Renderer
	{
		private GL gl;
		private uint shaderProgram;
		private uint vertexArray;
		private Shader shader;
		private Camera camera;

		public Renderer(Camera camera)
		{
			this.camera = camera;
		}

		public void Initialize(GL glContext)
		{
			gl = glContext;
			gl.ClearColor(.5f, .8f, 1f, 1f);

			shader = new Shader(gl, "Shaders/vertex.glsl", "Shaders/fragment.glsl");

			float[] vertices = {
				// positions		 // colors
				 0.5f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f,
				 0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,
				-0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,
				-0.5f,  0.5f, 0.0f,  1.0f, 1.0f, 0.0f
			};

			uint[] indices = {
				0, 1, 3,
				1, 2, 3
			};

			vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(vertexArray);

			uint vertexBuffer = gl.GenBuffer();
			gl.BindBuffer(GLEnum.ArrayBuffer, vertexBuffer);
			unsafe
			{
				fixed (float* v = &vertices[0])
				{
					gl.BufferData(GLEnum.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), v, GLEnum.StaticDraw);
				}
			}

			uint elementBuffer = gl.GenBuffer();
			gl.BindBuffer(GLEnum.ElementArrayBuffer, elementBuffer);
			unsafe
			{
				fixed (uint* i = &indices[0])
				{
					gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), i, GLEnum.StaticDraw);
				}
			}

			unsafe
			{
				gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 6 * sizeof(float), (void*)0);
				gl.EnableVertexAttribArray(0);

				// Color attribute
				gl.VertexAttribPointer(1, 3, GLEnum.Float, false, 6 * sizeof(float), (void*)(3 * sizeof(float)));
				gl.EnableVertexAttribArray(1);

				gl.BindBuffer(GLEnum.ArrayBuffer, 0);
				gl.BindVertexArray(0);
			}
		}

		public void Render(float aspectRatio)
		{
			gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
			shader.Use();

			Matrix4X4<float> view = camera.GetViewMatrix();
			Matrix4X4<float> projection = camera.GetProjectionMatrix(aspectRatio);

			shader.SetUniform("view", view);
			shader.SetUniform("projection", projection);

			shader.SetUniform("timeElapsed", (float)Time.TimeElapsed);

			gl.BindVertexArray(vertexArray);
			unsafe
			{
				gl.DrawElements(GLEnum.Triangles, 6, GLEnum.UnsignedInt, (void*)0);
			}
		}
	}
}