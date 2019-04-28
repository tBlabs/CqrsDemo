using System.IO;

namespace tBlabs.Cqrs.Core.Interfaces
{
	public interface IMessageWithStream : IMessage
	{
		Stream Stream { get; set; }
	}
}