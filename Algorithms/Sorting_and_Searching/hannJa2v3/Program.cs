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

        //array of name strings
        static string[] nam = new string[20] { "wendy", "ellen", "freddy", "tom", "susan",
                             "dick", "harry", "aloysius", "zelda", "sammy",
                             "mary", "hortense", "georgie", "ada", "daisy",
                             "paula", "alexander", "louis", "fiona", "bessie"  };

        //array of weights corresponding to these names
        static int[] wght = new int[20] { 120, 115, 195, 235, 138, 177, 163, 150, 128, 142,
                       118, 134, 255, 140, 121, 108, 170, 225, 132, 148 };

        static void Main(string[] args)
        {
            string[] WKnam = new string[LSIZE];
            int[] WKwght = new int[LSIZE];

            OutLists("UNSORTED ARRAY DATA", "NAME", "WEIGHT");

            char choice;
            bool isSorted = false;

            PutMenu();
            choice = GetChoice(isSorted);
    
            while (choice != '4' || !isSorted)
            {
                if(choice < '4')
                {
                    ChooseSort(choice, ref WKnam, ref WKwght, ref isSorted);
                }
                Console.WriteLine();
                PutMenu();
                choice = GetChoice(isSorted);
            }
            SearchSection();
        }

        static void OutLists(string title, string col1, string col2)
        {
            Console.WriteLine(title.PadLeft(30));
            Console.WriteLine();

            Console.Write(col1.PadLeft(15));
            Console.WriteLine(col2.PadLeft(15));
            Console.WriteLine("=======================================");

            for (int i = 0; i < LSIZE; i++)
            {
                Console.WriteLine(nam[i].PadLeft(15) + wght[i].ToString().PadLeft(15));
            }
            Console.WriteLine("=======================================");
            Console.WriteLine();
            Console.WriteLine();

            Console.ReadLine();
        }

        static void CopyLists(ref string[] WKnam, ref int[] WKwght)
        {
            WKnam = nam;
            WKwght = wght;
        }

        static void PutMenu()
        {
            Console.WriteLine("Menu");
            Console.WriteLine("1. Insertion Sort");
            Console.WriteLine("2. Selecion Sort");
            Console.WriteLine("3. Shell Sort");
            Console.WriteLine("4. Search");
        }

        static char GetChoice(bool isSorted)
        {
            char choice;

            Console.Write("Choice:  ");

            choice = Console.ReadKey(false).KeyChar;

            while (choice < '1' || choice > '4' || (choice == '4' && !isSorted))
            {
                if (!isSorted && choice == '4')
                {
                    Console.WriteLine("   Error: You must sort at least once");
                }
                else
                {
                    Console.WriteLine("   Error: Only valid choices are: 1, 2, 3 and 4");
                }
                Console.Write("Choice:  ");
                choice = Console.ReadKey(false).KeyChar;
                
            }

            return choice;
        }

        static void ChooseSort(char choice, ref string[] WKnam, ref int[] WKwght, ref bool isSorted)
        {
            CopyLists(ref WKnam, ref WKwght);

            Console.WriteLine("");

            switch(choice)
            {
                case '1':
                    Insertion();
                    break;
                case '2':
                    Selection();
                    break;
                case '3':
                    Shell();
                    break;
            }
            isSorted = true;
            
        }

        static void Insertion()
        {
            Console.WriteLine("Insert");
            Console.ReadKey();
        }

        static void Selection()
        {
            Console.WriteLine("Selection");
            Console.ReadKey();
        }

        static void Shell()
        {
            Console.WriteLine("Shell");
            Console.ReadKey();
        }

        static void SearchSection()
        {
            Console.Clear();
            Console.WriteLine("Welcome To The Search Section");
            Console.ReadKey();
        }
    }
}
