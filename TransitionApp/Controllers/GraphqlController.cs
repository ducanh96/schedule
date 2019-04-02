using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using TransitionApp.Application.RequestModel;
using TransitionApp.Domain.Notifications;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Models;
using TransitionApp.Models.Vehicle;

namespace TransitionApp.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphqlController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
       


        public GraphqlController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)

        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            //await MassTransitRabbitAsync();

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);
            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        private void PublishMessage()
        {
            // send queue
            string UserName = "guest";
            string Password = "guest";
            string HostName = "localhost";

            //Main entry point to the RabbitMQ .NET AMQP client
            string exchangeName = "HelloWorld_RabbitMQ";
            string queueName = "HelloQueue";

            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                UserName = UserName,
                Password = Password,
                HostName = HostName
            };
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {

                    channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
                    channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                    string message = "Hello World. This is my first RabbitMQ Message";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.QueueBind(queueName, exchangeName, string.Empty);

                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: string.Empty,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine("Message Sent Successfully - {0}", message);
                }
            }
        }

        private async Task MassTransitRabbitAsync()
        {
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(
                rabbit =>
                {
                    IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost/"), settings =>
                    {

                    });



                }
              );
            await rabbitBusControl.StartAsync();


            await rabbitBusControl.Publish<IVehicleCreate>(new
            {
                Name = "Duc Anh"
            });


            Console.ReadLine();

            await rabbitBusControl.StopAsync();
        }

    }
}