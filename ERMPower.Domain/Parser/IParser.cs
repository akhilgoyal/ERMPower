using ERMPower.Domain.Entity;

namespace ERMPower.Domain.Parser
{
    public interface IParser
    {
        EnergyModel ParseLine(string line);
    }
}
