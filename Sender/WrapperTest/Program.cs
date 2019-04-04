using System;
using RestSharp;
using RabbitMQ.Client;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Requests requests = new Requests();

            var factory = new ConnectionFactory() { HostName = "10.3.56.27" };
            factory.UserName = "manager";
            factory.Password = "ehb";
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                var message = requests.GetUser(1);

                

                XmlDocument doc = new XmlDocument();
                doc = (XmlDocument)JsonConvert.DeserializeXmlNode(message, "root");

                //doc.Validate(xsd)

                var body = Encoding.UTF8.GetBytes(doc.InnerXml);
                channel.BasicPublish(exchange: "logs",
                                         routingKey: "",
                                         basicProperties: null,
                                         body: body);
                Console.WriteLine(" [x] Sent {0}", doc);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();



        }
    }
}
