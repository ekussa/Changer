using System.Collections.Generic;

namespace Changer
{
    public interface IStorage
    {
        List<decimal> Distinct { get; }
        bool Contain(in decimal value);
    }
}
