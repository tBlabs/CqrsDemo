using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Core.Test
{
    public class SampleQuery : IQuery<string>
    {
        public string Foo { get; set; }
    }

    public class MessageProviderTest
    {
        private readonly IMessageProvider messageProvider;

        public MessageProviderTest()
        {
            var thisProjectTypes = new ThisAssemblyTypesProvider();
            var messageTypeProvider = new MessageTypeProvider(thisProjectTypes);
            messageProvider = new MessageProvider(messageTypeProvider);
        }

        [Fact]
        public void MessageProvider_should_not_resolve_from_empty_input()
        {
            string messageAsJson = null;
            Action act = () => messageProvider.Resolve(messageAsJson);

            Assert.Throws<Exception>(act);

            messageAsJson = "";
            act = () => messageProvider.Resolve(messageAsJson);

            Assert.Throws<Exception>(act);
        }

        [Fact]
        public void MessageProvider_should_not_resolve_from_senseless_input()
        {
            string messageAsJson = "senseless_input";
            Action act = () => messageProvider.Resolve(messageAsJson);

            Assert.Throws<Exception>(act);
        }

        [Fact]
        public void MessageProvider_should_not_resolve_from_message_with_empty_args()
        {
            var messageAsJson = "{ 'name': 'SampleQuery', 'args': '' }";
            IMessage message = messageProvider.Resolve(messageAsJson);

            message.Should().BeNull();
        }

        [Fact]
        public void MessageProvider_should_not_resolve_from_message_without_args()
        {
            var messageAsJson = "{ 'name': 'SampleQuery' }";
            
            Action act = () => messageProvider.Resolve(messageAsJson);

            Assert.Throws<Exception>(act);
        }

        [Fact]
        public void MessageProvider_should_resolve_from_valid_message()
        {
            var messageAsJson = "{ 'name': 'SampleQuery', 'args': \"{ 'Foo': 'Bar' }\" }";
            IMessage message = messageProvider.Resolve(messageAsJson);

            message.Should().BeOfType<SampleQuery>();
            message.As<SampleQuery>().Foo.Should().Be("Bar");
        }
    }
}

