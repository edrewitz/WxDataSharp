﻿using System;
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
