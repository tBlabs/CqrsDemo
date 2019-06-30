using System;
using System.Threading.Tasks;
using tBlabs.Cqrs.Core.Interfaces;

namespace FeatureX
{
    public class FeatureXSampleQuery : IQuery<int>
    {
        public int Value { get; set; }
    }

    public class FeatureXSampleQueryHandler : IQueryHandler<FeatureXSampleQuery, Task<int>>
    {
        public Task<int> Handle(FeatureXSampleQuery query)
        {
            return Task.FromResult(query.Value * 5);
        }
    }

    public class FeatureXSampleCommand : ICommand
    {
        public int Value { get; set; }
    }

    public class FeatureXSampleCommandHandler : ICommandHandler<FeatureXSampleCommand>
    {
        public Task Handle(FeatureXSampleCommand command)
        {
            Console.WriteLine("FeatureXSampleCommandHandler Value="+command.Value.ToString());

            return Task.CompletedTask;
        }
    }
}
