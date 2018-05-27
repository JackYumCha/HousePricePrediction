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

            DirectoryInfo rootData = new DirectoryInfo(@"C:\Users\erris\Desktop\New Data");

            List<FileInfo> allFiles = rootData.GetDirectories().Aggregate(new List<FileInfo>(), (list, dir) =>
            {
                list.AddRange(dir.GetFiles());
                return list;
            });

            var xCsv = @"C:\Users\erris\Desktop\New Data\x.csv";
            var yCsv = @"C:\Users\erris\Desktop\New Data\y.csv";
            var indexKeyCsv = @"C:\Users\erris\Desktop\New Data\index-key.csv";

            using(StreamWriter swX = new StreamWriter(xCsv))
            {
                using(CsvWriter csvX = new CsvWriter(swX))
                {
                    using (StreamWriter swY = new StreamWriter(yCsv))
                    {
                        using (CsvWriter csvY = new CsvWriter(swY))
                        {
                            using (StreamWriter swIndexKey = new StreamWriter(indexKeyCsv))
                            {
                                using (CsvWriter csvIndexKey = new CsvWriter(swIndexKey))
                                {
                                    csvX.WriteField("NumberOfBedrooms");
                                    csvX.WriteField("NumberOfBathrooms");
                                    csvX.WriteField("NumberOfParkings");
                                    csvX.WriteField("Latitude");
                                    csvX.WriteField("Longitude");
                                    csvX.NextRecord();

                                    int index = 0;

                                    csvY.WriteField("HouseIndex");
                                    csvY.NextRecord();

                                    csvIndexKey.WriteField("HouseIndex");
                                    csvIndexKey.WriteField("HouseKey");
                                    csvIndexKey.WriteField("Postcode");

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

                                        if (!numberOfBedrooms.HasValue || numberOfBedrooms.Value == 0)
                                            continue;
                                        if (!numberOfBathrooms.HasValue || numberOfBathrooms.Value == 0)
                                            continue;
                                        if (!numberOfParkings.HasValue)
                                            numberOfParkings = 0;

                                        if (property.Lat == null || property.Lat == "")
                                            continue;
                                        if (property.Lng == null || property.Lng == "")
                                            continue;

                                        csvX.WriteField(numberOfBedrooms);
                                        csvX.WriteField(numberOfBathrooms);
                                        csvX.WriteField(numberOfParkings);
                                        csvX.WriteField(property.Lat);
                                        csvX.WriteField(property.Lng);
                                        csvX.NextRecord();

                                        csvY.WriteField(index);
                                        csvY.NextRecord();

                                        csvIndexKey.WriteField(index);
                                        csvIndexKey.WriteField(property._key);
                                        csvIndexKey.WriteField(property.Postcode);
                                        csvIndexKey.NextRecord();


                                        index += 1;
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
