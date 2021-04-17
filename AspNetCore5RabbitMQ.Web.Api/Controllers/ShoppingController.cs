using AspNetCore5RabbitMQ.Common.Models.ShoppingModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore5RabbitMQ.Web.Api.Controllers
{
    [Route("api/shopping")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        [HttpPost("TakeOrder")]
        public IActionResult TakeOrder(TakeOrderRequest request)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                };

                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.QueueDeclare(
                    queue: "TakeOrderQueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );
                
                for (int i = 1; i <= 100000; i++)
                {
                    request.CustomerId = i;

                    var data = JsonConvert.SerializeObject(request);
                    var byteData = Encoding.UTF8.GetBytes(data);


                    channel.BasicPublish(exchange: "",
                                         routingKey: "TakeOrderQueue",
                                         basicProperties: null,
                                         body: byteData);
                }

                return Ok("Success");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
