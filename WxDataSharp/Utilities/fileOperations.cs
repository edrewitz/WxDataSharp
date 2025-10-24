/*
 * This file hosts the class that manage a local file directory. 
 * 
 * (C) Eric J. Drewitz 2025
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxDataSharp.Utilities
{
    public static class ClearFiles
    {
        /*
        This class clears the contents of a directory when downloading new data.
        */

        public static void DeleteFiles(string fullPath, string displayPath)
        {
            /*
             * This function deletes old data files in a directory prior to downloading new data.
             * 
             * Required Arguments:
             * 
             * 1) string fullPath - The full system path to the directory containing the files to be deleted.
             * 
             * 2) string displayPath - A user-friendly path representation for logging purposes.
             * 
             * Returns
             * -------
             * 
             * Deletes all contents in a specified directory. 
             */

            if (Directory.Exists(fullPath))
            {
                string[] filePaths = Directory.GetFiles(fullPath);
                foreach (string filePath in filePaths)
                {
                    try
                    {
                        File.Delete(filePath);
                        Console.WriteLine($"Deleted: {displayPath}");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Error deleting {filePath}: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Directory not found: {displayPath}");
            }
        }
    }
}
