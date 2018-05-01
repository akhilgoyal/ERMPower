using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using ERMPower.Domain.Entity;
using ERMPower.Domain.Parser;
using ERMPower.Services;
using Unity;
using Unity.Resolution;

namespace ERMPower.ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dir = ConfigurationManager.AppSettings.Get("MeterFilePath");
            //Unity IOC to register types
            var container = UnityConfig.GetConfiguredContainer();

            var files = Directory.GetFiles(dir);
            foreach (var file in files)
            {
                try
                {
                    //using the file prefix to differentiate the files.
                    var fileName = Path.GetFileName(file);
                    var fileType = fileName.Split('_')[0];

                    // New file type can be parsed only by adding a new implementation of IParser 
                    //and by registering the new parser type in the unity container
                    var fileProcessor = container.Resolve<IFileProcessingService>
                     (new ParameterOverride("parser", container.Resolve<IParser>(fileType.ToUpper())),
                         new ParameterOverride("filePath", file));

                    fileProcessor.Import();
                    var currentMedian = fileProcessor.GetMedian();
                    var listAboveMedian = fileProcessor.GetDataOverMedian();
                    Console.WriteLine("Values 20% above the Median for file name  " + fileName);
                    PrintData(fileName, currentMedian, listAboveMedian);
                    var listBelowMedian = fileProcessor.GetDataBelowMedian();
                    Console.WriteLine("======================================================================================");
                    Console.WriteLine("Values 20% below the Median for file name  " + fileName);
                    PrintData(fileName, currentMedian, listBelowMedian);
                    Console.WriteLine("======================================================================================");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


            }
            Console.ReadLine();

        }

        private static void PrintData(string fileName, double currentMedian, IEnumerable<EnergyModel> dataList)
        {
            Console.WriteLine(string.Format("{0,-25} {1,30} {2,10} {3,-60}", "File Name", "DateTime", "Value", "Median Value"));
            foreach (var model in dataList)
            {
                Console.WriteLine(string.Format("{0,25} {1,25} {2,5} {3,5}", fileName, model.MeterDateTime.ToString(), model.MedianField, currentMedian));
            }
        }
    }
}
