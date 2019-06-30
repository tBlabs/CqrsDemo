using System;
using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Services;

namespace ConsoleAppHost
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
                var res = await _messageBus.Execute("{ 'FeatureXSampleQuery': { 'Value': 5 } }");
                Console.WriteLine(res);

                await _messageBus.Execute("{ 'FeatureXSampleCommand': { 'Value': 7 } }");

                await _messageBus.Execute("{ 'NotExistingMessage': { 'Foo': 'Bar' } }");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}