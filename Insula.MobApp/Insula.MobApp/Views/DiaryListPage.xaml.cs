using Insula.MobApp.Data;
using Insula.MobApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Insula.MobApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DiaryListPage : ContentPage
	{
        public DiaryListViewModel DiaryListViewModel { get; private set; }
        public DiaryListPage ()
		{
			InitializeComponent();
            Init();
            DiaryListViewModel = new DiaryListViewModel(this) { Navigation = this.Navigation };
            this.BindingContext = DiaryListViewModel;
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
        }
        /*
        protected override void OnAppearing()
        {
            base.OnAppearing();
            DiaryListViewModel.LoadData();
            //this.BindingContext = DiaryListViewModel;
            //ListView_DiaryList.ItemsSource = DiaryListViewModel.DiaryList;  // = await App.RestService.GetResponse<List<DiaryItem>>(Constants.DiaryUrl);
        }*/

        void ToolbarItem_Clicked_Add(object sender, EventArgs e)
        {
            DiaryListViewModel.Add();
        }

        void ToolbarItem_Clicked_Logout(object sender, EventArgs e)
        {
            DiaryListViewModel.Logout();
        }

        void ToolbarItem_Clicked_Settings(object sender, EventArgs e)
        {
            DiaryListViewModel.Settings();
        }

        void ToolbarItem_Clicked_Calculator(object sender, EventArgs e)
        {
            DiaryListViewModel.Calculator();
        }

        void Item_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            DiaryListViewModel.Selected(sender, e);
        }
    }
}