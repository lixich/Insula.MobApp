using Insula.MobApp.Models;
using Newtonsoft.Json;
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
	public partial class DiaryItemPage : ContentPage
	{
		public DiaryItemPage ()
		{
			InitializeComponent ();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Label_Time.TextColor = Constants.TextColor;
            Label_Insulin.TextColor = Constants.TextColor;
            Label_Carbo.TextColor = Constants.TextColor;
            Label_GlucoseBefore.TextColor = Constants.TextColor;
            Label_GlucoseAfter.TextColor = Constants.TextColor;
        }
        async void Button_Clicked_Save(object sender, EventArgs e)
        {
            var diaryItem = (DiaryItem)BindingContext;
            if (diaryItem.Id == 0)
            {
                diaryItem.UserId = App.User.Id;
                diaryItem = await App.RestService.PostResponse<DiaryItem>(Constants.DiaryUrl, JsonConvert.SerializeObject(diaryItem));
            }
            else
            {
                diaryItem = await App.RestService.PutResponse<DiaryItem>(diaryItem.Uri, JsonConvert.SerializeObject(diaryItem));
            }
            await Navigation.PopAsync();
        }

        async void Button_Clicked_Delete(object sender, EventArgs e)
        {
            var diaryItem = (DiaryItem)BindingContext;
            var result = await App.RestService.DeleteResponse(diaryItem.Uri);
            await Navigation.PopAsync();
        }

        void Button_Clicked_Cancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}