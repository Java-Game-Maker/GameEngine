
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace GameEngine
{
	public class RenderingSystem : EntitySystem
	{
		private GL gl;
		public float world_time = 16;

		public RenderingSystem(GL gl)
		{
			this.gl = gl;
		}

		public override void Update(EntityManager entityManager)
		{
			var viewMatrix = Managers.cameraSystem.GetViewMatrix();
			var projectionMatrix = Managers.cameraSystem.GetProjectionMatrix();

			var renderableEntities = entityManager.GetAllEntitiesWithComponent<MeshComponent>();

			foreach(var entity in renderableEntities)
			{
				var transform = entityManager.GetComponent<TransformComponent>(entity);
				var mesh = entityManager.GetComponent<MeshComponent>(entity);
				var shaderComponent = entityManager.GetComponent<ShaderComponent>(entity);

				RenderEntity(transform, mesh, shaderComponent, viewMatrix, projectionMatrix);
			}
		}

		public Vector3D<float> CalculateLightDirection(float timeOfDay)
		{
			float normalizedTime = timeOfDay / 24.0f;
			float azimuthAngle = normalizedTime * MathF.PI * 2.0f;
			float elevationAngle = MathF.PI / 4.0f * MathF.Sin(normalizedTime * MathF.PI);

			float x = MathF.Cos(elevationAngle) * MathF.Cos(azimuthAngle);
			float y = MathF.Sin(elevationAngle);
			float z = MathF.Cos(elevationAngle) * MathF.Sin(azimuthAngle);

			return new Vector3D<float>(x, y, z);
		}

		private unsafe void RenderEntity(TransformComponent transform, MeshComponent mesh, ShaderComponent shaderComponent, Matrix4X4<float> viewMatrix, Matrix4X4<float> projectionMatrix)
		{
			var shaderProgram = shaderComponent.ShaderProgramId;

			gl.UseProgram(shaderProgram);

			int viewLocation = gl.GetUniformLocation(shaderProgram, "view");
			int projectionLocation = gl.GetUniformLocation(shaderProgram, "projection");
			int modelLocation = gl.GetUniformLocation(shaderProgram, "model");

			int lightDirLocation = gl.GetUniformLocation(shaderProgram, "lightDir");
			int viewPosLocation = gl.GetUniformLocation(shaderProgram, "viewPos");
			int lightColorLocation = gl.GetUniformLocation(shaderProgram, "lightColor");
			int objectColorLocation = gl.GetUniformLocation(shaderProgram, "objectColor");

			if (viewLocation == -1 || projectionLocation == -1 || modelLocation == -1 ||
				lightDirLocation == -1 || viewPosLocation == -1 || lightColorLocation == -1 || objectColorLocation == -1)
			{
				Console.WriteLine("Error: Unable to get uniform location");
				CheckForErrors();
				return;
			}

			gl.UniformMatrix4(viewLocation, 1, false, viewMatrix.ToFloatArray());
			gl.UniformMatrix4(projectionLocation, 1, false, projectionMatrix.ToFloatArray());

			Matrix4X4<float> modelMatrix = CalculateModelMatrix(transform);
			gl.UniformMatrix4(modelLocation, 1, false, modelMatrix.ToFloatArray());

			var lightDir = CalculateLightDirection(world_time);
			var viewPos = new Vector3D<float>(0.0f, 0.0f, 3.0f);
			var lightColor = new Vector3D<float>(1.0f, 1.0f, 1.0f);
			var objectColor = new Vector3D<float>(1.0f, 0.5f, 0.31f);

			gl.Uniform3(lightDirLocation, lightDir.X, lightDir.Y, lightDir.Z);
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