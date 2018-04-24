﻿using Insula.MobApp.Models;
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
            Navigation.PopAsync();
        }

        void ToolbarItem_Clicked_Settings(object sender, EventArgs e)
        {
            var SignUpPage = new SignUpPage();
            SignUpPage.BindingContext = App.User;
            Navigation.PushAsync(SignUpPage);
        }

        void ToolbarItem_Clicked_Calculator(object sender, EventArgs e)
        {
            DisplayAlert("Calculator", "Dose insulin calculator", "OK");
            //Navigation.PushAsync(new CalculatorPage());
        }

        void Item_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var diaryItemPage = new DiaryItemPage();
            diaryItemPage.BindingContext = e.SelectedItem as DiaryItem;
            Navigation.PushAsync(diaryItemPage);
        }
    }
}