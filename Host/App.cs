using System;
using System.Threading.Tasks;
using Core.Services;

namespace Host
{
    public class App
    {
	    private readonly IMessageBus _messageBus;

	    public App(IMessageBus messageBus)
	    {
		    _messageBus = messageBus;
	    }

		public async Task Run()
        {
            try
            {
                await _messageBus.ExecuteFromJson("{ 'name': 'SampleQuery', 'args': \"{ 'Foo': 'Bar' }\" }");

                await _messageBus.ExecuteFromJson("{ 'name': 'SampleCommand', 'args': \"{ 'Foo': 'Bar' }\" }");

                await _messageBus.ExecuteFromJson("{ 'name': 'NotExistingMessage', 'args': \"{ 'Foo': 'Bar' }\" }");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}