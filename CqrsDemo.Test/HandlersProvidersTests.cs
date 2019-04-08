using System;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Core.Test
{
    public class HandlersProvidersTests
    {
	    public class Command : ICommand
	    { }

	    public class Query : IQuery<int>
	    { }

	    public class CommandHandler : ICommandHandler<Command>
	    {
		    public Task Handle(Command command)
		    {
			    throw new NotImplementedException();
		    }
	    }

	    public class QueryHandler : IQueryHandler<Query, int>
	    {
		    public int Handle(Query query)
		    {
			    throw new NotImplementedException();
		    }
	    }

	    private readonly ITypesProvider _typesProvider;

	    public HandlersProvidersTests()
	    {
			var typesProviderMock = new Mock<ITypesProvider>();
			typesProviderMock.Setup(x => x.Types).Returns(new Type[]
				{typeof(Command), typeof(Query), typeof(CommandHandler), typeof(QueryHandler)});
			_typesProvider = typesProviderMock.Object;
	    }

		[Fact]
        public void HandlersProvider_should_collect_all_handlers()
        {        
            var sut = new HandlersProvider(_typesProvider);

            sut.Handlers.Count.Should().Be(2);
            sut.Handlers.Should().Contain(typeof(CommandHandler));
            sut.Handlers.Should().Contain(typeof(QueryHandler));
        }

        [Fact]
        public void HandlerTypeProvider_should_provide_handler_by_message_type()
        {
            var sut = new HandlerTypeProvider(_typesProvider);

            sut.GetByMessageType(typeof(Command)).Should().Be<CommandHandler>();
            sut.GetByMessageType(typeof(Query)).Should().Be<QueryHandler>();
        }
    }
}
