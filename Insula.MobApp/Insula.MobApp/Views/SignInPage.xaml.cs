using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insula.MobApp.Models;
using Insula.MobApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Insula.MobApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInPage : ContentPage
	{
        public SignInViewModel SignInViewModel { get; private set; }
		public SignInPage()
		{
			InitializeComponent();
            Init();
            SignInViewModel = new SignInViewModel(this) { Navigation = this.Navigation };
            this.BindingContext = SignInViewModel;
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Label_Username.TextColor = Constants.TextColor;
            Label_Password.TextColor = Constants.TextColor;
            LogoIcon.HeightRequest = Constants.LogoIconHeight;
            App.StartCheckIfInternet(Label_Internet, this);
            //-----Delegates
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Button_Clicked_SignIn(s, e);
        }

        void Button_Clicked_SignIn(object sender, EventArgs e)
        {
            SignInViewModel.SignIn();
        }

        void ToolbarItem_Clicked_SignUp(object sender, EventArgs e)
        {
            SignInViewModel.SignUp();
        }
    }
}