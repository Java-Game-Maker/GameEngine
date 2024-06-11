
namespace GameEngine
{
	public class Resource
	{
		public uint Id { get; set; }
		public ResourceType resourceType { get; set; }
		public ResourceState resourceState { get; set; }

		public void Detach(){}
	}
}