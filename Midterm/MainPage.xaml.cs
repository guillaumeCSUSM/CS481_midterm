using System;
using System.Linq;
using System.Net.Http;
using Microcharts;
using Midterm.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace Midterm
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            HandleDailyStock("BA");
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
                var minValue = data.TimeSeriesDaily.Values.Min(t => t.Low);
                var maxValue = data.TimeSeriesDaily.Values.Max(t => t.High);
                LabelHighest.Text = maxValue;
                LabelLowest.Text = minValue;

                //CHARTS
                var entries = data.TimeSeriesDaily.Take(30).Select(t => {
                    return new Entry(float.Parse(t.Value.High))
                    {
                        ValueLabel = t.Value.High
                    };
                });
                var chart = new LineChart()
                {
                    Entries = entries,
                    MinValue = float.Parse(minValue),
                    MaxValue = float.Parse(maxValue),
                    LabelTextSize = 35
                };
                ChartView.Chart = chart;
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
