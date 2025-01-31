from Player import Player
from perlin_noise import PerlinNoise

class Minecraft:
	def __init__(self, _g):
		self.globals = _g
		self.player = Player(_g)

		self.cubePath		= self.globals["Utils"].FromAssets("./Models/cube.obj")
		self.vertexShader	= self.globals["Utils"].FromAssets("./Shaders/vertex.glsl")
		self.fragmentShader = self.globals["Utils"].FromAssets("./Shaders/fragment.glsl")

		self.define_shader()

		self.chunk_x = 32
		self.chunk_y = 12
		self.chunk_z = 32
		self.noise = PerlinNoise()

		self.gen_map()

	def define_shader(self):
		self.globals["Managers"].resourceManager.Import_Shader(
			"standardShader",
			self.vertexShader,
			self.fragmentShader
		)
		self.globals["Managers"].resourceManager.Import_Texture("minecraft_dirt", self.globals["Utils"].FromAssets("./minecraft_dirt.png"))

		location = self.globals["Managers"]._GL.GetUniformLocation(self.globals["Managers"].resourceManager.Get_Shader("standardShader").ShaderProgramId, "texture1")
		self.globals["Managers"]._GL.Uniform1(location, self.globals["Managers"].resourceManager.Get_Texture("minecraft_dirt").Id)
	
	def gen_map(self):
		for x in range(self.chunk_x):
			for z in range(self.chunk_z):
				go = self.globals["GameObject"]("./Models/cube.obj", f"{x}_{z}")
				go.SetPosition(x * 2, self.noise.noise((x * .5) / self.chunk_y, (z * .5) / self.chunk_y, 0) * 10, z * 2)
				self.globals["Managers"].meshSystem.BindMesh(self.globals["Managers"].resourceManager.models[f"{x}_{z}"])
	
	def Update(self):
		self.player.Update()
