using System;
using System.Collections.Generic;
using System.Text;

namespace Furaha.Libraries.XeroSdk.MoneyComputing
{
    /// <summary>
    /// This is here to allow dependancy injection.
    /// Feel free to extend.
    /// </summary>
    public interface IMoneyMath
    {
        decimal Round(decimal input, double decimalPlaces);

        decimal Truncate(decimal input, double decimalPlaces);
    }

    public class MoneyMath : IMoneyMath
    {
        /// <summary>
        /// Safely rounds an input value to the number of desired decimal places
        /// </summary>
        /// <param name="input">The input value</param>
        /// <param name="decimalPlaces">The maximum number of decimal places the rounded value should have</param>
        public decimal Round(decimal input, double decimalPlaces)
        {
            return TrimDecimals(input, Math.Round, decimalPlaces);
        }

        /// <summary>
        /// Safely truncates an input value to the number of desired decimal places
        /// </summary>
        /// <param name="input">The input value</param>
        /// <param name="decimalPlaces">The maximum number of decimal places the truncated value should have</param>
        public decimal Truncate(decimal input, double decimalPlaces)
        {
            return TrimDecimals(input, Math.Truncate, decimalPlaces);
        }

        /// <summary>
        /// Ensures that values after the decimal point are not unintentionally lost when performing mathematical operations on floating point numbers.
        /// Does this by first shifting the required number of decimal digits to the left of the decimal place, then performs the math operation, then shifts back the decimal digits to their original place values.
        /// </summary>
        /// <param name="input">The value on which the math operation is to be performed</param>
        /// <param name="trimmer">The math operation function</param>
        /// <param name="decimalPlaces">The maximum number of decimal places the result of the operation should have</param>
        private decimal TrimDecimals(decimal input, Func<double, double> trimmer, double decimalPlaces)
        {
            if (decimalPlaces < 0)
            {
                throw new Exception($"Cannot adjust value to {decimalPlaces} decimal places. Decimal places must be 0 or more");
            }
            var multiplier = Math.Pow(10, decimalPlaces);
            var bigValue = (double)input * multiplier;
            var trimmedValue = trimmer(bigValue);
            var finalResult = (decimal)(trimmedValue / multiplier);
            return finalResult;
        }
    }
}