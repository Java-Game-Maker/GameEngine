using System;
using System.IO;
using System.Collections.Generic;
using Assimp;
using Assimp.Configs;

namespace GameEngine
{
	public class RM_Obj
	{
		public static MeshComponent LoadOBJ(string path)
		{
			var context = new AssimpContext();
			var scene = context.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.JoinIdenticalVertices);

			if(scene == null || scene.Meshes.Count == 0)
			{
				throw new Exception("Failed to load the OBJ file.");
			}

			var mesh = scene.Meshes[0];
			var vertices = new List<float>();
			var indices = new List<uint>();

			for (int i = 0; i < mesh.Vertices.Count; i++)
			{
				var vertex = mesh.Vertices[i];
				vertices.Add(vertex.X);
				vertices.Add(vertex.Y);
				vertices.Add(vertex.Z);

				vertices.Add(1.0f);
				vertices.Add(1.0f);
				vertices.Add(1.0f);
			}

			foreach (var face in mesh.Faces)
			{
				if (face.IndexCount == 3)
				{
					indices.Add((uint)face.Indices[0]);
					indices.Add((uint)face.Indices[1]);
					indices.Add((uint)face.Indices[2]);
				}
				else
				{
					Console.WriteLine("Non-triangular face detected, skipping.");
				}
			}

			if (vertices.Count / 3 <= indices.Max())
			{
				throw new Exception("The indices reference a vertex that does not exist.");
			}

			return new MeshComponent(vertices.ToArray(), indices.ToArray());
		}
	}
}
