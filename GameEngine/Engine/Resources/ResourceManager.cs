using System.Runtime.InteropServices;
using System.Collections.Generic;
using Silk.NET.OpenGL;
using FreeImageAPI;
using System;

namespace GameEngine
{
	public enum ResourceType
	{
		Texture,
		Sound,
		Shader,
		Obj
	}

	public enum ResourceState
	{
		Detached,
		Loading,
		Loaded
	}

	public class ResourceManager
	{
		private readonly GL gl;

		public readonly Dictionary<string, MeshComponent> models = new Dictionary<string, MeshComponent>();
		public readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
		private readonly Dictionary<string, ShaderComponent> shaders = new Dictionary<string, ShaderComponent>();

		public ResourceManager(GL _gl)
		{
			gl = _gl;
		}

		/* Import Texture */

		private unsafe Texture LoadTexture(string path)
		{
			var image = RM_Texture.LoadImage(path);
			if(image == null)
			{
				throw new Exception("Failed to load texture: " + path);
			}

			uint textureId = gl.GenTexture();
			gl.BindTexture(TextureTarget.Texture2D, textureId);

			fixed(byte* pData = image.Data)
			{
				gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba, (uint)image.Width, (uint)image.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, pData);
			}

			gl.GenerateMipmap(TextureTarget.Texture2D);

			var texture = new Texture(textureId, image.Width, image.Height);
			return texture;
		}

		public void Import_Texture(string name, string path)
		{
			if(!textures.ContainsKey(name))
			{
				textures.Add(name, LoadTexture(path));
			}else{ Console.WriteLine($"Failed to import texture. Name: {name}, is already booked by another texture."); }
		}

		public void Detach_Texture(string name)
		{
			if(textures.ContainsKey(name))
			{
				gl.DeleteTexture(textures[name].Id);
				textures.Remove(name);
			}
		}

		public Texture Get_Texture(string name) => textures[name];

		/* Import Mesh */

		public void Import_Model(string name, string path)
		{
			if(!models.ContainsKey(name))
			{
				models.Add(name, RM_Obj.LoadOBJ(path));
			}else{Console.WriteLine($"Failed to import object. Name: {name}, is already booked by another object.");}
		}

		public void Detach_Model(string name)
		{
			MeshComponent mesh = models[name];
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
			models.Remove(name);
		}

		public MeshComponent Get_Model(string name) => models[name];

	}
}