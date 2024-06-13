
function CreateEntity()
	return Managers.EntityManager:CreateEntity()
end

function AddComponent(entityId, componentType, params)
	return Managers.EntityManager:AddComponent(entityId, componentType, params)
end
