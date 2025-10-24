/*
 * This file hosts the class that download and store ECMWF IFS data. 
 * 
 * (C) Eric J. Drewitz 2025
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxDataSharp.Utilities;
using WxDataSharp.Client;

namespace WxDataSharp.ECMWFIFS
{
    public class ECMWFIFS
    {

        public static async Task DownloadECMWFIFS(int step, int finalForecastHour)
        {
            /*
            This function does the following:
            
            1) Downloads the latest available ECMWF IFS data

            2) Saves the ECMWF IFS data to f:ECMWF/IFS/{fileName}

            Required Arguments:

            1) int step - The forecast step interval. Must be 3 or 6. 

            2) int finalForecastHour - The final forecast hour to download. Must be a multiple of 3 or 6 before 144 hours and 6 after 144 hours.

            Optional Arguments: None

            Returns
            -------

            ECMWF IFS data files saved to f:ECMWF/IFS/{fileName}
            */

            DateTime utcNow = DateTime.UtcNow;
            DateTime yDay = utcNow.AddDays(-1);
            int hour = utcNow.Hour;
            string run = "";
            DateTime time = utcNow;

            if ((hour >= 8) && (hour < 20))
            {
                run = "00";
            }
            else if ((hour >= 20) && (hour < 24))
            {
                run = "12";
            }
            else
            {
                run = "12";
                time = yDay;
            }

            List<string> url_list = [];


            if (finalForecastHour <= 144)
            {

                for (int i = 0; i < (finalForecastHour + step); i += step)
                {
                    string u = $"https://data.ecmwf.int/forecasts/{time.ToString("yyyyMMdd")}/{run}z/ifs/0p25/oper/{time.ToString("yyyyMMdd")}{run}0000-{i}h-oper-fc.grib2";

                    url_list.Add(u);
                }
            }
            else
            {
                for (int i = 0; i < (144 + step); i += step)
                {
                    string u = $"https://data.ecmwf.int/forecasts/{time.ToString("yyyyMMdd")}/{run}z/ifs/0p25/oper/{time.ToString("yyyyMMdd")}{run}0000-{i}h-oper-fc.grib2";

                    url_list.Add(u);
                }

                for (int i = 144; i < (finalForecastHour + 6); i += 6)
                {
                    string u = $"https://data.ecmwf.int/forecasts/{time.ToString("yyyyMMdd")}/{run}z/ifs/0p25/oper/{time.ToString("yyyyMMdd")}{run}0000-{i}h-oper-fc.grib2";

                    url_list.Add(u);
                }

            }
                
            List<string> file_list = [];

            if (finalForecastHour <= 144)
            {
                for (int i = 0; i < (finalForecastHour + step); i += step)
                {
                    string f = $"{time.ToString("yyyyMMdd")}{run}0000-{i}h-oper-fc.grib2";
                    file_list.Add(f);
                }
            }
            else
            {
                for (int i = 0; i < (144 + step); i += step)
                {
                    string f = $"{time.ToString("yyyyMMdd")}{run}0000-{i}h-oper-fc.grib2";
                    file_list.Add(f);
                }

                for (int i = 144; i < (finalForecastHour + 6); i += 6)
                {
                    string f = $"{time.ToString("yyyyMMdd")}{run}0000-{i}h-oper-fc.grib2";
                    file_list.Add(f);
                }

            }

            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string folderName = $"Weather Data/ECMWF/IFS/";
            string displayFolderName = $@"Weather Data\ECMWF\IFS\";
            string fullPath = Path.Combine(directoryPath, folderName);
            string displayPath = Path.Combine(directoryPath, displayFolderName);

            List<string> paths = [];
            for (int i = 0; i < file_list.Count; i++)
            {
                string path = $"{fullPath}{file_list[i]}";
                paths.Add(path);
            }

            if (!Directory.Exists($"{fullPath}"))
            {
                Directory.CreateDirectory($"{fullPath}");
                Console.WriteLine($"Folder {displayPath}' created");
            }
            else
            {
                Console.WriteLine($"Folder '{displayPath}' already exists");
            }

            ClearFiles.DeleteFiles(fullPath, displayPath);

            for (int a = 0; a < url_list.Count; a++)
            {
                try
                {
                    await FileDownloader.DownloadFileAsync(url_list[a], paths[a]);
                }
                catch
                {
                    try
                    {
                        for (int r = 0; r < 6; r++)
                        {

                            Console.WriteLine($"Network Connection disrupted\nWaiting 30 seconds and reconnecting.\nRetries left: {5 - r}");
                            Thread.Sleep(30000);
                            await FileDownloader.DownloadFileAsync(url_list[a], paths[a]);
                            bool success = true;
                            if (success == true)
                            {
                                break;
                            }
                            else
                            {

                            }

                        }

                    }
                    catch
                    {
                        Console.WriteLine("Cannot reconnect. Exiting....");

                    }

                }

            }
        }

    }
}
