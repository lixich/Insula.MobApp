using Insula.MobApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Insula.MobApp.ViewModel;

namespace Insula.MobApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
	{
        public SignUpViewModel SignUpViewModel { get; private set; }
        public SignUpPage ()
		{
			InitializeComponent ();
            Init();
            SignUpViewModel = new SignUpViewModel(this) { Navigation = this.Navigation };
            this.BindingContext = SignUpViewModel;
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Label_Username.TextColor = Constants.TextColor;
            Label_Password.TextColor = Constants.TextColor;

            //-----Delegates
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Entry_Birthday.Focus();
            Entry_Birthday.Completed += (s, e) => Entry_Weight.Focus();
            Entry_Weight.Completed += (s, e) => Entry_Growth.Focus();
            Entry_Growth.Completed += (s, e) => Entry_Insulin.Focus();
            Entry_Insulin.Completed += (s, e) => Entry_NormalGlucose.Focus();
            Entry_NormalGlucose.Completed += (s, e) => Button_Clicked_Save(s, e);
        }

        void Button_Clicked_Save(object sender, EventArgs e)
        {
            SignUpViewModel.Save();
        }
    }
}