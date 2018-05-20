using System;
using System.IO;
using Xunit;
using Newtonsoft.Json;

namespace HouseDataCleansing
{
    public class UnitTest1
    {
        [Fact(DisplayName = "Convert Data From Json To CSV")]
        public void ConvertDataFromJsonToCSV()
        {

            var fileList = Directory.GetFiles(@"C:\Users\erris\Desktop\House Data\4110");

            foreach(string filename in fileList)
            {
                string json =  File.ReadAllText(filename);
                var property = JsonConvert.DeserializeObject<HousePriceScraper.Property>(json);

                // number of rooms, number of parking, number of bathrooms
                // year build, landsize

                 

            }

        }

    }
}
