using System;
using System.Collections.Generic;

namespace GameEngine
{
	public struct Entity
	{
		public int Id;
	}

	public abstract class EntityComponent
	{
	}

	public abstract class EntitySystem
	{
		public abstract void Update(EntityManager entityManager);
	}
}
