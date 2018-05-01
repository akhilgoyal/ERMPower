using System.Collections.Generic;
using ERMPower.Domain.Entity;

namespace ERMPower.Services
{
    public interface IFileProcessingService
    {
        void Import();
        double GetMedian();
        IEnumerable<EnergyModel> GetDataOverMedian();
        IEnumerable<EnergyModel> GetDataBelowMedian();
    }
}
