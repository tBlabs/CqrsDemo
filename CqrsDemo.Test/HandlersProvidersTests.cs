using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using tBlabs.Cqrs.Core.Interfaces;
using tBlabs.Cqrs.Core.Services;
using Xunit;

namespace tBlabs.Cqrs.Core.Tests
{
    public class HandlersProvidersTests
    {
	    public class HPTestCommand : ICommand
	    { }

	    public class HPTestCommandHandler : ICommandHandler<HPTestCommand>
	    {
		    public Task Handle(HPTestCommand command)
		    {
			    throw new NotImplementedException();
		    }
	    }

		public class HPTestQuery : IQuery<int>
	    { }

	    public class HPTestQueryHandler : IQueryHandler<HPTestQuery, int>
	    {
		    public int Handle(HPTestQuery query)
		    {
			    throw new NotImplementedException();
		    }
	    }

	    public readonly ITypesProvider _typesProvider;

	    public HandlersProvidersTests()
	    {
			var typesProviderMock = new Mock<ITypesProvider>();
			typesProviderMock.Setup(x => x.Types).Returns(new Type[]
				{typeof(HPTestCommand), typeof(HPTestQuery), typeof(HPTestCommandHandler), typeof(HPTestQueryHandler)});
			_typesProvider = typesProviderMock.Object;
	    }

		[Fact]
        public void HandlersProvider_should_collect_all_handlers()
        {        
            var sut = new HandlersProvider(_typesProvider);

            sut.Handlers.Count.Should().Be(2);
            sut.Handlers.Should().Contain(typeof(HPTestCommandHandler));
            sut.Handlers.Should().Contain(typeof(HPTestQueryHandler));
        }

        [Fact]
        public void HandlerTypeProvider_should_provide_handler_by_message_type()
        {
            var sut = new HandlerTypeProvider(_typesProvider);

            sut.GetByMessageType(typeof(HPTestCommand)).Should().Be<HPTestCommandHandler>();
            sut.GetByMessageType(typeof(HPTestQuery)).Should().Be<HPTestQueryHandler>();
        }
    }
}
