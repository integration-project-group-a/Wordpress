using System;
using System.Collections.Generic;
using System.Text;

namespace Sender
{
    class Visitors
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string gsm { get; set; }
        public int version { get; set; }
        public int banned { get; set; }
        public int isActive { get; set; }
        public DateTime birthDate { get; set; }
        public string btwNummer { get; set; }
        public int gdpr { get; set; }

        public Visitors(string firstname, string lastname, string email, int version)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;
            this.version = version;

        }
    }
}
