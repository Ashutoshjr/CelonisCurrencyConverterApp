using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Timers;

namespace CurrencyConverter
{
    internal class MainViewModel : INotifyPropertyChanged
    {

        private readonly Queue<KeyValuePair<string, HistoricConversionRate>> requestQueue = new Queue<KeyValuePair<string, HistoricConversionRate>>();

        public List<ConverstionHistory> HistoryData { get; set; }

        public MainViewModel()
        {
            FromCurrency = Currency.EUR;
            ToCurrency = Currency.USD;
            Amount = 1;
            StartDate = new DateTime(2000, 01, 01);
            EndDate = new DateTime(2000, 01, 10);

            System.Timers.Timer newTimer = new System.Timers.Timer();
            newTimer.Elapsed += new ElapsedEventHandler(RemoveOldCachedata);
            newTimer.Interval = 900000;  //every 15 min remove old cache data 
            newTimer.Start();
        }

        private void RemoveOldCachedata(object sender, ElapsedEventArgs e)
        {
            try
            {
                requestQueue.Dequeue();
                //item remove
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Exception" + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        //IConvertible converter = null;

        private Currency fromCurrency;
        private Currency toCurrency;
        private double amount;
        private double exchangedAmount;
        private DateTime startDate;
        private DateTime endDate;
        private HistoricConversionRate historicConversionRate;
        private string tempToCurrencyValue;
        private string tempFromCurrencyValue;


        private ObservableCollection<HistoricConversionRate> obserHistoricRate = new ObservableCollection<HistoricConversionRate>();

        public ObservableCollection<HistoricConversionRate> ObserHistoricRate
        {
            get => obserHistoricRate;
            set => obserHistoricRate = value;
        }

        public HistoricConversionRate HistoricConversionRate
        {
            get => historicConversionRate;
            set
            {
                historicConversionRate = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }

        public Currency FromCurrency
        {
            get => fromCurrency;
            set
            {
                fromCurrency = value;
                OnPropertyChanged();
            }
        }

        public Currency ToCurrency
        {
            get => toCurrency;
            set
            {
                toCurrency = value;
                OnPropertyChanged();
            }
        }
        public double Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        public double ExchangedAmount
        {
            get => exchangedAmount;
            private set
            {
                exchangedAmount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// get the current currency details.
        /// </summary>
        /// <returns></returns>
        private async Task ConvertCurrency()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.frankfurter.app/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string url = $"{client.BaseAddress}latest?amount={Amount}&from={FromCurrency}&to={ToCurrency}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var currencyData = JsonConvert.DeserializeObject<CurrencyConvertionDetails>(await response.Content.ReadAsStringAsync());

                        ExchangedAmount = getCurrencyValue(currencyData.rates);

                    }
                    else
                    {
                        MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Exception :" + ex.Message);
            }
        }


        double getCurrencyValue(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(double))
                {
                    double value = (double)pi.GetValue(myObject);
                    if (value > 0)
                    {
                        return value;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// get the historic currency details by calling API
        /// </summary>
        /// <returns></returns>
        private async Task GetHistoricalData()
        {
            try
            {
                bool IsExist = false;

                string baseAddress = "https://api.frankfurter.app/";
                string dateRange = $"{StartDate.ToString("yyyy-MM-dd")}..{endDate.ToString("yyyy-MM-dd")}";
                string fromAndTo = $"?from={FromCurrency}&to={ToCurrency}";
                var historicApiURI = new Uri($"{baseAddress}{dateRange}{fromAndTo}").ToString();

                if (requestQueue.Any(s => s.Key == historicApiURI) && tempToCurrencyValue == toCurrency.ToString() && tempFromCurrencyValue == fromCurrency.ToString())
                    IsExist = true;

                tempToCurrencyValue = toCurrency.ToString();
                tempFromCurrencyValue = fromCurrency.ToString();

                if (!IsExist)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        string uri = historicApiURI;//$"{client.BaseAddress}{dateRange}";
                        var response = await client.GetAsync(uri);
                        if (response.IsSuccessStatusCode)
                        {
                            var currencyHistoryData = JsonConvert.DeserializeObject<CurrencyDetails>(await response.Content.ReadAsStringAsync());

                            dynamic ratesData = ((IEnumerable)currencyHistoryData.rates).Cast<object>().ToList();
                            HistoryData = new List<ConverstionHistory>();

                            foreach (var item in ratesData)
                            {
                                foreach (var item1 in item.Value)
                                {
                                    var chistory = new ConverstionHistory();
                                    chistory.Date = item.Name;
                                    chistory.CurrencyType = item1.Name;
                                    chistory.Ratefrom = item1.Value.Value;
                                    HistoryData.Add(chistory);
                                }
                            }

                            //Call API to get latest currency details
                            string latestAvailableConversionDate = string.Empty;
                            string LatestCurrencyValue = $"{baseAddress}latest?from={fromCurrency.ToString()}";
                            var latestCurrencyResponse = await client.GetAsync(LatestCurrencyValue);
                            if (latestCurrencyResponse.IsSuccessStatusCode)
                                latestAvailableConversionDate = JsonConvert.DeserializeObject<LatestCurrency>(await latestCurrencyResponse.Content.ReadAsStringAsync()).date;

                            HistoricConversionRate = new HistoricConversionRate
                            {
                                ConverstionHistory = HistoryData,
                                LatestConversionRate = ExchangedAmount.ToString(),
                                LatestConversionDate = latestAvailableConversionDate,
                            };

                            if (requestQueue.Count < 50) //need to add this value in configuration
                            {
                                requestQueue.Enqueue(new KeyValuePair<string, HistoricConversionRate>(historicApiURI, HistoricConversionRate));
                                ObserHistoricRate.Add(HistoricConversionRate);
                            }
                            else
                            {
                                requestQueue.Dequeue();
                                requestQueue.Enqueue(new KeyValuePair<string, HistoricConversionRate>(historicApiURI, HistoricConversionRate));
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Historic data is not avaiable for given start and end date. Please try to change the dates");
                        }
                    }
                }
                else
                {
                    ObserHistoricRate.Clear();
                    ObserHistoricRate.Add(requestQueue.FirstOrDefault(t => t.Key == historicApiURI).Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception :" + ex.Message);
            }
        }


        public Array Currencies => Enum.GetValues(typeof(Currency));

        //public ICommand ConvertCommand { get; set; }

        private void OnPropertyChanged([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));

                if (property == "Amount" || property == "ToCurrency" || property == "FromCurrency" || property == "StartDate" || property == "EndDate")
                {
                    _ = StartCurrencyConversionTask();
                }
            }
        }

        /// <summary>
        /// Starts the currency conversion process by calling apis
        /// </summary>
        /// <returns></returns>
        public async Task StartCurrencyConversionTask()
        {
            try
            {
                //get current rate of currency
                await ConvertCurrency();

                //get historic Data
                await GetHistoricalData();
            }
            catch (Exception)
            {


            }
        }
    }
}
