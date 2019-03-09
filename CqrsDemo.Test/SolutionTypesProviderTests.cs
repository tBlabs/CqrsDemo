using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Shouldly;
using Xunit;
using Shuttle.Core.Reflection;

namespace Core.Test
{
    public class SolutionTypesProviderTests
    {
	    [Fact]
	    public void Test()
	    {
		    //var sut = new SolutionTypesProvider();

		    //var x = sut.Types;
		    //	sut.Types.ShouldContain(typeof(SampleQuery));
		    var x = new ReflectionService().GetAssemblies();
	    }

    }
}
