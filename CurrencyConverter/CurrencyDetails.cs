using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyConverter
{
    internal class CurrencyDetails
    {

        public double Amount { get; set; }

        public string Base { get; set; }

        public string start_date { get; set; }

        public string end_date { get; set; }

        public Object rates { get; set; }

    }


    internal class CurrencyConvertionDetails
    {

        public double Amount { get; set; }

        public string Base { get; set; }

        public string start_date { get; set; }

        public string end_date { get; set; }

        public Rates rates { get; set; }

    }


    internal class LatestCurrency
    {

        public double amount { get; set; }

        public string Base { get; set; }

        public string date { get; set; }

        public Rates rates { get; set; }

    }


    public class HistoricConversionRate
    {

        public string LatestConversionRate { get; set; }

        public string LatestConversionDate { get; set; }

        public List<ConverstionHistory> ConverstionHistory { get; set; }

    }

}
