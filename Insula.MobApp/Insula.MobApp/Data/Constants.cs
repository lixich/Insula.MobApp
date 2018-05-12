using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Insula.MobApp.Data
{
    public class Constants
    {
        public static bool IsDev = true;

        public static Color BackgroundColor = Color.White;
        public static Color TextColor = Color.Gray;
        public static Color MainTextColor = Color.Black;

        public static int LogoIconHeight = 150;

        //-----Url-----
        public static string Url = "http://46.146.110.136:5000/";
        //public static string Url = "https://6eb221c8.ngrok.io/";
        public static string UserUrl = Url + "user/";
        public static string DiaryUrl = Url + "dose/";
        public static string ForecastUrl = Url + "forecast/";

        //-----Text------
        public static string NoInternetText = "No Internet, please reconnect.";
    }
}
