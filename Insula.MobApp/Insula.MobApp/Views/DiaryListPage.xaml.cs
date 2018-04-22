using Insula.MobApp.Models;
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
		public DiaryListPage ()
		{
			InitializeComponent ();
            Init();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            ListView_DiaryList.ItemsSource = await App.RestService.GetResponse<List<DiaryItem>>(Constants.DiaryUrl);
        }
        
        void ToolbarItem_Clicked_Add(object sender, EventArgs e)
        {
            var diaryItemPage = new DiaryItemPage();
            diaryItemPage.BindingContext = new DiaryItem();
            Navigation.PushAsync(diaryItemPage);
        }

        void ToolbarItem_Clicked_Logout(object sender, EventArgs e)
        {
            App.RestService.Logout();
            Navigation.InsertPageBefore(new SignInPage(), this);
            //Navigation.PushAsync(new SignInPage());
            Navigation.PopAsync();
        }

        void Item_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var diaryItemPage = new DiaryItemPage();
            diaryItemPage.BindingContext = e.SelectedItem as DiaryItem;
            Navigation.PushAsync(diaryItemPage);
        }
    }
}