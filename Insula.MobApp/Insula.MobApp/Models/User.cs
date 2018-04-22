using System;
using System.Collections.Generic;
using System.Text;

namespace Insula.MobApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Birthday { get; set; }
        public string Weight { get; set; }
        public string Growth { get; set; }
        public string Insulin { get; set; }
        public double NormalGlucose { get; set; }

        public User() { }
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public bool CheckInformation()
        {
            if (this.Username != null && this.Password != null && !this.Username.Equals("") && !this.Password.Equals(""))
                return true;
            else
                return false;
        }
    }
}
