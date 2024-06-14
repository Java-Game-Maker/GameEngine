
from minecraft import Minecraft as MC

class Main:
	def __init__(self):
		print("Starting python main script")
		self.mc = MC(globals())
	
	def Start(self):
		pass
	
	def Update(self):
		self.mc.Update()

m = Main()
Start = m.Start
Update = m.Update