
namespace GameEngine
{
	public class EntityManager
	{
		private int _nextEntityId = 0;
		private Dictionary<int, List<EntityComponent>> _entityComponents = new Dictionary<int, List<EntityComponent>>();

		public Entity CreateEntity()
		{
			var entity = new Entity { Id = _nextEntityId++ };
			_entityComponents[entity.Id] = new List<EntityComponent>();
			return entity;
		}

		public void AddComponent<T>(Entity entity, T component) where T : EntityComponent
		{
			_entityComponents[entity.Id].Add(component);
		}

		public T GetComponent<T>(Entity entity) where T : EntityComponent
		{
			foreach(var component in _entityComponents[entity.Id])
			{
				if(component is T)
					return component as T;
			}
			return null;
		}

		public List<T> GetAllComponentsOfType<T>() where T : EntityComponent
		{
			var components = new List<T>();
			foreach(var entityComponents in _entityComponents.Values)
			{
				foreach(var component in entityComponents)
				{
					if(component is T)
						components.Add(component as T);
				}
			}
			return components;
		}

		public List<Entity> GetAllEntitiesWithComponent<T>() where T : EntityComponent
		{
			var entities = new List<Entity>();
			foreach(var kvp in _entityComponents)
			{
				foreach(var component in kvp.Value)
				{
					if(component is T)
					{
						entities.Add(new Entity { Id = kvp.Key});
						break;
					}
				}
			}
			return entities;
		}
	}
}