
namespace GameEngine
{
	public class Resource
	{
		public uint Id { get; set; }
		public ResourceType resourceType { get; set; }

		public virtual void Detach(ResourceManager resourceManager)
		{
		}
	}
}