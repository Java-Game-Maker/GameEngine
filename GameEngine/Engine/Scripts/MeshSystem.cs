
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
			gl.BindVertexArray(mesh.VAO);
			gl.BindBuffer(GLEnum.ArrayBuffer, mesh.VBO);
			gl.BindBuffer(GLEnum.ElementArrayBuffer, mesh.EBO);

			gl.EnableVertexAttribArray(0); // pos
			gl.VertexAttribPointer(0, 3, GLEnum.Float, false, sizeof(float) * 8, (void*)0);
			gl.EnableVertexAttribArray(1); // norm
			gl.VertexAttribPointer(1, 3, GLEnum.Float, false, sizeof(float) * 8, (void*)(sizeof(float) * 3));
			gl.EnableVertexAttribArray(2); // TexCoord
			gl.VertexAttribPointer(2, 2, GLEnum.Float, false, sizeof(float) * 8, (void*)(sizeof(float) * 6));
		}

		public override void Update(EntityManager entityManager)
		{
			throw new NotImplementedException();
		}
	}
}