
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

	def create_entity(self):
		self.entity = self.globals["Managers"].entityManager.CreateEntity()
		camComp = self.globals["CameraComponent"]()
		self.transformComp = self.globals["TransformComponent"]()

		camComp.Speed = self.currentSpeed
		camComp.walkSpeed = self.walkingSpeed
		camComp.runSpeed = self.runningSpeed

		camComp.InitInput(self.transformComp)
		self.globals["Managers"].inputHandler.inputStates.Add(camComp.editor_state)

		self.globals["Managers"].entityManager.AddComponent(self.entity, camComp)
		self.globals["Managers"].entityManager.AddComponent(self.entity, self.transformComp)
	
	def Update(self):
		pass
