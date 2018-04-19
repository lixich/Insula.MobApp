using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Insula.MobApp.Data;
using Insula.MobApp.Droid.Data;

[assembly: Xamarin.Forms.Dependency(typeof(NetworkConnection))]

namespace Insula.MobApp.Droid.Data
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnected { get; set; }

        public void CheckNetWorkConnection()
        {
            var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }
    }
}