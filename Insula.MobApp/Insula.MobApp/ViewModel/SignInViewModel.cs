using Insula.MobApp.Models;
using Insula.MobApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Insula.MobApp.ViewModel
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public Page Page { get; set; }
        public User User { get; private set; }

        public SignInViewModel(Page page)
        {
            Page = page;
            User = new User();
        }

        public string Username
        {
            get { return User.Username; }
            set
            {
                if (User.Username != value)
                { User.Username = value; OnPropertyChanged("Username"); }
            }
        }
        public string Password
        {
            get { return User.Password; }
            set
            {
                if (User.Password != value)
                { User.Password = value; OnPropertyChanged("Password"); }
            }
        }

        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Username)) || (!string.IsNullOrEmpty(Password)));
            }
        }

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                if (_IsBusy != value)
                { _IsBusy = value; OnPropertyChanged("IsBusy"); }
            }
        }

        async public void SignIn()
        {
            if (!IsValid)
            {
                await Page.DisplayAlert("Login", "Login not correct, empty username or password.", "OK");
            }
            else
            {
                IsBusy = true;
                var user = new User(Username, Password);
                user = await App.RestService.Authentication(user);
                if (user != null)
                {
                    App.User = user;
                    IsBusy = false;
                    Navigation.InsertPageBefore(new DiaryListPage(), Page);
                    await Navigation.PopAsync();
                }
                else
                {
                    IsBusy = false;
                    await Page.DisplayAlert("Login", "Login or Password Failed.", "OK");
                }
            }
        }

        public void SignUp()
        {
            Navigation.PushAsync(new SignUpPage());
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
