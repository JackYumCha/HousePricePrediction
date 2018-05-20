using System;
using System.IO;
using Xunit;
using Newtonsoft.Json;
using CsvHelper;

namespace HouseDataCleansing
{
    public class UnitTest1
    {
        [Fact(DisplayName = "Convert Data From Json To CSV")]
        public void ConvertDataFromJsonToCSV()
        {

            var fileList = Directory.GetFiles(@"C:\Users\erris\Desktop\House Data\4110");

            var resultCsv = @"C:\Users\erris\Desktop\House Data\4110.csv";

            var resultHistoryRoot = @"C:\Users\erris\Desktop\House Data\4110History";

            using(StreamWriter sw = new StreamWriter(resultCsv))
            {
                using(CsvWriter csv = new CsvWriter(sw))
                {
                    csv.WriteField("Unit");
                    csv.WriteField("StreetNumber");
                    csv.WriteField("StreetName");
                    csv.WriteField("StreetType");
                    csv.WriteField("StreetSuffix");
                    csv.WriteField("NumberOfBedrooms");
                    csv.WriteField("NumberOfBathrooms");
                    csv.WriteField("NumberOfParkings");
                    csv.WriteField("MiddlePrice");
                    csv.NextRecord();

                    foreach (string filename in fileList)
                    {
                        string json = File.ReadAllText(filename);
                        var property = JsonConvert.DeserializeObject<HousePriceScraper.Property>(json);

                        // number of rooms, number of parking, number of bathrooms
                        // year build, landsize

                        int? numberOfBedrooms = property.DomainBedroom.TryParseInt();

                        int? numberOfBathrooms = property.DomainBathroom.TryParseInt();

                        int? numberOfParkings = property.DomainParking.TryParseInt();

                        double? middle = property.DomainMidValue.TryParsePrice();

                        csv.WriteField(property.UnitNumber);
                        csv.WriteField(property.HouseNumber);
                        csv.WriteField(property.StreetName);
                        csv.WriteField(property.StreetType);
                        csv.WriteField(property.StreetSuffix);
                        csv.WriteField(numberOfBedrooms);
                        csv.WriteField(numberOfBathrooms);
                        csv.WriteField(numberOfParkings);
                        csv.WriteField(middle);
                        csv.NextRecord();

                        if(property.DomainRecords != null && property.DomainRecords.Count > 0)
                        {

                            using (StreamWriter swHis = new StreamWriter($@"{resultHistoryRoot}\{property._key}.csv"))
                            {
                                using (CsvWriter csvHis = new CsvWriter(swHis))
                                {

                                    csvHis.WriteField("Action");
                                    csvHis.WriteField("Agent");
                                    csvHis.WriteField("Date");
                                    csvHis.WriteField("Value");
                                    csvHis.NextRecord();

                                    foreach (var record in property.DomainRecords)
                                    {
                                        csvHis.WriteField(record.Action);
                                        csvHis.WriteField(record.Agent);
                                        csvHis.WriteField(record.Date);
                                        csvHis.WriteField(record.Value);
                                        csvHis.NextRecord();
                                    }

                                }
                            }
                        }
                    }
                }
            }
            

        }

    }
}
