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
            User = (User) App.User.Clone();
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
        public DateTime Birthday
        {
            get { return User.Birthday; }
            set
            {
                if (User.Birthday != value)
                { User.Birthday = value; OnPropertyChanged("Birthday"); }
            }
        }
        public string Weight
        {
            get { return User.Weight.ToString(); }
            set
            {
                if (User.Weight.ToString() != value)
                { User.Weight = StringConvertToDouble(User.Weight, value, "Weight"); }
            }
        }


        public string Growth
        {
            get { return User.Growth.ToString(); }
            set
            {
                if (User.Growth.ToString() != value)
                { User.Growth = StringConvertToDouble(User.Growth, value, "Growth"); }
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
        public string NormalGlucose
        {
            get { return User.NormalGlucose.ToString(); }
            set
            {
                if (User.NormalGlucose.ToString() != value)
                { User.NormalGlucose = StringConvertToDouble(User.NormalGlucose, value, "NormalGlucose"); }
            }
        }
        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Username)) &&
                    (!string.IsNullOrEmpty(Password)) &&
                    (!string.IsNullOrEmpty(Insulin)) &&
                    (!string.IsNullOrEmpty(Password)) &&
                    (Birthday != null) &&
                    (User.NormalGlucose >= 0));
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
            if (!IsValid)
            {
                await Page.DisplayAlert("Fields", "Fields not correct or empty.", "OK");
            }
            else
            {
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
                    var user = await App.RestService.PutResponse<User>(uri, JsonConvert.SerializeObject(User));
                    if (user != null)
                    {
                        App.User = user;
                        await Page.DisplayAlert("Settings", "Save success", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Label_Message = "Save failed";
                    }
                }
            }
        }

        private double StringConvertToDouble(double oldValue, string newValue, string onPropertyChanged)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                OnPropertyChanged(onPropertyChanged);
                return 0;
            }
            else
            {
                double num = oldValue;
                if (Double.TryParse(newValue, out num) && oldValue != num)
                {
                    OnPropertyChanged(onPropertyChanged);
                    return num;
                }
                else
                {
                    return oldValue;
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
