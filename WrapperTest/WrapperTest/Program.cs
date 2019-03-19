using System;
using RestSharp;

namespace WrapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new User("UpdateTestSubject", "nieuw@gmail.com", "test");


            Requests getUsers = new Requests();

            //getUsers.AddUser(user);

            getUsers.UpdateUser(9, user);

            //getUsers.GetUsers();

            //getUsers.DeleteUser(6);


            Console.WriteLine("test");

        }
    }
}
