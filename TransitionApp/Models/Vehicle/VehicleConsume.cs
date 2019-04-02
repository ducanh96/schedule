using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel;

namespace TransitionApp.Models.Vehicle
{
    public class VehicleConsume : IConsumer<VehicleReadModel>
    {
        public async Task Consume(ConsumeContext<VehicleReadModel> context)
        {
            await Console.Out.WriteLineAsync($"Id: {context.Message.Id}");
            await Console.Out.WriteLineAsync($"PlateNumber: {context.Message.LicensePlate}");

        }
    }
}
