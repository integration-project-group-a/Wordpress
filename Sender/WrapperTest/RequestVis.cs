using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using System.Threading.Tasks;
using RestSharp.Authenticators;

namespace Sender
{
    class RequestVis
    {
        public void GetVisitors()
        {
            var client = new RestClient("http://localhost/wordpress/wp-json/integration/");
            client.Authenticator = new HttpBasicAuthenticator("admin", "admin");

            var request = new RestRequest("visitordb", Method.GET);

            request.RequestFormat = DataFormat.Json;

            var resposne = client.Execute(request);

            var content = resposne.Content;

            Console.WriteLine(content);

        }

        public String GetVisitor(int visitorsID)
        {
            var client = new RestClient("http://localhost/wordpress/wp-json/integration/");
            client.Authenticator = new HttpBasicAuthenticator("admin", "admin");

            var request = new RestRequest("visitordb/", Method.GET);
            //request.AddUrlSegment("id", visitorsID);

            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            var content = response.Content;

            int start = content.IndexOf("{\"id\":\""+visitorsID+"\"");
            string temp = content.Substring(start);
            int index = temp.IndexOf("}");

            var uitkomst = temp.Substring(0, index+1);
            //Console.WriteLine(content);

            return uitkomst;
        }

        public void AddVisitor(Visitors visitor)
        {
            var client = new RestClient("http://localhost/wordpress/wp-json/integration/");

            client.Authenticator = new HttpBasicAuthenticator("admin", "admin");
            var request = new RestRequest("visitordb", Method.POST);

            request.AddJsonBody(visitor);

            var response = client.Post(request);


        }

        public void UpdateVisitor(int id, Visitors visitor)
        {
            Visitors update = visitor;

            var client = new RestClient("http://10.3.56.25/wp-json/integration/");
            client.Authenticator = new HttpBasicAuthenticator("Front-end", "Front-end");
            var request = new RestRequest("visitordb/{id}", Method.POST);
            request.AddUrlSegment("id", id);

            request.AddJsonBody(visitor);

            IRestResponse response = client.Execute(request);


        }

        public void DeleteVisitor(int id)
        {
            var client = new RestClient("http://10.3.56.25/wp-json/integration/");
            client.Authenticator = new HttpBasicAuthenticator("Front-end", "Front-end");
            var request = new RestRequest("visitordb/{id}?force=true", Method.DELETE);
            request.AddUrlSegment("id", id);
            string json = "{" + "\"reassign\": \"1\"" + "}";

            request.AddJsonBody(json);

            IRestResponse response = client.Execute(request);
        }
    }
}
