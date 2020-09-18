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

            OutLists("UNSORTED ARRAY DATA", "NAME", "WEIGHT", nam, wght);

            char choice;
            bool isSorted = false;

            PutMenu();
            choice = GetChoice(isSorted);

            while (choice != '4' || !isSorted)
            {
                if (choice < '4')
                {
                    ChooseSort(choice, ref WKnam, ref WKwght, ref isSorted);
                }
                PutMenu();
                choice = GetChoice(isSorted);
            }
            SearchSection();
        }

        static void OutLists(string title, string col1, string col2, string[] WKnam, int[] WKwght)
        {
            Console.WriteLine(title.PadLeft(30));
            Console.WriteLine();

            Console.Write(col1.PadLeft(15));
            Console.WriteLine(col2.PadLeft(15));
            Console.WriteLine("=======================================");

            for (int i = 0; i < LSIZE; i++)
            {
                Console.WriteLine(WKnam[i].PadLeft(15) + WKwght[i].ToString().PadLeft(15));
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

            switch (choice)
            {
                case '1':
                    Insertion(WKnam, WKwght);
                    break;
                case '2':
                    Selection(WKnam, WKwght);
                    break;
                case '3':
                    Shell(WKnam, WKwght);
                    break;
            }
            isSorted = true;

        }

        static void Insertion(string[] WKnam, int[] WKwght)
        {
            int k = 1;
            int n = WKnam.Length;
            string y;
            int i, yw;
            bool found;

            do
            {
                yw = WKwght[k];
                y = WKnam[k];
                i = k - 1;
                found = false;

                while (i >= 0 && !found)
                {
                    if (y.CompareTo(WKnam[i]) < 0)
                    {
                        WKwght[i + 1] = WKwght[i];
                        WKnam[i + 1] = WKnam[i];
                        i = i - 1;
                    }
                    else
                    {
                        found = true;
                    }
                }
                WKnam[i + 1] = y;
                WKwght[i + 1] = yw;
                k = k + 1;
            } while (k <= n - 1);

            OutLists("INSERTION SORT OUTPUT", "NAME", "WEIGHT", WKnam, WKwght);
            Console.ReadKey();
        }

        static void Selection(string[] WKnam, int[] WKwght)
        {
            int bigW, i, where, j;
            string bigN;
            int n = LSIZE;

            i = n - 1;

            do
            {
                bigN = WKnam[0];
                bigW = WKwght[0];
                where = 0;
                j = 1;
                do
                {
                    if(WKnam[j].CompareTo(bigN) > 0)
                    {
                        bigN = WKnam[j];
                        bigW = WKwght[j];
                        where = j;
                    }
                    j = j + 1;
                } while (j <= i);
                WKnam[where] = WKnam[i];
                WKwght[where] = WKwght[i];
                WKnam[i] = bigN;
                WKwght[i] = bigW;
                i = i - 1;
            } while (i > 0);

            OutLists("SELECTION SORT OUTPUT", "NAME", "WEIGHT", WKnam, WKwght);

            Console.ReadKey();
        }

        static void Shell(string[] WKnam, int[] WKwght)
        {
            int[] gaplist = new int[LSIZE];
            int i, gap, j, w, k, numgaps;
            int n = LSIZE;
            string y;
            bool found;

            numgaps = GetGaps(gaplist);

            i = numgaps - 1;

            do
            {
                gap = gaplist[i];
                j = gap;
                do
                {
                    y = WKnam[j];
                    w = WKwght[j];
                    k = j - gap;
                    found = false;
                    while(k>=0 && !found)
                    {
                        if(y.CompareTo(WKnam[k]) < 0)
                        {
                            WKnam[k + gap] = WKnam[k];
                            WKwght[k + gap] = WKwght[k];
                            k = k - gap;
                        }
                        else
                        {
                            found = true;
                        }
                    }
                    WKnam[k + gap] = y;
                    WKwght[k + gap] = w;
                    j = j + 1;
                } while (j <= n - 1);
                i = i - 1;
            } while (i >= 0);

            OutLists("SHELL SORT OUTPUT", "NAME", "WEIGHT", WKnam, WKwght);

            Console.ReadKey();
        }

        static void SearchSection()
        {
            Console.Clear();
            Console.WriteLine("Welcome To The Search Section");
            Console.ReadKey();
        }

        static int GetGaps(int[] g)
        {
            int low = 0, high = LSIZE - 1;
            int gap = 1;
            int pos = 0;

            while(gap<(high-low)+1)
            {
                g[pos] = gap;
                gap = gap * 3;
                pos = pos + 1;
            }
           
            return pos;
        }
    }
}
