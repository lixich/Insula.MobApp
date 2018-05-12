using Insula.MobApp.Data;
using Insula.MobApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Insula.MobApp.ViewModel
{
    public class DiaryItemViewModel : INotifyPropertyChanged, ICloneable 
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public DiaryItem DiaryItem { get; set; }
        public Page Page { get; set; }

        public DiaryItemViewModel()
        {
            DiaryItem = new DiaryItem() { Time = DateTime.Now };
        }

        public DiaryItemViewModel(Page page)
        {
            Page = page;
            DiaryItem = new DiaryItem() { Time = DateTime.Now };
        }

        public DiaryItemViewModel(Page page, DiaryItem diaryItem)
        {
            Page = page;
            DiaryItem = diaryItem;
        }

        DiaryListViewModel _DiaryListViewModel;
        public DiaryListViewModel DiaryListViewModel
        {
            get { return _DiaryListViewModel; }
            set
            {
                if (_DiaryListViewModel != value)
                {
                    _DiaryListViewModel = value;
                    OnPropertyChanged("DiaryListViewModel");
                }
            }
        }

        public bool CanDelete
        {
            get
            {
                if (DiaryItem.Id == 0)
                    return false;
                else return true;
            }
        }

        public int Id
        {
            get { return DiaryItem.Id; }
            private set { DiaryItem.Id = value; }
        }

        public int UserId
        {
            get { return DiaryItem.UserId; }
            private set { DiaryItem.UserId = value; }
        }

        public string uri
        {
            get { return DiaryItem.uri; }
            private set { DiaryItem.uri = value; }
        }

        public string Insulin
        {
            get { return DiaryItem.Insulin.ToString(); }
            set
            {
                if (DiaryItem.Insulin.ToString() != value)
                { DiaryItem.Insulin = StringConvertToDouble(DiaryItem.Insulin, value, "Insulin");  OnPropertyChanged("DisplayName"); }
            }
        }

        public DateTime Date
        {
            get { return DiaryItem.Time.Date; }
            set
            {
                if (DiaryItem.Time.Date != value)
                {
                    var dateTime = new DateTime(value.Year, value.Month, value.Day);
                    dateTime = dateTime.Add(Time);
                    DiaryItem.Time = dateTime;
                    OnPropertyChanged("Date");
                    OnPropertyChanged("DisplayName");
                }
            }
        }

        public TimeSpan Time
        {
            get { return DiaryItem.Time.TimeOfDay; }
            set
            {
                if (DiaryItem.Time.TimeOfDay != value)
                {
                    var dateTime = new DateTime(Date.Year, Date.Month, Date.Day);
                    dateTime = dateTime.Add(value);
                    DiaryItem.Time = dateTime;
                    OnPropertyChanged("Time");
                    OnPropertyChanged("DisplayName");
                }
            }
        }

        public string Carbo
        {
            get { return DiaryItem.Carbo.ToString(); }
            set
            {
                if (DiaryItem.Carbo.ToString() != value)
                { DiaryItem.Carbo = StringConvertToDouble(DiaryItem.Carbo, value, "Carbo"); OnPropertyChanged("DisplayName"); }
            }
        }

        public string GlucoseBefore
        {
            get { return DiaryItem.GlucoseBefore.ToString(); }
            set
            {
                if (DiaryItem.GlucoseBefore.ToString() != value)
                { DiaryItem.GlucoseBefore = StringConvertToDouble(DiaryItem.GlucoseBefore, value, "GlucoseBefore"); }
            }
        }

        public string GlucoseAfter
        {
            get { return DiaryItem.GlucoseAfter.ToString(); }
            set
            {
                if (DiaryItem.GlucoseAfter.ToString() != value)
                { DiaryItem.GlucoseAfter = StringConvertToDouble(DiaryItem.GlucoseAfter, value, "GlucoseAfter"); }
            }
        }

        public string DisplayName
        {
            get { return $"{Date.Date.ToShortDateString()} {Time.ToString().Substring(0, 5)} Carbo: {Carbo} Dose: {Insulin}"; }
        }

        public bool IsValid
        {
            get
            {
                return ((Time != null) &&
                    (DiaryItem.Insulin >= 0) &&
                    (DiaryItem.Carbo >= 0) &&
                    (DiaryItem.GlucoseBefore >= 0) &&
                    (DiaryItem.GlucoseAfter >= 0));
            }
        }

        public async void Save()
        {
            if (IsValid)
            {
                if (DiaryItem.Id == 0)
                {
                    DiaryItem.UserId = App.User.Id;
                    DiaryItem = await App.RestService.PostResponse<DiaryItem>(Constants.DiaryUrl, JsonConvert.SerializeObject(DiaryItem));
                    DiaryListViewModel.DiaryList.Add(this);
                }
                else
                {
                    DiaryItem = await App.RestService.PutResponse<DiaryItem>(DiaryItem.uri, JsonConvert.SerializeObject(DiaryItem));
                    for (int i = 0; i < DiaryListViewModel.DiaryList.Count; i++)
                    {
                        var diaryItem = DiaryListViewModel.DiaryList[i];
                        if (diaryItem.Id == DiaryItem.Id)
                            DiaryListViewModel.DiaryList[i] = this;
                    }
                }
                await Navigation.PopAsync();
            }
            else
            {
                await Page.DisplayAlert("Fields", "Fields not correct or empty.", "OK");
            }
        }

        public async void Delete()
        {
            var result = await App.RestService.DeleteResponse(DiaryItem.uri);
            if (result)
            {
                var diaryItemToRemove = this;
                for (int i = 0; i < DiaryListViewModel.DiaryList.Count; i++)
                {
                    var diaryItem = DiaryListViewModel.DiaryList[i];
                    if (diaryItem.Id == DiaryItem.Id)
                        diaryItemToRemove = diaryItem;
                }
                DiaryListViewModel.DiaryList.Remove(diaryItemToRemove);
                await Navigation.PopAsync();
            }
            else
            {
                await Page.DisplayAlert("Delete", "Error deleting, try to update the data.", "OK");
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

        public object Clone()
        {
            var diaryItem = (DiaryItem)DiaryItem.Clone();
            return new DiaryItemViewModel
            {
                DiaryItem = diaryItem,
                Navigation = this.Navigation,
                Page = this.Page,
                DiaryListViewModel = this.DiaryListViewModel
            };
        }
    }
}
