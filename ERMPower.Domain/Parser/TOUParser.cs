using System;
using ERMPower.Domain.Entity;

namespace ERMPower.Domain.Parser
{
    public class TOUParser : IParser
    {
        public EnergyModel ParseLine(string line)
        {
            var data = line.Split(',');
            return new EnergyModel()
            {
                MeterDateTime = DateTime.Parse(data[3]),
                MedianField = double.Parse(data[5])
            };
        }
    }
}
