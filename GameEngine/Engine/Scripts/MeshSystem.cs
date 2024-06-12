
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
			mesh.NBO = gl.GenBuffer();
			mesh.UVBO = gl.GenBuffer();

			gl.BindVertexArray(mesh.VAO);

			// Vertex positions
			gl.BindBuffer(GLEnum.ArrayBuffer, mesh.VBO);
			fixed (float* v = &mesh.vertices[0])
			{
				gl.BufferData(GLEnum.ArrayBuffer, (uint)(mesh.vertices.Length * sizeof(float)), v, GLEnum.StaticDraw);
			}

			// Element indices
			gl.BindBuffer(GLEnum.ElementArrayBuffer, mesh.EBO);
			fixed (uint* i = &mesh.indices[0])
			{
				gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(mesh.indices.Length * sizeof(uint)), i, GLEnum.StaticDraw);
			}

			// Normals
			gl.BindBuffer(GLEnum.ArrayBuffer, mesh.NBO);
			fixed (float* n = &mesh.Normals[0])
			{
				gl.BufferData(GLEnum.ArrayBuffer, (nuint)(mesh.Normals.Length * sizeof(float)), n, GLEnum.StaticDraw);
			}

			// UVs
			gl.BindBuffer(GLEnum.ArrayBuffer, mesh.UVBO);
			fixed (float* uv = &mesh.UVs[0])
			{
				gl.BufferData(GLEnum.ArrayBuffer, (nuint)(mesh.UVs.Length * sizeof(float)), uv, GLEnum.StaticDraw);
			}

			// Position attribute
			gl.EnableVertexAttribArray(0);
			gl.BindBuffer(GLEnum.ArrayBuffer, mesh.VBO);
			gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 3 * sizeof(float), (void*)0);

			// Normal attribute
			gl.EnableVertexAttribArray(1);
			gl.BindBuffer(GLEnum.ArrayBuffer, mesh.NBO);
			gl.VertexAttribPointer(1, 3, GLEnum.Float, false, 3 * sizeof(float), (void*)0);

			// UV attribute
			gl.EnableVertexAttribArray(2);
			gl.BindBuffer(GLEnum.ArrayBuffer, mesh.UVBO);
			gl.VertexAttribPointer(2, 2, GLEnum.Float, false, 2 * sizeof(float), (void*)0);

			gl.BindVertexArray(0);

			Console.WriteLine($"VAO: {mesh.VAO}, VBO: {mesh.VBO}, EBO: {mesh.EBO}, NBO: {mesh.NBO}");
		}

		public void DetachMesh(MeshComponent mesh)
		{
			if(mesh.VAO != 0)
			{
				gl.DeleteVertexArray(mesh.VAO);
				mesh.VAO = 0;
			}

			if(mesh.VBO != 0)
			{
				gl.DeleteBuffer(mesh.VBO);
				mesh.VBO = 0;
			}

			if(mesh.EBO != 0)
			{
				gl.DeleteBuffer(mesh.EBO);
				mesh.EBO = 0;
			}

			if(mesh.NBO != 0)
			{
				gl.DeleteBuffer(mesh.NBO);
				mesh.NBO = 0;
			}
		}

		public override void Update(EntityManager entityManager)
		{
		}
	}
}