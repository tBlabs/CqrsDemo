using System;
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

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Type[] thisProjectTypes = this.GetType().Assembly.GetTypes();
            var handlersProvider = new HandlersProvider(thisProjectTypes);

            int count = handlersProvider.Services.Count;

            count.Should().Be(2);
            handlersProvider.Services.Should().Contain(typeof(CommandHandler));
            handlersProvider.Services.Should().Contain(typeof(QueryHandler));
        }
    }
}
