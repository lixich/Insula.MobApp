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
	public partial class SignInPage : ContentPage
	{
		public SignInPage()
		{
			InitializeComponent ();
            Init();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Label_Username.TextColor = Constants.TextColor;
            Label_Password.TextColor = Constants.TextColor;
            ActivitySpinner.IsVisible = false;
            LogoIcon.HeightRequest = Constants.LogoIconHeight;
            App.StartCheckIfInternet(Label_Internet, this);

            //-----Delegates
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Button_Clicked_SignIn(s, e);
        }

        async void Button_Clicked_SignIn(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            if (user.CheckInformation())
            {
                user = await App.RestService.Authentication(user);
                if (user != null)
                {
                    //await DisplayAlert("Login", "Login Success.", "OK");
                    Navigation.InsertPageBefore(new DiaryListPage(), this);
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("Login", "Login or Password Failed", "OK");

            }
            else await DisplayAlert("Login", "Login Not Correct, empty username or password.", "OK");
        }

        async void ToolbarItem_Clicked_SignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}