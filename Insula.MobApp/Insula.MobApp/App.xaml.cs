using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Insula.MobApp.Data;
using Insula.MobApp.Models;
using Insula.MobApp.Views;
using Xamarin.Forms;

namespace Insula.MobApp
{
	public partial class App : Application
	{
        static RestService restService;
        static User user;
        private static Label labelScreen;
        private static bool hasInternet;
        private static Page currentPage;
        private static Timer timer;
        private static bool noInterShow;

        public App ()
		{
			InitializeComponent();

			MainPage = new LoginPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        public static RestService RestService
        {
            get
            {
                if (restService == null)
                    restService = new RestService();
                return restService;
            }
        }

        public static User User
        {
            get
            {
                if (user == null)
                    user = new User();
                return user;
            }
            set
            {
                user = value;
            }
        }


        //-----Internet Connection-----
        public static void StartCheckIfInternet(Label label, Page page)
        {
            labelScreen = label;
            label.Text = Constants.NoInternetText;
            label.IsVisible = false;
            hasInternet = true;
            currentPage = page;
            if (timer == null)
            {
                timer = new Timer((e) =>
                {
                    StartCheckIfInternet();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        private static void StartCheckIfInternet()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetWorkConnection();
            if(!networkConnection.IsConnected)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (hasInternet)
                    {
                        if (!noInterShow)
                        {
                            hasInternet = false;
                            labelScreen.IsVisible = true;
                            await ShowDisplayAlert();
                        }
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hasInternet = true;
                    labelScreen.IsVisible = false;
                });
            }
        }

        public static bool CheckIfInternet()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetWorkConnection();
            return networkConnection.IsConnected;
        }

        public static async Task<bool> CheckIfInternetAlert()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetWorkConnection();
            if (!networkConnection.IsConnected)
            {
                if (!noInterShow)
                {
                    await ShowDisplayAlert();
                }
                return false;
            }
            return true;
        }

        private static async Task ShowDisplayAlert()
        {
            noInterShow = false;
            await currentPage.DisplayAlert("Internet", "Device has no internet, please reccomect", "OK");
            noInterShow = false;
        }
    }
}
