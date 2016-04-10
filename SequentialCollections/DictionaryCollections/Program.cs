using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DictionaryCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable lookup = new Hashtable();
            lookup["0"] = "Zero";
            lookup["1"] = "One";
            lookup["2"] = "Two";
            lookup["3"] = "Three";
            lookup["4"] = "Four";

            string ourNumber = "333-555-1212";
            foreach (char character in ourNumber)
            {
                string digit = character.ToString();
                if(lookup.Contains(digit))
                {
                    Console.WriteLine(lookup[digit]);
                }
            }
            Console.ReadLine();
        }
    }
}
