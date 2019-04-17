using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MassTransit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Models.Vehicle;

namespace TransitionApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            //{
            //    var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
            //    {
            //        h.Username("guest");
            //        h.Password("guest");
            //    });

            //    sbc.ReceiveEndpoint(host, "abc_queue", ep =>
            //    {
            //        ep.Handler<VehicleReadModel>(context =>
            //        {
            //            return Console.Out.WriteLineAsync($"Received: {context.Message.LicensePlate}");
            //        });
            //        ep.Consumer<VehicleConsume>();
            //    });
            //});

            //bus.Start(); // This is important!

            ////Console.WriteLine("Press any key to exit");
            ////Console.ReadKey();

            //bus.Stop();

            CreateWebHostBuilder(args).Build().Run();
     
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
