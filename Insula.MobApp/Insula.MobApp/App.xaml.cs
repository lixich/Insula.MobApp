using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
