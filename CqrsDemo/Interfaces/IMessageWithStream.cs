using System.IO;

namespace Core.Interfaces
{
	public interface IMessageWithStream : IMessage
	{
		Stream Stream { get; set; }
	}
}