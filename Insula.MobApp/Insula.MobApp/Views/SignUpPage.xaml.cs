using Insula.MobApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Insula.MobApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
	{
		public SignUpPage ()
		{
			InitializeComponent ();
            Init();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Label_Username.TextColor = Constants.TextColor;
            Label_Password.TextColor = Constants.TextColor;

            //-----Delegates
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Button_Clicked_SignUp(s, e);
        }

        async void Button_Clicked_SignUp(object sender, EventArgs e)
        {
            var user = new User()
            {
                Username = Entry_Username.Text,
                Password = Entry_Password.Text,
            };

            user = await App.RestService.PostResponse<User>(Constants.UserUrl, JsonConvert.SerializeObject(user));
            if (user != null)
            {
                await DisplayAlert("Sign up", "Sign up success", "OK");
                Navigation.InsertPageBefore(new DiaryListPage(), Navigation.NavigationStack.First());
                await Navigation.PopToRootAsync();
            }
            else
            {
                Label_Message.Text = "Sign up failed";
            }
        }
    }
}