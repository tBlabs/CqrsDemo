using System;

namespace CqrsDemo
{
    public class App
    {
        public App(IMessageBus messageBus)
        {       
            string messageAsJson = "{ 'name': 'SampleQuery', 'args': \"{ 'Foo': 'Bar' }\" }";
            messageBus.Exe(messageAsJson);

            messageAsJson = "{ 'name': 'SampleCommand', 'args': \"{ 'Foo': 'Bar' }\" }";
            messageBus.Exe(messageAsJson);
          
            Console.ReadKey();
        }
    }
}