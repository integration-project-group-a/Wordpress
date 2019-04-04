using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

class ReceiveLogs
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "10.3.56.27" };
        factory.UserName = "manager";
        factory.Password = "ehb";
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "logs", type: "fanout");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: "logs",
                              routingKey: "");

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(message);
                
                string jsonText = JsonConvert.SerializeXmlNode(doc,Newtonsoft.Json.Formatting.None,true);

                JObject jObject = JObject.Parse(jsonText);

                HttpWebRequest httpWebRequest;
                HttpWebResponse httpWebResponse;
                string elasticId;
                switch ((string)jObject["header"]["description"])
                {
                    case "Creation of a visitor":
                        Console.WriteLine("visitor creat");
                        break;
                    case "Update of a visitor":
                        Console.WriteLine("visitor update");
                        break;
                    case "Deletion of a visitor":
                        Console.WriteLine("visitor delete");
                        break;
                }
            };
            channel.BasicConsume(queue: queueName,autoAck: true,consumer: consumer);
            Console.ReadLine();
        }
       
    }
}