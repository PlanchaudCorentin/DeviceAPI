using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace CommandDevice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
       
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{commandType}")]
        public ActionResult<string> Get(string commandType)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "192.168.43.88",
                VirtualHost = "/",
                Password = "devproject",
                UserName = "admin",
                Port = 5672
            };
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            JObject o = new JObject();
            o.Add("commandType", commandType);
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o));
            channel.BasicPublish("", "Command", null, messageBodyBytes);
            channel.Close();
            conn.Close();
            
            return "value";
        }
    }
}