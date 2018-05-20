using System;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace HousePriceScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Linux系统必备:
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--log-level=3");

            using (ChromeDriver chromeDriver = new ChromeDriver(options))
            {
                chromeDriver.Url = "https://www.realestate.com.au/buy";

                chromeDriver.Navigate();

                var searchInputElement = chromeDriver.FindElementByCssSelector("input.rui-input.rui-location-box.rui-auto-complete-input");
                searchInputElement.SendKeys("Sunybank, QLD 4109; ");

                var searchButtonElement = chromeDriver.FindElementByCssSelector("button.rui-search-button");

                searchButtonElement.Click();

               var advertisements = chromeDriver.FindElementsByCssSelector("div.listingInfo.rui-clearfix");

                List<String> addresses = new List<string>();

               foreach(var advertisement in advertisements)
                {
                    var addresssEle = advertisement.FindElement(By.CssSelector("div.vcard"));
                    addresses.Add(addresssEle.Text);

                }

                Debugger.Break();

            }


        }
    }
}
