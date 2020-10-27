using System.Collections.Generic;
using System.Linq;

namespace Changer
{
    public class BottomUpDispatcher : IDispatcher
    {
        private readonly IStorage _storage;

        public BottomUpDispatcher(IStorage storage)
        {
            _storage = storage;
        }
        
        public Bills ForAmount(decimal value)
        {
            var result = new Bills();
            foreach (var bill in GetDistinctAscendingBills())
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

        private IEnumerable<decimal> GetDistinctAscendingBills()
        {
            return _storage.Distinct.OrderBy(_ => _);
        }
    }
}