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
            DiaryItem = new DiaryItem();
        }

        public DiaryItemViewModel(Page page)
        {
            Page = page;
            DiaryItem = new DiaryItem();
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
        public string Time
        {
            get { return DiaryItem.Time; }
            set
            {
                if (DiaryItem.Time != value)
                { DiaryItem.Time = value; OnPropertyChanged("Time"); OnPropertyChanged("DisplayName"); }
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
            get { return $"{Time} Carbo: {Carbo} Dose: {Insulin}"; }
        }

        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Time)) ||
                    (DiaryItem.Insulin <= 0) ||
                    (DiaryItem.Carbo <= 0) ||
                    (DiaryItem.GlucoseBefore <= 0) ||
                    (DiaryItem.GlucoseAfter <= 0));
            }
        }

        public async void Save()
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
            }
            await Navigation.PopAsync();
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
