using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore5RabbitMQ.Common.Models.ShoppingModels
{
    public class TakeOrderRequest
    {
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
    }
}
