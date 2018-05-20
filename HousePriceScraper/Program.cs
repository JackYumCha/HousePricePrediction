using System;
using System.Diagnostics;
using OpenQA.Selenium.Chrome;


namespace HousePriceScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using(ChromeDriver chromeDriver = new ChromeDriver())
            {
                chromeDriver.Url = "https://www.realestate.com.au/buy";
                chromeDriver.Navigate();

                var searchInputElement = chromeDriver.FindElementByCssSelector("input.rui-input.rui-location-box.rui-auto-complete-input");

                searchInputElement.SendKeys("Sunnybank, QLD 4109; ");

                var searchButtonElement = chromeDriver.FindElementByCssSelector("button.rui-search-button");

                searchButtonElement.Click();

                var mapLinkElement = chromeDriver.FindElementById("mapLink");

                mapLinkElement.Click();

                Debugger.Break();
            }
        }
    }
}
