using Insula.MobApp.Models;
using Insula.MobApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Insula.MobApp.ViewModel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public Page Page { get; set; }
        public User User { get; private set; }

        public SignUpViewModel(Page page)
        {
            Page = page; 
            User = App.User;
        }

        public int Id
        {
            get { return User.Id; }
            private set { User.Id = value; }
        }
        public string uri
        {
            get { return User.uri; }
            private set { User.uri = value; }
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
        public string Birthday
        {
            get { return User.Birthday; }
            set
            {
                if (User.Birthday != value)
                { User.Birthday = value; OnPropertyChanged("Birthday"); }
            }
        }
        public double Weight
        {
            get { return User.Weight; }
            set
            {
                if (User.Weight != value)
                { User.Weight = value; OnPropertyChanged("Weight"); }
            }
        }
        public double Growth
        {
            get { return User.Growth; }
            set
            {
                if (User.Growth != value)
                { User.Growth = value; OnPropertyChanged("Growth"); }
            }
        }
        public string Insulin
        {
            get { return User.Insulin; }
            set
            {
                if (User.Insulin != value)
                { User.Insulin = value; OnPropertyChanged("Insulin"); }
            }
        }
        public double NormalGlucose
        {
            get { return User.NormalGlucose; }
            set
            {
                if (User.Weight != value)
                { User.Weight = value; OnPropertyChanged("NormalGlucose"); }
            }
        }
        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Username)) ||
                    (!string.IsNullOrEmpty(Password)) ||
                    (!string.IsNullOrEmpty(Insulin)) ||
                    (!string.IsNullOrEmpty(Password)) ||
                    (NormalGlucose <= 0));
            }
        }

        private string _Label_Message = "";

        public string Label_Message
        {
            get { return _Label_Message; }
            set
            {
                if (_Label_Message != value)
                { _Label_Message = value; OnPropertyChanged("Label_Message"); }
            }
        }

        public async void Save()
        {
            Label_Message = "";
            if (!App.IsAuthorized())
            {
                var user = await App.RestService.PostResponse<User>(Constants.UserUrl, JsonConvert.SerializeObject(User));
                if (user != null)
                {
                    App.User = user;
                    await Page.DisplayAlert("Settings", "Sign up success", "OK");
                    Navigation.InsertPageBefore(new DiaryListPage(), Navigation.NavigationStack[0]);
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    Label_Message = "Sign up failed";
                }
            }
            else
            {
                if (IsValid)
                {
                    var user = await App.RestService.PutResponse<User>(uri, JsonConvert.SerializeObject(User));
                    if (user != null)
                    {
                        await Page.DisplayAlert("Settings", "Save success", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Label_Message = "Save failed";
                    }
                }
                else
                {
                    await Page.DisplayAlert("Fields", "Fields not correct or empty.", "OK");
                }
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
