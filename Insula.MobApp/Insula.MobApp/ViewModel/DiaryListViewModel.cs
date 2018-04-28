using Insula.MobApp.Data;
using Insula.MobApp.Models;
using Insula.MobApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Insula.MobApp.ViewModel
{
    public class DiaryListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CreateFriendCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        public INavigation Navigation { get; set; }
        public Page Page { get; set; }

        public DiaryListViewModel(Page page)
        {
            Page = page;
            LoadData();
            CreateFriendCommand = new Command(CreateDiaryItem);
            BackCommand = new Command(Back);
        }

        public async void LoadData()
        {
            var DiaryItems = await App.RestService.GetResponse<List<DiaryItem>>(Constants.DiaryUrl);
            DiaryList = new ObservableCollection<DiaryItemViewModel>();
            foreach (var item in DiaryItems)
                DiaryList.Add(new DiaryItemViewModel(Page, item) { DiaryListViewModel = this, Navigation = this.Navigation });
        }
        //public ObservableCollection<DiaryItem> DiaryItems { get; set; }


        private ObservableCollection<DiaryItemViewModel> _DiaryList;
        public ObservableCollection<DiaryItemViewModel> DiaryList
        {
            get { return _DiaryList; }
            set { _DiaryList = value; OnPropertyChanged("DiaryList");  }
        }

        DiaryItemViewModel _SelectedDiaryItem;
        public DiaryItemViewModel SelectedDiaryItem
        {
            get { return _SelectedDiaryItem; }
            set
            {
                if (_SelectedDiaryItem != value)
                {
                    //DiaryItemViewModel tempDiaryItem = value;
                    //tempDiaryItem.DiaryItem = (DiaryItem) value.DiaryItem.Clone();
                    //DiaryItemViewModel tempDiaryItem = new DiaryItemViewModel() { DiaryItem = (DiaryItem)value.DiaryItem.Clone() };
                    DiaryItemViewModel tempDiaryItem = (DiaryItemViewModel) value.Clone();
                    _SelectedDiaryItem = null;
                    OnPropertyChanged("SelectedDiaryItem");
                    Navigation.PushAsync(new DiaryItemPage(tempDiaryItem), true);
                }
            }
        }

        private void CreateDiaryItem()
        {
            Navigation.PushAsync(new DiaryItemPage(new DiaryItemViewModel(Page) { DiaryListViewModel = this }));
        }

        public void Add()
        {
            Navigation.PushAsync(new DiaryItemPage());
            /*
            var diaryItemPage = new DiaryItemPage();
            diaryItemPage.BindingContext = new DiaryItem();
            Navigation.PushAsync(diaryItemPage);
            */
        }

        public void Logout()
        {
            App.RestService.Logout();
            Navigation.InsertPageBefore(new SignInPage(), Page);
            Navigation.PopAsync();
        }

        public void Settings()
        {
            Navigation.PushAsync(new SignUpPage());
        }

        public void Calculator()
        {
            Page.DisplayAlert("Calculator", "Dose insulin calculator", "OK");
            //Navigation.PushAsync(new CalculatorPage());
        }

        public void Selected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new DiaryItemPage(SelectedDiaryItem));
            /*
            var diaryItemPage = new DiaryItemPage();
            diaryItemPage.BindingContext = e.SelectedItem as DiaryItemViewModel;
            Navigation.PushAsync(diaryItemPage);
            */
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
