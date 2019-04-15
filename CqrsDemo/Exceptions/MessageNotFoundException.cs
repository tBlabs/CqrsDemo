namespace Core.Exceptions
{
	public class MessageNotFoundException : NotFoundException
	{
		public MessageNotFoundException(string messageName) : base($"Message '{messageName}' not found")
		{ }
	}
}