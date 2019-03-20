using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Midterm.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Midterm
{
    public partial class StockTab : ContentPage
    {

        public StockTab()
        {
            InitializeComponent();
            HandleDailyStock("MSFT");
        }

        async void HandleDailyStock(String symbolName)
        {
            var client = new HttpClient();
            var apiAddress = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + symbolName + "&apikey=TJJE976QPL0ESQKJ";
            var uri = new Uri(apiAddress);

            var data = new Stock();
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<Stock>(jsonContent);
            }
            StockListView.ItemsSource = data.TimeSeriesDaily;
            if (data.TimeSeriesDaily != null)
            {
                LabelHighest.Text = data.TimeSeriesDaily.Values.Max(t => t.High);
                LabelLowest.Text = data.TimeSeriesDaily.Values.Min(t => t.Low);
            }
            else
            {
                LabelHighest.Text = "";
                LabelLowest.Text = "";
                symbolName += " - Not found";
            }
            LabelSymbol.Text = symbolName;
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            HandleDailyStock(SearchBar.Text);
        }
    }
}
