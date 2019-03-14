using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using System.Threading.Tasks;
using RestSharp.Authenticators;

namespace WrapperTest
{
    class Requests
    {


        public void GetUsers() {
            var client = new RestClient("http://10.3.56.25/wp-json/wp/v2/");
            client.Authenticator = new HttpBasicAuthenticator("Front-end", "Front-end");

            var request = new RestRequest("users", Method.GET);

            request.RequestFormat = DataFormat.Json;

            var resposne = client.Execute(request);

            var content = resposne.Content;

            Console.WriteLine(content);

        }

        public void AddUser(User user)
        {
            var client = new RestClient("http://10.3.56.25/wp-json/wp/v2/");
            client.Authenticator = new HttpBasicAuthenticator("Front-end", "Front-end");
            var request = new RestRequest("users", Method.POST);

            request.AddJsonBody(user);

            var response = client.Post(request);  


        }

        public void UpdateUser(int id, User user)
        {
            var client = new RestClient("http://10.3.56.25/wp-json/wp/v2/");
            client.Authenticator = new HttpBasicAuthenticator("Front-end", "Front-end");
            var request = new RestRequest("users/3", Method.POST);
                //request.AddUrlSegment("id", id);        

            request.AddJsonBody(user);

            IRestResponse response = client.Execute(request);
            
            //var response = client.Post(request);


        }

    }
}
