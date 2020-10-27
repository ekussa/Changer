using System.Collections.Generic;
using System.Linq;

namespace Changer
{
    public class PrioritizedTopDownDispatcher : IDispatcher
    {
        private readonly IStorage _storage;
        private readonly decimal[] _priority;

        public PrioritizedTopDownDispatcher(
            IStorage storage,
            decimal[] priority)
        {
            _storage = storage;
            _priority = priority;
        }
        
        public Bills ForAmount(decimal value)
        {
            var result = new Bills();
            foreach (var bill in GetPrioritizedDistinctBills())
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

        private IEnumerable<decimal> GetPrioritizedDistinctBills()
        {
            var bills = _storage.
                Distinct.
                OrderByDescending(_ => _);

            var result = new List<decimal>(_priority);
            var selectedBills = bills.
                Where(bill => !result.Contains(bill));
            result.AddRange(selectedBills);
            return result;
        }
    }
}
