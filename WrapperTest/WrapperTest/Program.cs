using System;
using RestSharp;

namespace WrapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new User("nieuw@gmail.com");


            Requests getUsers = new Requests();

            //getUsers.AddUser(user);

            getUsers.UpdateUser(3, user);

            //getUsers.GetUsers();



            Console.WriteLine("test");

        }
    }
}
