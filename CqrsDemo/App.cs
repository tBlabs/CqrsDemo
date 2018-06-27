using System;
using System.Reflection;

namespace CqrsDemo
{
    public class App
    {
        public App(IMessageBus messageBus)
        {
            //try
            //{
            //    string messageAsJson = "";

            //    messageAsJson = "{ 'name': 'SampleQuery', 'args': \"{ 'Foo': 'Bar' }\" }";
            //    messageBus.Exe(messageAsJson);
                                
            //    messageAsJson = "{ 'name': 'SampleCommand', 'args': \"{ 'Foo': 'Bar' }\" }";
            //    messageBus.Exe(messageAsJson);

            //    messageAsJson = "{ 'name': 'NotExistingMessage', 'args': \"{ 'Foo': 'Bar' }\" }";
            //    messageBus.Exe(messageAsJson);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            foreach (var t in this.GetType().Assembly.GetTypes())
            {
                Console.WriteLine(t);
            }

            Console.ReadKey();
        }
    }
}