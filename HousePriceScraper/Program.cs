using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
namespace HousePriceScraper
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--log-level=3");
            using (ChromeDriver chromeDriver = new ChromeDriver())
            {
                chromeDriver.Url = "https://www.realestate.com.au/buy";
                chromeDriver.Navigate();
                var searchInputElement = chromeDriver.FindElementByCssSelector("input.rui-input.rui-location-box.rui-auto-complete-input");
                searchInputElement.SendKeys("Sunnybank, QLD 4109; ");
                var searchButtonElement = chromeDriver.FindElementByCssSelector("button.rui-search-button");
                searchButtonElement.Click();
                var advertisements = chromeDriver.FindElementsByCssSelector("div.listingInfo.rui-clearfix");
                List<string> addresses = new List<string>();
                foreach(var advertisement in advertisements)
                {
                    var addressElement = advertisement.FindElement(By.CssSelector("div.vcard"));
                    addresses.Add(addressElement.Text);
                }

                //var mapLinkElement = chromeDriver.FindElementById("mapLink");

                //mapLinkElement.Click();

                Debugger.Break();
            }
        }
    }
}
