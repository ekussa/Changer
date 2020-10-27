using System.Collections.Generic;
using System.Linq;

namespace Changer
{
    public class LimitedStorage : IStorage
    {
        private readonly Dictionary<decimal, int> _billsAndAmount;
        public List<decimal> Distinct => _billsAndAmount.Keys.ToList();

        public LimitedStorage(Dictionary<decimal, int> billsAndAmount)
        {
            _billsAndAmount = billsAndAmount;
        }
        
        public bool Contain(in decimal value)
        {
            var result = _billsAndAmount.ContainsKey(value);
            if (!result) return false;
            
            if (_billsAndAmount[value] == 0)
                return false;
            _billsAndAmount[value]--;
            return true;
        }
    }
}
