using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace WrapperTest
{
    class User
    {
        public User(string username, string email, string password)
        {
            this.username = username;
            this.email = email;
            this.password = password;
        }

        public User(string email)
        {
            this.email = email;
        }

        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
