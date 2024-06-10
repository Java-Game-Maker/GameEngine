
using Silk.NET.OpenGL;

namespace GameEngine
{
	public class MeshSystem : EntitySystem
	{
		private GL gl;

		public MeshSystem(GL _gl)
		{
			this.gl = _gl;
		}

		public unsafe void BindMesh(MeshComponent mesh)
		{
			mesh.VAO = gl.GenVertexArray();
			mesh.VBO = gl.GenBuffer();
			mesh.EBO = gl.GenBuffer();

			gl.BindVertexArray(mesh.VAO);

			gl.BindBuffer(GLEnum.ArrayBuffer, mesh.VBO);
			fixed (float* v = &mesh.vertices[0])
			{
				gl.BufferData(GLEnum.ArrayBuffer, (uint)(mesh.vertices.Length * sizeof(float)), v, GLEnum.StaticDraw);
			}

			gl.BindBuffer(GLEnum.ElementArrayBuffer, mesh.EBO);
			fixed (uint* i = &mesh.indices[0])
			{
				gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(mesh.indices.Length * sizeof(uint)), i, GLEnum.StaticDraw);
			}

			gl.EnableVertexAttribArray(0); // position
			gl.VertexAttribPointer(0, 3, GLEnum.Float, false, sizeof(float) * 6, (void*)0);
			gl.EnableVertexAttribArray(1); // color
			gl.VertexAttribPointer(1, 3, GLEnum.Float, false, sizeof(float) * 6, (void*)(sizeof(float) * 3));

			gl.BindVertexArray(0);

			Console.WriteLine($"VAO: {mesh.VAO}, VBO: {mesh.VBO}, EBO: {mesh.EBO}");
		}

		public override void Update(EntityManager entityManager)
		{
		}
	}
}