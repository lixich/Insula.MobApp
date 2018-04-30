using Insula.MobApp.Models;
using Insula.MobApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Insula.MobApp.ViewModel
{
    public class ForecastViewModel : INotifyPropertyChanged, ICloneable 
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public Page Page { get; set; }
        public DiaryItem DiaryItem { get; set; }

        public ForecastViewModel()
        {
            DiaryItem = new DiaryItem() { Time = DateTime.Now.ToString() };
        }

        public ForecastViewModel(Page page)
        {
            Page = page;
            DiaryItem = new DiaryItem() { Time = DateTime.Now.ToString() };
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

        public double Insulin
        {
            get { return DiaryItem.Insulin; }
            set
            {
                if (DiaryItem.Insulin != value)
                { DiaryItem.Insulin = value; OnPropertyChanged("Insulin"); }
            }
        }
        public string Time
        {
            get { return DiaryItem.Time; }
            set
            {
                if (DiaryItem.Time != value)
                { DiaryItem.Time = value; OnPropertyChanged("Time"); }
            }
        }
        public double Carbo
        {
            get { return DiaryItem.Carbo; }
            set
            {
                if (DiaryItem.Carbo != value)
                { DiaryItem.Carbo = value; OnPropertyChanged("Carbo"); }
            }
        }

        public double GlucoseBefore
        {
            get { return DiaryItem.GlucoseBefore; }
            set
            {
                if (DiaryItem.GlucoseBefore != value)
                { DiaryItem.GlucoseBefore = value; OnPropertyChanged("GlucoseBefore"); }
            }
        }

        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Time)) ||
                        (Insulin <= 0) ||
                        (Carbo <= 0) ||
                        (GlucoseBefore <= 0));
            }
        }

        private ObservableCollection<Forecast> _ForecastList;
        public ObservableCollection<Forecast> ForecastList
        {
            get { return _ForecastList; }
            set { _ForecastList = value; OnPropertyChanged("ForecastList"); }
        }

        private Forecast _SelectedForecast;
        public Forecast SelectedForecast
        {
            get { return _SelectedForecast; }
            set
            {
                if (_SelectedForecast != value && value != null)
                {
                    _SelectedForecast = value;
                    if (value != null)
                    {
                        Insulin = value.Value;
                        OnPropertyChanged("Insulin");
                    }
                    OnPropertyChanged("SelectedForecast");
                }
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

        public async void Save()
        {
            DiaryItem.UserId = App.User.Id;
            DiaryItem.GlucoseAfter = App.User.NormalGlucose;
            DiaryItem = await App.RestService.PostResponse<DiaryItem>(Constants.DiaryUrl, JsonConvert.SerializeObject(DiaryItem));
            DiaryListViewModel.DiaryList.Add(new DiaryItemViewModel(Page, DiaryItem));
            await Navigation.PopAsync();
        }

        public async void Forecast()
        {
            IsBusy = true;
            /*ForecastList = new ObservableCollection<Forecast>() { new Forecast() { Value = 1, Name = "name1" },
                                                                  new Forecast() { Value = 2, Name = "name2"  }};*/
            ForecastList = await App.RestService.PostResponse<ObservableCollection<Forecast>>(Constants.ForecastUrl, JsonConvert.SerializeObject(DiaryItem));
            if (ForecastList.Count > 0)
            {
                SelectedForecast = ForecastList[0];
            }
            IsBusy = false;
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
