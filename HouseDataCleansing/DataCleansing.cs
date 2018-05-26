using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xunit;
using Newtonsoft.Json;
using CsvHelper;
using HousePriceScraper;

namespace HouseDataCleansing
{
    public class DataCleansing
    {
        [Fact(DisplayName = "Convert Data From Json To CSV")]
        public void ConvertDataFromJsonToCSV()
        {

            DirectoryInfo rootData = new DirectoryInfo(@"D:\VSTS\Repos\HouseData");

            List<FileInfo> allFiles = rootData.GetDirectories().Aggregate(new List<FileInfo>(), (list, dir) =>
            {
                list.AddRange(dir.GetFiles());
                return list;
            });

            var featureCsv = @"D:\VSTS\Repos\HouseData\x.csv";
            var targetCsv = @"D:\VSTS\Repos\HouseData\y.csv";
            var indexKexCsv = @"D:\VSTS\Repos\HouseData\index-key.csv";

            // var resultHistoryRoot = @"C:\Users\erris\Desktop\House Data\4110History";

            
            using (StreamWriter swFeature = new StreamWriter(featureCsv))
            {
                using(CsvWriter csvFeature = new CsvWriter(swFeature))
                {

                    using (StreamWriter swTarget = new StreamWriter(targetCsv))
                    {
                        using (CsvWriter csvTarget = new CsvWriter(swTarget))
                        {
                            using (StreamWriter swIndexKey = new StreamWriter(indexKexCsv))
                            {
                                using (CsvWriter csvIndexKey = new CsvWriter(swIndexKey))
                                {
                                    //csvFeature.WriteField("index");
                                    //csv.WriteField("Unit");
                                    //csv.WriteField("StreetNumber");
                                    //csv.WriteField("StreetName");
                                    //csv.WriteField("StreetType");
                                    //csv.WriteField("StreetSuffix");
                                    //csv.WriteField("MiddlePrice");

                                    csvFeature.WriteField("NumberOfBedrooms");
                                    csvFeature.WriteField("NumberOfBathrooms");
                                    csvFeature.WriteField("NumberOfParkings");

                                    csvFeature.WriteField("Longitude");
                                    csvFeature.WriteField("Latitude");
                                    //csvFeature.WriteField("index");

                                    csvFeature.NextRecord();

                                    csvTarget.WriteField("index");
                                    csvTarget.NextRecord();

                                    csvIndexKey.WriteField("index");
                                    csvIndexKey.WriteField("key");
                                    csvIndexKey.WriteField("postcode");
                                    csvIndexKey.NextRecord();

                                    int index = 0;

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

                                        double? lat = property.Lat.TryParseDouble();
                                        double? lng = property.Lng.TryParseDouble();

                                        if (!numberOfBedrooms.HasValue || numberOfBedrooms.Value == 0)
                                            continue;
                                        if (!numberOfBathrooms.HasValue || numberOfBathrooms.Value == 0)
                                            continue;
                                        if (!middle.HasValue || middle.Value == 0)
                                            continue;
                                        if (property.Lat == null || property.Lat == "")
                                            continue;
                                        if (property.Lng == null || property.Lng == "")
                                            continue;

                                        csvFeature.WriteField(numberOfBedrooms);
                                        csvFeature.WriteField(numberOfBathrooms);
                                        csvFeature.WriteField(numberOfParkings);
                                        csvFeature.WriteField(property.Lat);
                                        csvFeature.WriteField(property.Lng);
                                        //csvFeature.WriteField(index);
                                        csvFeature.NextRecord();

                                        csvTarget.WriteField(index);
                                        csvTarget.NextRecord();

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
