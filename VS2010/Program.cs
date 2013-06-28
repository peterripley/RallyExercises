using System;
using System.Text;

namespace WriteNumber
{
    class Program
    {
        static private string[] _digitNames = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        static private string[] _tenNames = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        static private string[] _teenNames = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        static private string[] _placeValueNames = { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion" };

        static void Main(string[] args)
        {
            Decimal dollarAmount = 0M;
            string input = null;

            Console.WriteLine("Please enter a dollar amount between 0.00 and 1 quintillion.  Use 2 decimal places for the fractional amount.");

            do
            {
                Console.Write("\r\nAmount (or [Enter] to quit)? ");
                input = Console.ReadLine();

                if (!String.IsNullOrEmpty(input))
                {
                    try
                    {
                        // Proceed if the string provided can be converted to a number and that number is not greater than zero but not greater than 1 quintillion and has 2 decimal places. 
                        if (Decimal.TryParse(input.Replace("$", null).Replace(",", null), out dollarAmount) & dollarAmount >= 0 && dollarAmount <= 1000000000000000000.00M && (dollarAmount % 1).ToString().Length == 4)
                        {
                            StringBuilder amountText = new StringBuilder();
                            string periodValueText = null;
                            int pennyAmount = Convert.ToInt32(dollarAmount % 1 * 100);

                            // Loop for the period values from each element in the name array, down to thousands
                            for (int e = _placeValueNames.Length; e >= 1; e--)
                            {
                                periodValueText = GetPeriodValueText((long)(dollarAmount % (long)Math.Pow(1000, e + 1) / (long)Math.Pow(1000, e)));

                                if (!String.IsNullOrEmpty(periodValueText))
                                {
                                    amountText.Append(periodValueText).Append(" ").Append(_placeValueNames[e - 1]).Append(" ");
                                }
                            }
                            // Add the text for under the thousands period value, plus the fractional amount, and capitalize the first character 
                            periodValueText = GetPeriodValueText((long)(dollarAmount % 1000));

                            if (!String.IsNullOrEmpty(periodValueText))
                            {
                                amountText.Append(periodValueText).Append(" ");
                            }
                            (amountText.Append(amountText.ToString() == String.Empty ? "zero and " : "and ").Append(pennyAmount.ToString("00")).Append("/100 dollars")[0]) = amountText[0].ToString().ToUpperInvariant()[0];

                            Console.WriteLine("\r\n" + amountText.ToString());
                        }
                        else
                        {
                            Console.WriteLine("\r\nThe value provided was either not a valid dollar amount or was outside of the accepted range.");
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
                throw new Exception("Value provided was outside of the accepted range.");
            }

            // Build the period value string starting with hundreds.
            if (PeriodValue > 99)
            {
                valueText.Append(_digitNames[(int)PeriodValue / 100 - 1]).Append(" hundred ");
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
                    valueText.Append(_digitNames[PeriodValue_Ones - 1]);
                }
            }
            return valueText.ToString();
        }
    }
}
