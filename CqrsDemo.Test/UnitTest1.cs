using System;
using Xunit;

namespace CqrsDemo.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var handlersProvider = new HandlersProvider();

            int count = handlersProvider.Services.Count;

            Assert.Equal(0, count);
        }
    }
}
