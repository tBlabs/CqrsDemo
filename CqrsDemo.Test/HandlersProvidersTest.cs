using System;
using System.Text;
using Core.Cqrs;
using Core.Services;
using FluentAssertions;
using Xunit;

namespace Core.Test
{
    public class Command1 : ICommand
    {

    }

    public class Query1 : IQuery<int>
    {

    }

    public class CommandHandler : ICommandHandler<Command1>
    {
        public void Handle(Command1 command)
        {
            throw new NotImplementedException();
        }
    }

    public class QueryHandler : IQueryHandler<Query1, int>
    {
        public int Handle(Query1 query)
        {
            throw new NotImplementedException();
        }
    }

    public class HandlersProvidersTest
    {
        [Fact]
        public void HandlersProvider_should_collect_handlers()
        {
            var solutionTypesProvider = new SolutionTypesProvider();
            var handlersProvider = new HandlerProvider(solutionTypesProvider);

            int count = handlersProvider.Handlers.Count;

            count.Should().Be(2);
            handlersProvider.Handlers.Should().Contain(typeof(CommandHandler));
            handlersProvider.Handlers.Should().Contain(typeof(QueryHandler));
        }

        [Fact]
        public void HandlerTypeProvider_should_collect_handlers_with_their_messages()
        {
            var solutionTypesProvider = new SolutionTypesProvider();
            var handlersProvider = new HandlerTypeProvider(solutionTypesProvider);

            handlersProvider.GetByMessageType(typeof(Command1))
                .Should().Be<CommandHandler>();

            handlersProvider.GetByMessageType(typeof(Query1))
                .Should().Be<QueryHandler>();
        }
    }
}
