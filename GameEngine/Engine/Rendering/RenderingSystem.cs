
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

			int viewLocation = gl.GetUniformLocation(shaderProgram, "view");
			int projectionLocation = gl.GetUniformLocation(shaderProgram, "projection");
			int modelLocation = gl.GetUniformLocation(shaderProgram, "model");

			int lightPosLocation = gl.GetUniformLocation(shaderProgram, "lightPos");
			int viewPosLocation = gl.GetUniformLocation(shaderProgram, "viewPos");
			int lightColorLocation = gl.GetUniformLocation(shaderProgram, "lightColor");
			int objectColorLocation = gl.GetUniformLocation(shaderProgram, "objectColor");

			if (viewLocation == -1 || projectionLocation == -1 || modelLocation == -1 ||
                lightPosLocation == -1 || viewPosLocation == -1 || lightColorLocation == -1 || objectColorLocation == -1)
            {
                Console.WriteLine("Error: Unable to get uniform location");
                CheckForErrors();
                return;
            }

			gl.UniformMatrix4(viewLocation, 1, false, viewMatrix.ToFloatArray());
			gl.UniformMatrix4(projectionLocation, 1, false, projectionMatrix.ToFloatArray());

			Matrix4X4<float> modelMatrix = CalculateModelMatrix(transform);
			gl.UniformMatrix4(modelLocation, 1, false, modelMatrix.ToFloatArray());

			var lightPos = new Vector3D<float>(1.2f, 1.0f, 2.0f);
            var viewPos = new Vector3D<float>(0.0f, 0.0f, 3.0f);
            var lightColor = new Vector3D<float>(1.0f, 1.0f, 1.0f);
            var objectColor = new Vector3D<float>(0.8f, 0.5f, 0.31f);

            gl.Uniform3(lightPosLocation, lightPos.X, lightPos.Y, lightPos.Z);
            gl.Uniform3(viewPosLocation, viewPos.X, viewPos.Y, viewPos.Z);
            gl.Uniform3(lightColorLocation, lightColor.X, lightColor.Y, lightColor.Z);
            gl.Uniform3(objectColorLocation, objectColor.X, objectColor.Y, objectColor.Z);

			gl.BindVertexArray(mesh.VAO);

			var error = gl.GetError();
			if (error != GLEnum.NoError)
			{
				Console.WriteLine($"OpenGL Error before DrawElements: {error}");
			}

			gl.DrawElements(GLEnum.Triangles, (uint)mesh.IndexCount, GLEnum.UnsignedInt, null);

			error = gl.GetError();
			if (error != GLEnum.NoError)
			{
				Console.WriteLine($"OpenGL Error after DrawElements: {error}");
			}

			gl.BindVertexArray(0);
			gl.UseProgram(0);
		}

		private void CheckForErrors()
		{
			var error = gl.GetError();
			if (error != GLEnum.NoError)
			{
				Console.WriteLine($"OpenGL Error: {error}");
			}
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