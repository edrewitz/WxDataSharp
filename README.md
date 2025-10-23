# WxDataSharp

![weather icon](https://github.com/edrewitz/WxDataSharp/blob/master/icons/weather%20icon%20(3).jpg) ![csharp icon](https://github.com/edrewitz/WxDataSharp/blob/master/icons/download.png)

A C# library that will help automate the downloading of weather data. 

This library supplements my WxData Python library in C#. 
This library has functions that do the following:

1) Download weather data from various data sources.
   - Ensures handling of interrupted connections to prevent partial data loss.

2) Automatically builds the directories local on your computer.

3) Scans the files to ensure they are up to date.

4) Deletes old data and downloads new data after new data comes in. 
