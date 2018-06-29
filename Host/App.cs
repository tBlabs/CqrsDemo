using Core;
using System;
using System.Reflection;

namespace Host
{
    public class App
    {
        public App(IMessageBus messageBus)
        {
            try
            {
                string messageAsJson = "";

                messageAsJson = "{ 'name': 'SampleQuery', 'args': \"{ 'Foo': 'Bar' }\" }";
                messageBus.Exe(messageAsJson);

                messageAsJson = "{ 'name': 'SampleCommand', 'args': \"{ 'Foo': 'Bar' }\" }";
                messageBus.Exe(messageAsJson);

                messageAsJson = "{ 'name': 'NotExistingMessage', 'args': \"{ 'Foo': 'Bar' }\" }";
                messageBus.Exe(messageAsJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}