using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace Sender
{
    class User
    {
        public User(string username, string email, string password)
        {
            this.username = username;
            this.email = email;
            this.password = password;
        }

        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public string ToJsonString()
        {
            string json =
                "{" +
                "\"username\": " + "\"" + this.username + "\"," + 
                "\"email\": " + "\"" + this.email + "\"," +
                "\"password\": " + "\"" + this.password + "\"," +
                "}";

            return json;
        }
    }

    
}
