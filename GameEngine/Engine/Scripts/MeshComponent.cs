
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace GameEngine
{
	public class MeshComponent : EntityComponent
	{
		public float[] vertices { get; set; }
		public uint[] indices { get; set; }
		public float[] Normals { get; set; }
		public float[] UVs { get; set; }

		public uint VAO { get; set; }
		public uint VBO { get; set; }
		public uint EBO { get; set; }
		public uint NBO { get; set; }
		public uint UVBO { get; set; }
		public uint IndexCount { get; set; }

		public MeshComponent(float[] _vertices, uint[] _indices)
		{
			vertices = _vertices;
			indices = _indices;
			IndexCount = (uint)_indices.Length;
		}

		public float[] CalculateNormals(float[] vertices, uint[] indices)
		{
			var normals = new float[vertices.Length];
			for (int i = 0; i < indices.Length; i += 3)
			{
				int index0 = (int)indices[i] * 3;
				int index1 = (int)indices[i + 1] * 3;
				int index2 = (int)indices[i + 2] * 3;

				Vector3D<float> v0 = new Vector3D<float>(vertices[index0], vertices[index0 + 1], vertices[index0 + 2]);
				Vector3D<float> v1 = new Vector3D<float>(vertices[index1], vertices[index1 + 1], vertices[index1 + 2]);
				Vector3D<float> v2 = new Vector3D<float>(vertices[index2], vertices[index2 + 1], vertices[index2 + 2]);

				Vector3D<float> edge1 = v1 - v0;
				Vector3D<float> edge2 = v2 - v0;
				Vector3D<float> normal = Vector3D.Cross(edge1, edge2);
				normal = Vector3D.Normalize(normal);

				normals[index0] += normal.X;
				normals[index0 + 1] += normal.Y;
				normals[index0 + 2] += normal.Z;
				normals[index1] += normal.X;
				normals[index1 + 1] += normal.Y;
				normals[index1 + 2] += normal.Z;
				normals[index2] += normal.X;
				normals[index2 + 1] += normal.Y;
				normals[index2 + 2] += normal.Z;
			}

			for (int i = 0; i < normals.Length; i += 3)
			{
				Vector3D<float> normal = new Vector3D<float>(normals[i], normals[i + 1], normals[i + 2]);
				normal = Vector3D.Normalize(normal);

				normals[i] = normal.X;
				normals[i + 1] = normal.Y;
				normals[i + 2] = normal.Z;
			}

			return normals;
		}
	}
}