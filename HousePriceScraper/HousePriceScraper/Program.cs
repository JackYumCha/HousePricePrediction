using System;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;



namespace HousePriceScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex csv = new Regex(@"~\-\-csv=");
            var argCsvPath = args.FirstOrDefault(arg => csv.IsMatch(arg));
            var pathCsv = csv.Replace(argCsvPath, "");
            using (StreamReader csvStreamReader = new StreamReader(pathCsv))
            {
                using(CsvReader csvReader = new CsvReader())


            }

        }
    }
}
