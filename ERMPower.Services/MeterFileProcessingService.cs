using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ERMPower.Domain.Entity;
using ERMPower.Domain.Parser;

namespace ERMPower.Services
{
    public class MeterFileProcessingService : IFileProcessingService
    {
        private IParser _parser;
        private string _filePath;
        private List<EnergyModel> energyModels;
        private double? currentMedian = null;
        
        public MeterFileProcessingService(IParser parser,string filePath)
        {
            _parser = parser;
            _filePath = filePath;
            energyModels = new List<EnergyModel>();
        }

        public void Import()
        {
            var firstLine = true;
            using (FileStream fs = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                        
                    }
                    var model = _parser.ParseLine(line);
                    energyModels.Add(model);
                }
            }
        }

        public double GetMedian()
        {
            if (currentMedian != null) return (double) currentMedian;
            var temp = energyModels.Select(x => x.MedianField).ToArray();
            Array.Sort(temp);
            var count = temp.Length;
            if (count == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            if (count % 2 != 0) return (double) (currentMedian = temp[count / 2]);
            // count is even, average two middle elements
            var a = temp[count / 2 - 1];
            var b = temp[count / 2];
            return (double) (currentMedian = ((a + b) / 2));
        }

        public IEnumerable<EnergyModel> GetDataOverMedian()
        {
            var medianValue = GetMedian();
            
            var overValue = (medianValue * 0.20) + medianValue;
            var listOverMedian = energyModels.Where(x => x.MedianField > overValue).ToList();

            return listOverMedian;
        }

        public IEnumerable<EnergyModel> GetDataBelowMedian()
        {
            var medianValue = GetMedian();
            var belowValue = medianValue - (0.20 * medianValue) ;
            var listBelowMedian = energyModels.Where(x => x.MedianField < belowValue).ToList();

            return listBelowMedian;
        }
    }
}
