
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace GameEngine
{
	public class RenderingSystem : EntitySystem
	{
		private GL gl;
		private ShaderManager shaderManager;
		private CameraSystem cameraSystem;
		private MeshSystem meshSystem;

		public RenderingSystem(GL gl, ShaderManager _shaderManager, CameraSystem _cameraSystem, MeshSystem _meshSystem)
		{
			this.gl = gl;
			this.shaderManager = _shaderManager;
			this.cameraSystem = _cameraSystem;
			this.meshSystem = _meshSystem;
		}

		public override void Update(EntityManager entityManager)
		{
			var viewMatrix = cameraSystem.GetViewMatrix();
			var projectionMatrix = cameraSystem.GetProjectionMatrix();

			var renderableEntities = entityManager.GetAllEntitiesWithComponent<MeshComponent>();

			foreach(var entity in renderableEntities)
			{
				var transform = entityManager.GetComponent<TransformComponent>(entity);
				var mesh = entityManager.GetComponent<MeshComponent>(entity);
				var shaderComponent = entityManager.GetComponent<ShaderComponent>(entity);

				RenderEntity(transform, mesh, shaderComponent, viewMatrix, projectionMatrix);
			}
		}

		private unsafe void RenderEntity(TransformComponent transform, MeshComponent mesh, ShaderComponent shaderComponent, Matrix4X4<float> viewMatrix, Matrix4X4<float> projectionMatrix)
		{
			var shaderProgram = shaderComponent.ShaderProgramId;

			gl.UseProgram(shaderProgram);

			gl.UniformMatrix4(gl.GetUniformLocation(shaderProgram, "view"), 1, false, viewMatrix.ToFloatArray());
			gl.UniformMatrix4(gl.GetUniformLocation(shaderProgram, "projection"), 1, false, projectionMatrix.ToFloatArray());

			Matrix4X4<float> modelMatrix = CalculateModelMatrix(transform);
			var modelLocation = gl.GetUniformLocation(shaderProgram, "model");
			gl.UniformMatrix4(modelLocation, 1, false, modelMatrix.ToFloatArray());

			meshSystem.BindMesh(mesh);

			gl.DrawElements(GLEnum.Triangles, (uint)mesh.IndexCount, GLEnum.UnsignedInt, null);
			gl.UseProgram(0);
		}

		private Matrix4X4<float> CalculateModelMatrix(TransformComponent transform)
		{
			// Combine position, rotation, and scale into a model matrix
			var translationMatrix = Matrix4X4.CreateTranslation(new Vector3D<float>(transform.Position.X, transform.Position.Y, transform.Position.Z));
			var rotationMatrix = Matrix4X4.CreateRotationX(transform.Rotation.X) *
									Matrix4X4.CreateRotationY(transform.Rotation.Y) *
									Matrix4X4.CreateRotationZ(transform.Rotation.Z);
			var scaleMatrix = Matrix4X4.CreateScale(new Vector3D<float>(transform.Scale.X, transform.Scale.Y, transform.Scale.Z));

			return scaleMatrix * rotationMatrix * translationMatrix;
		}
	}
}