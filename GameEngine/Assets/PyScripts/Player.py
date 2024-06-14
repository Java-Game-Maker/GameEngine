
""" from Main import expose_globals

# Expose globals in the current scope
expose_globals(globals()) """

class Player:

	def __init__(self, _g):
		self.globals = _g
		self.pos = [0, 0, 0]

		self.currentSpeed = 2
		self.walkingSpeed = 2
		self.runningSpeed = 4

		self.jumpForce = 4
		self.Gravity = 2
		self.create_entity()
	
	def expose_globals(self):
		globals().scope['Time'] = self.globals.Time
		globals().scope['Managers'] = self.globals.Managers
		globals().scope['TransformComponent'] = self.globals.TransformComponent
		globals().scope['CameraComponent'] = self.globals.CameraComponent
		globals().scope['GameObject'] = self.globals.GameObject
		globals().scope['Utils'] = self.globals.Utils

	def create_entity(self):
		self.entity = self.globals["Managers"].entityManager.CreateEntity()
		camComp = self.globals["CameraComponent"]()
		transformComp = self.globals["TransformComponent"]()

		camComp.Speed = self.currentSpeed
		camComp.walkSpeed = self.walkingSpeed
		camComp.runSpeed = self.runningSpeed

		camComp.InitInput(transformComp)
		self.globals["Managers"].inputHandler.inputStates.Add(camComp.editor_state)

		self.globals["Managers"].entityManager.AddComponent(self.entity, camComp)
		self.globals["Managers"].entityManager.AddComponent(self.entity, transformComp)
	
	def Update(self):
		self.pos[1] -= 1 if(self.pos[1] > 2) else 0
