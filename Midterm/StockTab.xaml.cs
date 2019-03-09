using System;
using System.Collections.Generic;
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
            HandleDailyStock();
        }

        async void HandleDailyStock()
        {
            var client = new HttpClient();
            var apiAddress = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=MSFT&apikey=TJJE976QPL0ESQKJ";
            var uri = new Uri(apiAddress);

            var data = new Stock();
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<Stock>(jsonContent);
            }
            StockListView.ItemsSource = data.TimeSeriesDaily;
        }

        async void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var client = new HttpClient();
            var apiAddress = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=" + SearchBar.Text + "&apikey=TJJE976QPL0ESQKJ";
            var uri = new Uri(apiAddress);

            var data = new SearchStock();
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<SearchStock>(jsonContent);
            }
            SearchListView.ItemsSource = data.BestMatches;
        }
    }
}
