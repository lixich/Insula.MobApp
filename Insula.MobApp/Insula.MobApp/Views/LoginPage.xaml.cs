using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insula.MobApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Insula.MobApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            Init();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Label_Username.TextColor = Constants.MainTextColor;
            Label_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LogoIcon.HeightRequest = Constants.LogoIconHeight;
            App.StartCheckIfInternet(Label_Internet, this);

            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Button_Clicked_SignIn(s, e);
        }

        async void Button_Clicked_SignIn(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            if (user.CheckInformation())
            {
                var result = await App.RestService.Login(user);
                if (result != null)
                {
                    App.User = result;
                    await DisplayAlert("Login", "Login Success.", "OK");
                }
                else await DisplayAlert("Login", "Login or Password Failed", "OK");

            }
            else await DisplayAlert("Login", "Login Not Correct, empty username or password.", "OK");
        }
    }
}