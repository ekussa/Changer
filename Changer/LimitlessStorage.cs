using System.Collections.Generic;
using System.Linq;

namespace Changer
{
    public class LimitlessStorage : IStorage
    {
        private readonly Bills _bills;

        public List<decimal> Distinct => _bills.Keys.ToList();
        
        public LimitlessStorage(Bills bills)
        {
            _bills = bills;
        }

        public bool Contain(in decimal value)
        {
            return _bills.ContainsKey(value);
        }
    }
}
