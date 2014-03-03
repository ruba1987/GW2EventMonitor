using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET
{
    internal static class MathHelpers
    {
        /// <summary>
        /// Compares two doubles and determines if they are equal based on the number of double deviations (<paramref name="units"/>) allowed.
        /// <remarks>
        /// <code>
        /// For Exmaple:
        ///   double value1 = 0.1d;
        ///   double value2 = 1.0999999999999999d;
        ///   double value3 = 0.10000000000000003d;
        /// value2 is exactlly one possible double value from value1.  In otherwords there is no double value that can exists between value1 and value2. value2 is the next closest value greater than value1.
        /// value3 is exactlly two possible double values from value 1.  value2 is the only possible double representation between value1 and value3. value3 is the second closest value greater than value1.
        /// </code></remarks>
        /// </summary>
        /// <param name="value1">first value</param>
        /// <param name="value2">seconds value</param>
        /// <param name="units">number of possible double value deviations between <paramref name="value1"/> and <paramref name="value2"/></param>
        /// <returns>returns true if the number of possible double value deviations between <paramref name="value1"/> and <paramref name="value2"/> are less than or equal to <paramref name="units"/></returns>
        public static bool HasMinimalDifference(this double value1, double value2, int units)
        {
            long lValue1 = BitConverter.DoubleToInt64Bits(value1);
            long lValue2 = BitConverter.DoubleToInt64Bits(value2);

            // If the signs are different, return false except for +0 and -0. 
            if ((lValue1 >> 63) != (lValue2 >> 63))
            {
                if (value1 == value2)
                    return true;

                return false;
            }

            long diff = Math.Abs(lValue1 - lValue2);
            if (diff <= (long)units)
                return true;

            return false;
        }
    }
}
