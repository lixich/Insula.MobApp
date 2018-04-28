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

        public double Insulin
        {
            get { return DiaryItem.Insulin; }
            set
            {
                if (DiaryItem.Insulin != value)
                { DiaryItem.Insulin = value; OnPropertyChanged("Insulin"); OnPropertyChanged("DisplayName"); }
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
        public double Carbo
        {
            get { return DiaryItem.Carbo; }
            set
            {
                if (DiaryItem.Carbo != value)
                { DiaryItem.Carbo = value; OnPropertyChanged("Carbo"); OnPropertyChanged("DisplayName"); }
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

        public double GlucoseAfter
        {
            get { return DiaryItem.GlucoseAfter; }
            set
            {
                if (DiaryItem.GlucoseAfter != value)
                { DiaryItem.GlucoseAfter = value; OnPropertyChanged("GlucoseAfter"); }
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
                    (Insulin <= 0) ||
                    (Carbo <= 0) ||
                    (GlucoseBefore <= 0) ||
                    (GlucoseAfter <= 0));
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
            await Navigation.PopAsync();
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
