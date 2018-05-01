using Insula.MobApp.Models;
using Insula.MobApp.ViewModel;
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
        public DiaryItemViewModel DiaryItemViewModel { get; private set; }

        public DiaryItemPage ()
		{
			InitializeComponent();
            Init();
            DiaryItemViewModel = new DiaryItemViewModel(this) { Navigation = this.Navigation };
            this.BindingContext = DiaryItemViewModel;
        }

        public DiaryItemPage(DiaryItemViewModel diaryItemViewModel)
        {
            InitializeComponent();
            Init();
            DiaryItemViewModel = diaryItemViewModel;
            this.BindingContext = DiaryItemViewModel;
        }


        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Label_Time.TextColor = Constants.TextColor;
            Label_Date.TextColor = Constants.TextColor;
            Label_Insulin.TextColor = Constants.TextColor;
            Label_Carbo.TextColor = Constants.TextColor;
            Label_GlucoseBefore.TextColor = Constants.TextColor;
            Label_GlucoseAfter.TextColor = Constants.TextColor;
        }

        void Button_Clicked_Save(object sender, EventArgs e)
        {
            DiaryItemViewModel.Save();
        }

        void Button_Clicked_Delete(object sender, EventArgs e)
        {
            DiaryItemViewModel.Delete();
        }
    }
}