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
	public partial class ForecastPage : ContentPage
	{
        public ForecastViewModel ForecastViewModel { get; private set; }
        public ForecastPage()
		{
			InitializeComponent();
            Init();
            ForecastViewModel = new ForecastViewModel(this) { Navigation = this.Navigation };
            this.BindingContext = ForecastViewModel;
        }

        public ForecastPage(ForecastViewModel forecastViewModel)
        {
            InitializeComponent();
            Init();
            ForecastViewModel = forecastViewModel;
            this.BindingContext = ForecastViewModel;
        }


        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Label_Time.TextColor = Constants.TextColor;
            Entry_Time.IsEnabled = false;
            Label_Insulin.TextColor = Constants.TextColor;
            Label_Carbo.TextColor = Constants.TextColor;
            Label_GlucoseBefore.TextColor = Constants.TextColor;
        }

        void Button_Clicked_Save(object sender, EventArgs e)
        {
            ForecastViewModel.Save();
        }

        void Button_Clicked_Forecast(object sender, EventArgs e)
        {
            ForecastViewModel.Forecast();
        }
    }
}