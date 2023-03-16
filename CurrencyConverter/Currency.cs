using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Currency
    {

        [Description("Australian Dollar")]
        AUD,
        [Description("Bulgarian Lev")]
        BGN,
        [Description("Brazilian Real")]
        BRL,
        [Description("Canadian Dollar")]
        CAD,
        [Description("Swiss Franc")]
        CHF,
        [Description("Chinese Renminbi Yuan")]
        CNY,
        [Description("Czech Koruna")]
        CZK,
        [Description("Danish Krone")]
        DKK,
        [Description("Euro")]
        EUR,
        [Description("British Pound")]
        GBP,
        [Description("Hong Kong Dollar")]
        HKD,
        [Description("Hungarian Forint")]
        HUF,
        [Description("Indonesian Rupiah")]
        IDR,
        [Description("Israeli New Sheqel")]
        ILS,
        [Description("Indian Rupee")]
        INR,
        [Description("Icelandic Króna")]
        ISK,
        [Description("Japanese Yen")]
        JPY,
        [Description("South Korean Won")]
        KRW,
        [Description("Mexican Peso")]
        MXN,
        [Description("Malaysian Ringgit")]
        MYR,
        [Description("Norwegian Krone")]
        NOK,
        [Description("New Zealand Dollar")]
        NZD,
        [Description("Philippine Peso")]
        PHP,
        [Description("Polish Złoty")]
        PLN,
        [Description("Romanian Leu")]
        RON,
        [Description("Swedish Krona")]
        SEK,
        [Description("Singapore Dollar")]
        SGD,
        [Description("Thai Baht")]
        THB,
        [Description("Turkish Lira")]
        TRY,
        [Description("United States Dollar")]
        USD,
        [Description("South African Rand")]
        ZAR


        //public string AUD { get; set; }
        //public string BGN { get; set; }
        //public string BRL { get; set; }
        //public string CAD { get; set; }
        //public string CHF { get; set; }
        //public string CNY { get; set; }
        //public string CZK { get; set; }
        //public string DKK { get; set; }
        //public string EUR { get; set; }
        //public string GBP { get; set; }
        //public string HKD { get; set; }
        //public string HUF { get; set; }
        //public string IDR { get; set; }
        //public string ILS { get; set; }
        //public string INR { get; set; }
        //public string ISK { get; set; }
        //public string JPY { get; set; }
        //public string KRW { get; set; }
        //public string MXN { get; set; }
        //public string MYR { get; set; }
        //public string NOK { get; set; }
        //public string NZD { get; set; }
        //public string PHP { get; set; }
        //public string PLN { get; set; }
        //public string RON { get; set; }
        //public string SEK { get; set; }
        //public string SGD { get; set; }
        //public string THB { get; set; }
        //public string TRY { get; set; }
        //public string USD { get; set; }
        //public string ZAR { get; set; }


    }
}