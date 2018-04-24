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
        public double Weight { get; set; }
        public double Growth { get; set; }
        public string Insulin { get; set; }
        public double NormalGlucose { get; set; }
        public string uri { get; set; }

        public User() { }
        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public User(string Username, string Password, string Birthday, double Weight, double Growth, string Insulin, double NormalGlucose)
        {
            this.Username = Username;
            this.Password = Password;
            this.Birthday = Birthday;
            this.Weight = Weight;
            this.Growth = Growth;
            this.Insulin = Insulin;
            this.NormalGlucose = NormalGlucose;
        }

        public bool CheckSignIn()
        {
            if (this.Username != null && this.Password != null && !this.Username.Equals("") && !this.Password.Equals(""))
                return true;
            else
                return false;
        }
        public bool CheckSignUp()
        {
            if (this.Username != null 
                && this.Password != null
                && this.Password != null
                && this.Birthday != null
                && this.Insulin != null
                && !this.Username.Equals("") 
                && !this.Password.Equals("")
                && !this.Birthday.Equals("")
                && !this.Insulin.Equals("")
                )
                return true;
            else
                return false;
        }
    }
}
