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
	    public class TestCommand : ICommand
	    { }

	    public class TestQuery : IQuery<int>
	    { }

	    public class TestCommandHandler : ICommandHandler<TestCommand>
	    {
		    public Task Handle(TestCommand command)
		    {
			    throw new NotImplementedException();
		    }
	    }

	    public class TestQueryHandler : IQueryHandler<TestQuery, int>
	    {
		    public int Handle(TestQuery query)
		    {
			    throw new NotImplementedException();
		    }
	    }

	    public readonly ITypesProvider _typesProvider;

	    public HandlersProvidersTests()
	    {
			var typesProviderMock = new Mock<ITypesProvider>();
			typesProviderMock.Setup(x => x.Types).Returns(new Type[]
				{typeof(TestCommand), typeof(TestQuery), typeof(TestCommandHandler), typeof(TestQueryHandler)});
			_typesProvider = typesProviderMock.Object;
	    }

		[Fact]
        public void HandlersProvider_should_collect_all_handlers()
        {        
            var sut = new HandlersProvider(_typesProvider);

            sut.Handlers.Count.Should().Be(2);
            sut.Handlers.Should().Contain(typeof(TestCommandHandler));
            sut.Handlers.Should().Contain(typeof(TestQueryHandler));
        }

        [Fact]
        public void HandlerTypeProvider_should_provide_handler_by_message_type()
        {
            var sut = new HandlerTypeProvider(_typesProvider);

            sut.GetByMessageType(typeof(TestCommand)).Should().Be<TestCommandHandler>();
            sut.GetByMessageType(typeof(TestQuery)).Should().Be<TestQueryHandler>();
        }
    }
}
