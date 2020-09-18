using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hannJa3v2
{
    class Program
    {
        const int NMAX = 10;          //maximum size of each name string
        const int LSIZE = 20;         //number of actual name strings in array

        static void Main(string[] args)
        {
            //array of name strings
            string[] nam = new string[20] { "wendy", "ellen", "freddy", "tom", "susan",
                             "dick", "harry", "aloysius", "zelda", "sammy",
                             "mary", "hortense", "georgie", "ada", "daisy",
                             "paula", "alexander", "louis", "fiona", "bessie"  };

            //array of weights corresponding to these names
            int[] wght = new int[20] { 120, 115, 195, 235, 138, 177, 163, 150, 128, 142,
                       118, 134, 255, 140, 121, 108, 170, 225, 132, 148 };

            string tableTitle = "UNSORTED ARRAY DATA";
            string headerCol1 = "NAME";
            string headerCol2 = "WEIGHT";

            OutLists(nam, wght, ref tableTitle, ref headerCol1, ref headerCol2);
        }

        static void OutLists(string[] n, int[] w, ref string title, ref string col1, ref string col2)
        {
            Console.WriteLine(title.PadLeft(30));
            Console.WriteLine();

            Console.Write(col1.PadLeft(15));
            Console.WriteLine(col2.PadLeft(15));
            Console.WriteLine("=======================================");

            for (int i = 0; i < LSIZE; i++)
            {
                Console.WriteLine(n[i].PadLeft(15) + w[i].ToString().PadLeft(15));
            }
            Console.WriteLine("=======================================");
            Console.WriteLine();
            Console.WriteLine();

            Console.ReadLine();
        }
        static void CopyLists()
        {
        }
    }
}
