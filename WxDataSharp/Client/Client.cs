/*
 * This file hosts the FileDownloader class which is the HTTP Client that downloads files from HTTPS URLs.
 * 
 * (C) Eric J. Drewitz 2025
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxDataSharp.Client
{

    public class FileDownloader
    {
        /*
         * This public class contains the following:
         * 
         * 1) Task DownloadFileAsync
         * 
         */
        public static async Task DownloadFileAsync(string fileUrl, string localFilePath)
        {

            /*
            HTTPS Client that downloads the data and saves the data to the folder.

            Required Arguments:

            1) string fileUrl - The URL of the data file

            2) string localFilePath - The local file path on the computer where the data will be stored. 
            */

            using HttpClient client = new();
            try
            {

                // Get the file stream from the URL
                using (Stream contentStream = await client.GetStreamAsync(fileUrl))
                {
                    // Create a FileStream to save the content to the local path
                    using (FileStream fileStream = new(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // Copy the content stream to the file stream
                        await contentStream.CopyToAsync(fileStream);
                    }
                }
                // Prints success message to the user
                Console.WriteLine($"File downloaded successfully to: {localFilePath}");
            }
            catch (HttpRequestException e)
            {
                // Prints HTTPS error to the user.
                Console.WriteLine($"Error downloading file: {e.Message}");
                Console.WriteLine("Waiting 30 seconds...");
                Thread.Sleep(30000);

                for (int i = 0; i < 6; i++)
                {
                    try
                    {
                        using Stream contentStream = await client.GetStreamAsync(fileUrl);
                        // Create a FileStream to save the content to the local path
                        using FileStream fileStream = new(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                        // Copy the content stream to the file stream
                        await contentStream.CopyToAsync(fileStream);
                        Console.WriteLine($"File downloaded successfully to: {localFilePath}");
                        break;
                    }
                    catch
                    {
                        // Prints success message to the user
                        Console.WriteLine("Unable to reconnect.\nIncreasing time between reconnect attempts to 60 seconds...");
                        Console.WriteLine($"Retries Remaining: {5 - i}");
                        Thread.Sleep(60000);
                    }

                }

            }
            catch (Exception e)
            {
                // Prints error to the user. 
                Console.WriteLine($"An unexpected error occurred: {e.Message}");
            }
        }
    }
    
}
