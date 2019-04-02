using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;
using TransitionApp.Application.Implement;
using TransitionApp.Application.Interface;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.CommandHanders;
using TransitionApp.Domain.Commands;
using TransitionApp.Domain.Commands.Vehicle;
using TransitionApp.Domain.Commands.VehicleType;
using TransitionApp.Domain.Events.Vehicle;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Notifications;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Infrastructor.Bus;
using TransitionApp.Infrastructor.Implement.Repository;
using TransitionApp.Models;
using TransitionApp.Models.Vehicle;
using TransitionApp.Models.Vehicle.InputType;
using TransitionApp.Models.Vehicle.ObjectType;
using TransitionApp.Service.Implement;
using TransitionApp.Service.Interface;

namespace TransitionApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, Infrastructor.Bus.InMemoryBus>();
            // DI vehicle
            services.AddTransient<IVehicleAppService, VehicleAppService>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddScoped<IRequestHandler<CreateVehicleCommand, object>, VehicleCommandHandler>();
            services.AddScoped<IRequestHandler<EditVehicleCommand, object>, VehicleCommandHandler>();

            // DI VehicleType
            services.AddTransient<IVehicleTypeAppService, VehicleTypeAppService>();
            services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
            services.AddTransient<IVehicleTypeService, VehicleTypeService>();
            services.AddScoped<IRequestHandler<CreateVehicleTypeCommand, object>, VehicleTypeCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteVehicleTypeCommand, object>, VehicleTypeCommandHandler>();

            // DI Driver
            services.AddTransient<IDriverAppService, DriverAppService>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddTransient<IDriverService, DriverService>();

            #region Add event

            //services.AddScoped<INotificationHandler<VehicleCreateEvent>, VehicleCreateEvent>();
            services.AddSingleton<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            #endregion



            ////add masstran
            //var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        var host = cfg.Host(new Uri("rabbitmq://localhost/"), settings =>
            //        {
            //            settings.Username("guest");
            //            settings.Password("guest");
                        
            //        });
            //        cfg.ReceiveEndpoint(host, "AbcHandler", e =>
            //        {
                        
            //            //e.Consumer<VehicleConsume>();
            //            e.Handler<VehicleReadModel>(context =>
            //            {
            //                return Console.Out.WriteLineAsync($"Received: {context.Message.LicensePlate}");
            //            });
            //            //e.Consumer<VehicleConsume>();
            //             e.Consumer<AbcConsumer>();

            //        });
            //    });


            //services.AddSingleton<IPublishEndpoint>(bus);
            //services.AddSingleton<ISendEndpointProvider>(bus);
            //services.AddSingleton<IBus>(bus);

            //bus.Start();

            //services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());


            // add graphQl
            services.AddSingleton<TransitionAppQuery>();
            services.AddSingleton<TransitionAppMutation>();
            services.AddSingleton<Models.VehicleType>();
            services.AddSingleton<DriverType>();
            services.AddSingleton<BaseResponseType>();
            services.AddSingleton<InputObjectGraphType>();
            services.AddSingleton<VehicleInputType>();
            services.AddSingleton<ListVehicleType>();
            services.AddSingleton<TransitionApp.Models.Vehicle.ObjectType.VehicleType>();
            services.AddSingleton<CreateVehicleResponseType>();
            services.AddSingleton<EditVehicleInputType>();

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            // add swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "ASP.NET Core Web API"
                });
            });


            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new TransitionAppSchema(new FuncDependencyResolver(type => sp.GetService(type))));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseGraphiQl();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });


        }
    }
}
