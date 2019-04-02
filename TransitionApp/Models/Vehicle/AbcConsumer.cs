using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransitionApp.Models.Vehicle
{
    public class AbcConsumer : IConsumer<Abc>
    {
        public async Task Consume(ConsumeContext<Abc> context)
        {
            await Console.Out.WriteLineAsync($"Id: {context.Message.Name}");

        }
    }
}
