using System;
using System.Text.RegularExpressions;

namespace Check
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string nodeId = "A200_1";

            int numOfChars = nodeId.Length;
            var charsArray = nodeId.ToCharArray();
            for(int i = charsArray.Length; i > 0; i--)
            {
                //if(Int32.TryParse(charsArray[i]))
            }

            string numberPartOfId = Regex.Match(nodeId, @"\d+").Value;
            Console.WriteLine(numberPartOfId);

            string someNumber = "2";
            Console.WriteLine(Double.Parse(someNumber).GetType());
            Console.ReadKey();
        }
    }
}
