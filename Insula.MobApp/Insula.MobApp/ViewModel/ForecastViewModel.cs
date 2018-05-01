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
            DiaryItem = new DiaryItem() { Time = DateTime.Now };
        }

        public ForecastViewModel(Page page)
        {
            Page = page;
            DiaryItem = new DiaryItem() { Time = DateTime.Now };
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

        public string Insulin
        {
            get { return DiaryItem.Insulin.ToString(); }
            set
            {
                if (DiaryItem.Insulin.ToString() != value)
                { DiaryItem.Insulin = StringConvertToDouble(DiaryItem.Insulin, value, "Insulin"); }
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
                }
            }
        }

        public string Carbo
        {
            get { return DiaryItem.Carbo.ToString(); }
            set
            {
                if (DiaryItem.Carbo.ToString() != value)
                { DiaryItem.Carbo = StringConvertToDouble(DiaryItem.Carbo, value, "Carbo"); }
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

        public bool IsValid
        {
            get
            {
                return ((Time != null) ||
                        (DiaryItem.Insulin <= 0) ||
                        (DiaryItem.Carbo <= 0) ||
                        (DiaryItem.GlucoseBefore <= 0));
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
                        Insulin = value.Value.ToString();
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
