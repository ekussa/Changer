using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.VisualBasic.CompilerServices;

namespace Changer
{
    public class Bills : Dictionary<decimal, int>
    {
        public decimal Total => this.Sum(_ => _.Key * _.Value);

        public Bills()
        {
        }

        public Bills(IEnumerable<decimal> values)
        {
            foreach (var value in values)
            {
                if (ContainsKey(value))
                    this[value]++;
                else
                    Add(value, 1);
            }
        }

        public static Bills operator+(Bills origin, decimal value)
        {
            if (origin.ContainsKey(value))
                origin[value]++;
            else
                origin.Add(value, 1);
            return origin;
        }
        
        public static Bills operator-(Bills origin, decimal value)
        {
            if (origin.ContainsKey(value) && origin[value] != 0)
                origin[value]--;
            else
                origin.Add(value, -1);
            return origin;
        }
    }
}
