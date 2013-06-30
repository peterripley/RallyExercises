using System;
using System.Text;

// Peter Ripley - June 2013
// Solution for Rally Software -Exercise 1-
namespace WriteNumber
{
    class Program
    {
        static private string[] _oneNames = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        static private string[] _tenNames = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        static private string[] _teenNames = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        static private string[] _placeValueNames = { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion" };

        static void Main()
        {
            Decimal dollarAmount = 0M;
            string input = null;
            int placeValueNameCount = _placeValueNames.Length;

            Console.WriteLine(String.Format("Please enter a dollar amount between 0.01 and 1 {0}.  Use 2 decimal places for the fractional amount.", _placeValueNames[placeValueNameCount - 1]));

            do // while (!String.IsNullOrEmpty(input))
            {
                Console.Write("\r\nAmount (or [Enter] to quit) ? ");
                input = Console.ReadLine();

                if (!String.IsNullOrEmpty(input))
                {
                    try
                    {
                        // Proceed if the string provided can be converted to a number, that number is between 0.01 and 1000 to the power of the number of _placeValueeNames[] elements inclusive, and has 2 decimal places. 
                        if (Decimal.TryParse(input.Replace("$", null).Replace(",", null), out dollarAmount) & dollarAmount >= 0.01M && dollarAmount <= (decimal)Math.Pow(1000, placeValueNameCount) && (dollarAmount % 1).ToString().Length == 4)
                        {
                            StringBuilder amountText = new StringBuilder();
                            string periodValueText = null;
                            int pennyAmount = Convert.ToInt32(dollarAmount % 1 * 100);

                             // Get and append the text for the period values for each element in the name array, down to thousands
                            for (int e = _placeValueNames.Length; e >= 1; e--)
                            {
                                periodValueText = GetPeriodValueText((long)(dollarAmount % (long)Math.Pow(1000, e + 1) / (long)Math.Pow(1000, e)));
                                if (!String.IsNullOrEmpty(periodValueText))
                                {
                                    amountText.Append(periodValueText).Append(" ").Append(_placeValueNames[e - 1]).Append(" ");
                                }
                            }
                            // Get and append the text for under a thousand, append the fractional amount, and capitalize the first character of our resultant string before displaying it.
                            periodValueText = GetPeriodValueText((long)(dollarAmount % 1000));
                            amountText.Append(periodValueText).Append(String.IsNullOrEmpty(periodValueText) ? null : " ");
                            amountText.Append(String.IsNullOrEmpty(amountText.ToString()) ? "zero " : null).Append("and ").Append(pennyAmount.ToString("00")).Append("/100 dollars")[0] = amountText[0].ToString().ToUpperInvariant()[0];

                            Console.WriteLine("\r\n" + amountText.ToString());
                        }
                        else
                        {
                            Console.WriteLine("\r\nThe value provided was either not a validly formatted dollar amount or was outside of the accepted range.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(String.Format("\r\nAn unexpected error occurred: {0}", ex.Message));
                    }
                }
            } while (!String.IsNullOrEmpty(input));
        }

        // The integer provided can be between 0 and 999.  0 returns an empty string.
        static string GetPeriodValueText(long PeriodValue)
        {
            StringBuilder valueText = new StringBuilder();
            long PeriodValue_Tens = PeriodValue % 100;
            long PeriodValue_Ones = PeriodValue % 10;
                                    
            if(PeriodValue < 0 || PeriodValue > 999)
            {
                throw new ArgumentOutOfRangeException("PeriodValue");
            }
            // Build the period value string starting with hundreds.
            if (PeriodValue > 99)
            {
                valueText.Append(_oneNames[(int)PeriodValue / 100 - 1]).Append(" hundred ");
            }
            // Do we use a a teen name (thirteen, fourteen, etc.)...
            if (PeriodValue_Tens > 10 && PeriodValue_Tens < 20)
            {
                valueText.Append(_teenNames[PeriodValue_Tens - 11]);
            }
            else
            {
                //...or a ten-multiple name (twenty, thirty, etc.)...
                if (PeriodValue_Tens >= 20)
                {
                    valueText.Append(_tenNames[(int)PeriodValue_Tens / 10 - 1]).Append((PeriodValue_Ones > 0 ? "-" : null));
                }
                // ...and add a digit name if needed.
                if (PeriodValue_Ones > 0)
                {
                    valueText.Append(_oneNames[PeriodValue_Ones - 1]);
                }
            }
            return valueText.ToString();
        }
    }
}
    


