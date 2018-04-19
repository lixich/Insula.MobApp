using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Insula.MobApp.Models
{
    public class Constants
    {
        public static bool IsDev = true;

        public static Color BackgroundColor = Color.White;
        public static Color MainTextColor = Color.Black;

        public static int LogoIconHeight = 100;



        //-----Url-----
        //public static string Url = "http://127.0.0.1:5000/";
        public static string Url = "http://lixich.pythonanywhere.com/";
        public static string LoginUrl = Url + "user/";



        //-----Text------
        public static string NoInternetText = "No Internet, please reconnect.";
    }
}
