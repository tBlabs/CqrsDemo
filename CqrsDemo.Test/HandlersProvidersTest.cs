using System;
using System.Text;
using FluentAssertions;
using Xunit;

namespace CqrsDemo.Test
{
    public class Command : ICommand
    {

    }

    public class Query : IQuery<int>
    {

    }

    public class CommandHandler : ICommandHandler<Command>
    {
        public void Handle(Command command)
        {
            throw new NotImplementedException();
        }
    }

    public class QueryHandler : IQueryHandler<Query, int>
    {
        public int Handle(Query command)
        {
            throw new NotImplementedException();
        }
    }

    public class HandlersProvidersTest
    {
        [Fact]
        public void HandlersProvider_should_collect_handlers()
        {
            var thisProjectTypes = new ThisAssemblyTypesProvider();
            var handlersProvider = new HandlerProvider(thisProjectTypes);

            int count = handlersProvider.Services.Count;

            count.Should().Be(2);
            handlersProvider.Services.Should().Contain(typeof(CommandHandler));
            handlersProvider.Services.Should().Contain(typeof(QueryHandler));
        }

        [Fact]
        public void HandlerTypeProvider_should_collect_handlers_with_their_messages()
        {
            var thisProjectTypes = new ThisAssemblyTypesProvider();
            var handlersProvider = new HandlerTypeProvider(thisProjectTypes);

            handlersProvider.GetByMessageType(typeof(Command))
                .Should().Be<CommandHandler>();

            handlersProvider.GetByMessageType(typeof(Query))
                .Should().Be<QueryHandler>();
        }
    }
}
