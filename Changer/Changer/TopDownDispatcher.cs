using System.Collections.Generic;
using System.Linq;

namespace Changer
{
    public class TopDownDispatcher : IDispatcher
    {
        private readonly IStorage _storage;

        public TopDownDispatcher(IStorage storage)
        {
            _storage = storage;
        }
        
        public Bills ForAmount(decimal value)
        {
            var result = new Bills();
            foreach (var bill in GetDistinctDescendingBills())
            {
                while (value >= bill)
                {
                    if (!_storage.Contain(bill))
                        break;
                    value -= bill;
                    result += bill;
                }
            }
            return result;
        }

        private IEnumerable<decimal> GetDistinctDescendingBills()
        {
            return _storage.
                Distinct.
                OrderByDescending(_ => _);
        }
    }
}
