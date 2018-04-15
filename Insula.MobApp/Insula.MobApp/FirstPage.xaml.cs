using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Insula.MobApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FirstPage : ContentPage
	{
		public FirstPage ()
		{
			InitializeComponent ();
		}

        private void Button_Click_Authorization(object sender, EventArgs e)
        {
            if (Entry_AuthorizationLogin.Text == Entry_AuthorizationPassword.Text)
                Label_Authorization.Text = "Successful Authorization";
            else
                Label_Authorization.Text = "Login or password failed";
        }
    }
}