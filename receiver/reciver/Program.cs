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

                string id = "", uuid = "", mail= "";
                dynamic dynObj = JsonConvert.DeserializeObject(jsonText);
                foreach (var item in dynObj.data)
                {
                   uuid= item.UUID;
                   mail = item.email;
                }

                HttpWebRequest httpWebRequest;
                HttpWebResponse httpWebResponse;
                switch ((string)jObject["header"]["description"])
                {
                    case "Creation of a visitor":

                        httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://127.0.0.1/wordpress/wp-json/integration/visitordb"); //url
                        httpWebRequest.ContentType = "application/json"; //ContentType
                        httpWebRequest.Method = "GET"; //Methode

                        httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //sending request
                        using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                        {
                            String result = streamReader.ReadToEnd(); //get result Json string From respons
                            dynamic data = JObject.Parse(result);
                            if (data.email == mail)
                            {
                                id = data.id;
                            }
                        }

                        httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://127.0.0.1/wordpress/wp-json/integration/visitordb/" + id); //url
                        httpWebRequest.ContentType = "application/json"; //ContentType
                        httpWebRequest.Method = "PUT"; //Methode

                        //body
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(jsonText);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //sending request
                        using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                        {
                            String result = streamReader.ReadToEnd(); //get result Json string From respons
                            Console.WriteLine("result");
                            Console.WriteLine(result);
                        }
                        break;
                    case "Update of a visitor":
                        httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://127.0.0.1/wordpress/wp-json/integration/visitordb"); //url
                        httpWebRequest.ContentType = "application/json"; //ContentType
                        httpWebRequest.Method = "GET"; //Methode

                        httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //sending request
                        using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                        {
                            String result = streamReader.ReadToEnd(); //get result Json string From respons
                            dynamic data = JObject.Parse(result);
                            if (data.email == mail)
                            {
                                id = data.id;
                            }
                        }

                        httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://127.0.0.1/wordpress/wp-json/integration/visitordb/" + id); //url
                        httpWebRequest.ContentType = "application/json"; //ContentType
                        httpWebRequest.Method = "PUT"; //Methode

                        //body
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(jsonText);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //sending request
                        using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                        {
                            String result = streamReader.ReadToEnd(); //get result Json string From respons
                            Console.WriteLine("result");
                            Console.WriteLine(result);
                        }
                        Console.WriteLine("visitor update");
                        break;
                    case "Deletion of a visitor":

                        httpWebRequest = (HttpWebRequest)WebRequest.Create("http://127.0.0.1/wordpress/wp-json/integration/visitordb/" + id); //url
                        httpWebRequest.ContentType = "application/json"; //ContentType
                        httpWebRequest.Method = "DELETE"; //Methode

                        httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //sending request
                        using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                        {
                            String result = streamReader.ReadToEnd(); //get result Json string From respons
                            Console.WriteLine("result");
                            Console.WriteLine(result);
                        }
                        Console.WriteLine("visitor delete");
                        break;
                }
            };
            channel.BasicConsume(queue: queueName,autoAck: true,consumer: consumer);
            Console.ReadLine();
        }
       
    }
}