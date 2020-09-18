/****************************************************ID BLOCK******************************************************
 * 
 * Due Date:                October 17th, 2019
 * Software Designer:       Jennifer Hann
 * Course:                  420-306-AB (Fall 2019)
 * Deliverable:             Assignment #3 --- Sorting And Searching
 * 
 * Description:             This program takes in two unsorted parrallel arrays named:
 *                                  nam     a string array containing a list of names,
 *                                  wght    a integer array containing a list of weight corresponding to the names.
 *                          The program first display the array of names and weight in a table in an unsorted 
 *                          fashion.
 *                          Afterwards, a menu is shown, asking th user to out of 3 sort algorithms.
 *                          The user may choose to sort again or continue to the searching option.
 *                          There must be at least one sorting done before being able to search.
 *                          
 *                          The sort algorithms are: Insertion Sort, Selection Sort, Shell Sort.
 *                          Before sorting, the arrays are copied into a work array called WKnam and WKwght.
 *                          On each sort, the sorting will be done on an unsorted arrays.
 *                          Once sorted, the the sorted data will be displayed.
 *                          
 *                          After sorting, using binary search, the user can search a name from the sorted names.
 *                          If a the name is found, the name, the weight and the position will be display.
 *                          if the name is not found, an appropriate message will be displayed.
 *                          Once an empty line is entered by the user, the program will end.
 *                          
 *****************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hannJa3v2
{
    class Program
    {   /*************************************** GLOBAL VARIABLES **********************************/

        const int NMAX = 10;          //maximum size of each name string
        const int LSIZE = 20;         //maximum size the arrays

        //array of name strings
        static string[] nam = new string[20] { "wendy", "ellen", "freddy", "tom", "susan",
                             "dick", "harry", "aloysius", "zelda", "sammy",
                             "mary", "hortense", "georgie", "ada", "daisy",
                             "paula", "alexander", "louis", "fiona", "bessie"  };

        //array of weights corresponding to these names
        static int[] wght = new int[20] { 120, 115, 195, 235, 138, 177, 163, 150, 128, 142,
                       118, 134, 255, 140, 121, 108, 170, 225, 132, 148 };

        /*******************************************************************************************/


        /************************************************* MAIN BLOCK *******************************************/

        static void Main(string[] args)
        {   string[] WKnam = new string[LSIZE];                                  //a work array of the array nam
            int[] WKwght = new int[LSIZE];                                       //a work array of the array wght

            OutLists("UNSORTED ARRAY DATA", "NAME", "WEIGHT", nam, wght);        //print table of unsorted data

            char choice;                                                         //user input for the menu section
            bool isSorted = false;                                               //arrays are sorted switch

            /************************************ SORT LOOP ***********************************/

            PutMenu();                                                           //print the menu
            choice = GetChoice(isSorted);                                        //get valid user's input

            while (choice != '4' || !isSorted)                                   //choice is 1, 2 or 3 OR arrays 
                                                                                 //not sorted
            {   if (choice < '4')                                                //choice is 1, 2 or 3
                    ChooseSort(choice, ref WKnam, ref WKwght, ref isSorted);     //choose sort and array is sorted
                PutMenu();                                                       //print the menu 
                choice = GetChoice(isSorted);                                    //get valid user's input 
            }

            /*********************************************************************************/

            SearchSection(WKnam, WKwght);                                        //user can search a name
        }

        /*********************************************************************************************************/


        /****************************** PRINT LIST OF NAMES AND LIST OF WEIGHT ***********************************/

        static void OutLists(string title, string col1, string col2, string[] WKnam, int[] WKwght)           
        {   Console.WriteLine(title.PadLeft(30));                            //print title
            Console.WriteLine();

            Console.Write(col1.PadLeft(15));                                 //print the heading for the name
            Console.WriteLine(col2.PadLeft(15));                             //print the heading for the weight
            Console.WriteLine("=======================================");

            for (int i = 0; i < LSIZE; i++)                                  //loop through the arrays    
            {
                Console.WriteLine(WKnam[i].PadLeft(15)                       //print the array of names and weight
                    + WKwght[i].ToString().PadLeft(15));        
            }
            Console.WriteLine("=======================================");
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadLine();                                              //wait for the user to continue
        }

        /*********************************************************************************************************/


        /********************* COPY THE LIST OF NAMES AND WEIGHT ********************/

        static void CopyLists(ref string[] WKnam, ref int[] WKwght)
        {   WKnam = nam;                                //copy the global array of names to a work array
            WKwght = wght;                              //copy the global array of weight to a work array
        }

        /****************************************************************************/


        /************************* PRINT MENU **********************/

        static void PutMenu()
        {   Console.WriteLine("Menu");                  //print the choice the user can choose from
            Console.WriteLine("1. Insertion Sort");
            Console.WriteLine("2. Selecion Sort");
            Console.WriteLine("3. Shell Sort");
            Console.WriteLine("4. Search");
        }

        /**********************************************************/


        /*********************************** VALIDATE USER'S INPUT ***********************************/

        static char GetChoice(bool isSorted)
        {   char choice;                                                             //user choice from the menu

            Console.Write("Choice:  ");                                              //ask the user choice
            choice = Console.ReadKey(false).KeyChar;                                 //get the user's decision

            while (choice < '1' || choice > '4' || (choice == '4' && !isSorted))     //choice is smaller than 1 OR 
                                                                                     //choice is bigger than 4 
                                                                                     //OR choice is 4 
                                                                                     //OR array not sorted
            {   if (!isSorted && choice == '4')                                      //arrays have not been sorted 
                                                                                     //AND choice is 4
                    Console.WriteLine("   Error: " +
                        "You must sort at least once");      //array have not been sorted
                else
                    Console.WriteLine("   Error: Only valid choices are: 1, 2, 3 and 4");    //input out of range
                Console.Write("Choice:  ");                                                  //ask user input
                choice = Console.ReadKey(false).KeyChar;                                     //get user decision
            }
            return choice;                                                                   //return user input
        }

        /********************************************************************************************/


        /************************** COPY AND SWITCH-CASE-BREAK BLOCK ********************************/

        static void ChooseSort(char choice, ref string[] WKnam, ref int[] WKwght, ref bool isSorted)
        {   CopyLists(ref WKnam, ref WKwght);                           //copy original arrays into works arrays
            Console.WriteLine("");                                      //skip a line in the console

            switch (choice)                                             //get the appropriate sort
            {   case '1':                                               //user chose 1
                    Insertion(WKnam, WKwght);                           //sort work arrays via insertion sort
                    break;                                              //exit the switch case
                case '2':                                               //user chose 2
                    Selection(WKnam, WKwght);                           //sort work arrays via selection sort
                    break;                                              //exit the switch case
                case '3':                                               //user chose 3
                    Shell(WKnam, WKwght);                               //sort work array via shell sort
                    break;                                              //exit the switch case
            }
            isSorted = true;                                            //the arrays have been sorted
        }

        /*******************************************************************************************************/


        /********************************** INSERTION SORT ************************************/

        static void Insertion(string[] WKnam, int[] WKwght)
        {   int k = 1;                          
            int n = WKnam.Length;                
            string y;                          
            int i;                              
            int yw;                            
            bool found;                         

            do
            {   yw = WKwght[k];                 
                y = WKnam[k];                    
                i = k - 1;                       
                found = false;                  

                while (i >= 0 && !found)         
                {   if (y.CompareTo(WKnam[i]) < 0)           
                    {   WKwght[i + 1] = WKwght[i];          
                        WKnam[i + 1] = WKnam[i];            
                        i = i - 1;                           
                    }
                    else
                        found = true;                        
                }
                WKnam[i + 1] = y;                         
                WKwght[i + 1] = yw;                         
                k = k + 1;                                  
            } while (k <= n - 1);                            
            Console.Clear();                                                      //erase the menu and data
            OutLists("INSERTION SORT OUTPUT", "NAME", "WEIGHT", WKnam, WKwght);   //print the sorted table of data
        }

        /**************************************************************************************/


        /*********************************** SELECTION SORT ***********************************/

        static void Selection(string[] WKnam, int[] WKwght)
        {   int bigW;                              
            int i;                                
            int where;                            
            int j;                               
            string bigN;                        
            int n = LSIZE;                        
            i = n - 1;                             

            do
            {   bigN = WKnam[0];                   
                bigW = WKwght[0];                  
                where = 0;                         
                j = 1;                            
                do
                {   if (WKnam[j].CompareTo(bigN) > 0)            
                    {   bigN = WKnam[j];                         
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
            Console.Clear();                                                      //erase the menu and data
            OutLists("SELECTION SORT OUTPUT", "NAME", "WEIGHT", WKnam, WKwght);   //print the sorted table of data
        }

        /**************************************************************************************/


        /**************************************** SHELL SORT **********************************/

        static void Shell(string[] WKnam, int[] WKwght)
        {   int[] gaplist = new int[LSIZE];                     
            int i;                                               
            int gap;                                            
            int j;                                              
            int w;                                               
            int k;                                             
            int numgaps;                                        
            int n = LSIZE;                                     
            string y;                                           
            bool found;                                          

            numgaps = GetGaps(gaplist);                         
            i = numgaps - 1;                                    

            do 
            {   gap = gaplist[i];                                
                j = gap;                                         
                do
                {   y = WKnam[j];                                
                    w = WKwght[j];                             
                    k = j - gap;                                 
                    found = false;                              
                    while (k >= 0 && !found)                     
                    {   if (y.CompareTo(WKnam[k]) < 0)           
                        {   WKnam[k + gap] = WKnam[k];           
                            WKwght[k + gap] = WKwght[k];         
                            k = k - gap;                         
                        }
                        else
                            found = true;                        
                    }
                    WKnam[k + gap] = y;                         
                    WKwght[k + gap] = w;                         
                    j = j + 1;                                  
                } while (j <= n - 1);                           
                i = i - 1;                                      
            } while (i >= 0);                                  
            Console.Clear();                                                     //erase the menu and data
            OutLists("SHELL SORT OUTPUT", "NAME", "WEIGHT", WKnam, WKwght);      //print the sorted table of data
        }

        /**************************************************************************************/


        /*************** GENERATE GAPLISTS ****************/

        static int GetGaps(int[] g)
        {   int low = 0;                          
            int high = LSIZE - 1;                   
            int gap = 1;                       
            int pos = 0;                       

            while (gap < (high - low) + 1)         
            {   g[pos] = gap;                     
                gap = gap * 3;                   
                pos = pos + 1;               
            }
            return pos;                          
        }

        /*************************************************/


        /************************************* SEARCH LOOP ************************************/

        static void SearchSection(string[] WKnam, int[] WKwght)
        {   Console.Clear();                                                         //erase the data
            Console.WriteLine("Welcome To The Search Section");                      //print title of the section
            Console.WriteLine();                                                     //skip a line in the console 

            bool found = false;                                                      //search name is found switch
            bool exit = false;                                                       //end the program switch
            string xnam = string.Empty;                                              //user's string to search 
            int xwght = 0;                                                           //weight of the search name
            int position = 0;                                                        //index of the found name

            while (exit == false)                                                    //while user want to search
            {   GetName(ref xnam);                                                   //get user input
                if (xnam == "")                                                      //user entered nothing
                {   break;                                                           //break out of the loop 
                                                                                     //and end the program
                }
                found = Bsrch(WKnam, WKwght, ref xnam, ref xwght, ref position);     //search name, weight 
                                                                                     //and position 
                FoundMessage(found, xnam, xwght, position);                          //display appropriate message
            }
        }

        /**************************************************************************************/

        /*************************** GET NAME FROM USER ***************************/

        static void GetName(ref string xnam)
        {   Console.Write("Enter name to search for weight: ");               //ask for the name from the user
            xnam = Console.ReadLine();                                        //get the user input
        }

        /**************************************************************************/


        /******************************************** BINARY SEARCH ******************************************/

        static bool Bsrch(string[] WKnam, int[] WKwght, ref string a, ref int b, ref int c)
        {   int bsrch;                                  //position of the name
            int low;                                    //lower limit of the index in the list of names
            int high;                                   //higher limit of the index in the list of names
            int mid;                                    //middle division between the lower and hight limit
            bool found = false;                         //name has been found switch

            bsrch = -1;                                 //define bsrch
            low = 0;                                    //first lower limit is the first index of the array
            high = LSIZE - 1;                           //first higher limit is the last index of the array

            while (low<=high)                           //lower limit doesn't exceed higher limit
            {   mid = (low + high) / 2;                 //get the middle of the two limits
                if (a == WKnam[mid])                    //name is the middle element in the array of names
                {   bsrch = mid;                        //save the position of the found name
                    low = LSIZE;                        //force exit of the loop
                    found = true;                       //name entered by the user is found
                }
                else if (a.CompareTo(WKnam[mid])<0)     //name is before the name found at the middle of the array
                {   high = mid - 1;                     //mid become the higher limit
                    found = false;                      //name has not been found
                }
                else
                {   low = mid + 1;                      //mid become the lower limit 
                    found = false;                      //name has not been found
                }
            }
            if (found == true)                          //name has been found
            {   a = WKnam[bsrch];                       //save the name from the array of names
                b = WKwght[bsrch];                      //save the weight from the array of weight
                c = bsrch;                              //save the index of the found name
            }
            return found;                               //found is true when name is found
                                                        //found is false when name was not found
        }

        /*****************************************************************************************************/

        /***************************** PRINT THE MESSAGE FOLLOWING THE BINARY SEARCH *****************************/

        static void FoundMessage(bool found, string name, int weight, int position)
        {   if (found == true)                                //name has been found following the search
            Console.WriteLine("{0} was found at position {1} and her body weight is {2}", name, position, weight);
                                                              //print a message with the name, weight and position
            else
                Console.WriteLine("{0} was not found.", name); 
                                                              //print a message saying the name has not been found
        }

        /********************************************************************************************************/
    }
}
