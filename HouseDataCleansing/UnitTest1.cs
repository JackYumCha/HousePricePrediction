using System;
using System.Collections.Generic;
using System.Linq;
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

            DirectoryInfo rootData = new DirectoryInfo(@"C:\Users\dpeaudstempcal\Documents\DomainData\Data");

            List<FileInfo> allFiles = rootData.GetDirectories().Aggregate(new List<FileInfo>(), (list, dir) =>
            {
                list.AddRange(dir.GetFiles());
                return list;
            });

            var resultCsv = @"C:\Users\dpeaudstempcal\Documents\DomainData\data.csv";

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

                    foreach (FileInfo filename in allFiles)
                    {
                        string json = File.ReadAllText(filename.FullName);
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
