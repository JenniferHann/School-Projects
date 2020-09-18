/************************************************************* ID BLOCK ********************************************************************
 * 
 * Due Date:              November 18th, 2019
 * Software Designer:     Jennifer Hann
 * Course:                420-306-AB (Fall 2019)
 * Deliverable:           Assignment #4 --- State Analysis and Parsing
 * 
 * Description:           This program takes in a text data that will be obtained by initializing the test-data string within the program.
 *                        This test-data string is display to the user.
 *                        The program then seperate the test-data string into a number of text lines.
 *                        Then, each text lines is seperate even further into individual character.
 *                        The characters are parse and can be either whitespace (new lines, spaces or tabs), letters, digits, signs, decimal points, hyphens or apostrophes.
 *                        
 *                        Afterwards, the characters are processed in a given state until a character causes a state-transition into another state.
 *                        For each character encountered, there is a character processing if the state is unchanged, but also if the state changes.
 *                        While the text-data is analyse, information about the text-data will be stored in three arrays:
 *                                  mywords    array that keeps track of the number of words of various lengths,
 *                                  mydbles    array that holds all converted double constants encountered,
 *                                  myints     array that holds all the integer constants encountered.
 *                        
 *                        Once all the text lines have been analyse, the program will display three tables:
 *                                  word table     display the number of words of various lengths,
 *                                  integer table  display all integer constant encountered,
 *                                  double table   display all converted double constants encountered.
 *                        Once an empty line is entered by the user, the program will end.
 *                                  
 ******************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4v4_hannj
{   class Program
    {   enum StateType { white, word, num, dble, expt };                                 //list of state variable
        enum CharType { whsp, lett, expo, digit, plus, minus, point, quote, stop };      //list of character category 

        static char[] line;                                     //store the current string as an array of char
        static char ch;                                         //current character being processed
        static CharType type = CharType.whsp;                   //variable to store the type of the current character
        static StateType state = StateType.white;               //current state of the application
        static int wlen = 0;                                    //length of the current word
        static int k;                                           //subscript indicating present position within the current line
        static int len;                                         //length of current line
        static int ival;                                        //store the integer value  
        static double val;                                      //store the double value
        static int sign;                                        //store the sign of the interger and double value
        static int esign;                                       //store the sign of the exponent value
        static int expval;                                      //store the exponent value
        static int power;                                       //power of 10 to shif the decimal point to the left or right

        const int LMAX = 1000;                              //max size of the arrays
        static int[] mywords = new int[LMAX];               //store the number of words of various lengths
        static int[] myints = new int[LMAX];                //store all the integer constants encountered
        static double[] mydbles = new double[LMAX];         //store all the converted double constants encountered

        static int countI = 0;                              //keep track of the index of the array of integers
        static int countD = 0;                              //keep track of the index of the array of doubles

        /****************************************************** MAIN BLOCK **************************************************/
        static void Main(string[] args)
        {   int nLines = 4;                            //number of strings 
            string[] lines = new string[] {
                "    first 123		and then -.1234 but you'll need 123.456		 and 7e-4 plus one like +321. all quite avant-",
                "garde   whereas ellen's true favourites are 123.654E-2	exponent-form which can also be -54321E-03 or this -.9E+5",
                "We'll prefer items like			fmt1-decimal		+.1234567e+05 or fmt2-dec -765.3245 or fmt1-int -837465 and vice-",
                "versa or even format2-integers -19283746   making one think of each state's behaviour for 9 or even 3471e-7 states " }; //data to be analyse
            Console.Write("\n\n");

            for (int k = 0; k < nLines; k++)           //iterate over all the strings in lines array
                Console.Write(lines[k] + "\n\n");      //print out the text lines as a single strings

            for (int i = 0; i < nLines; i++)           //iterate over each string of data
            {   line = lines[i].ToCharArray();         //parse string into an array of char
                len = line.Length;                     //number of char for current string
                k = 0;                                 //keeps track of index in line array
                if (state != StateType.word || type != CharType.minus)     //first char of the line is not a word nor a number
                    state = StateType.white;                                    //set default state 
                ch = line[0];                          //get first char from the line array
                type = getType(ch);                    //check type of the current char

                while (k < len)                        //line still in progress
                {   switch (state)                     //branch to state-handler
                    {   case StateType.white:          //current char is a tab or a space
                            WhiteState();              //beginning of a space or a tab
                            break;                     //exit switch case
                        case StateType.word:           //current char is a letter
                            WordState();               //beginning of a word
                            break;                     //exit switch case
                        case StateType.num:            //current char is a plus, minus or number
                            NumState();                //beginning of a integer
                            break;                     //exit switch case
                        case StateType.dble:           //current char is a decimal point
                            DblState();                //beginning of a double
                            break;                     //exit switch case
                        case StateType.expt:           //current char is an e
                            ExpoState();               //beginning of a exponent
                            break;                     //exit switch case
                    }
                }
                if (type != CharType.minus)          //char is not a hyphen
                {   if (state == StateType.word)     //last char of the line is a word
                        WordToWhite();               //finish the word
                    if (state == StateType.num)      //last char of the line is a num
                        NumToWhite();                //finish the number
                    if (state == StateType.dble)     //last char of the line is a double
                        DblToWhite();                //finish the double
                    if (state == StateType.expt)     //last char of the line is an exponent
                        ExpoToWhite();               //finish the exponent
                }
            }
            
            Console.WriteLine("\n");
            PrintTables();                           //display the results from the data analyse 
            Console.ReadLine();                      //wait for user to terminate program
        }

        /**************************************** GET TYPE **********************************************/
        static CharType getType(char c)             //check type for current char
        {   CharType type = CharType.whsp;          //set default type, each value is seperated by a space
            if (isSpace(c))                         //compare current char to different type of white space
                type = CharType.whsp;               //current char is a white space
            else if (isAlpha(c))                    //compare current chart to ASCII value for letters
                if (toUpper(c) == 'E')              //current char is E
                    type = CharType.expo;           //beginning an exponent value
                else
                    type = CharType.lett;           //beginning of a word
            else if (isDigit(c))                    //compare current char to ASCII value for digits
                type = CharType.digit;              //beginning of a number
            else
            {   switch (c)                          //current char is a symbol
                {   case '+':                       //current char is a plus sign
                        type = CharType.plus;       //number is positive, sign of a number
                        break;                      //exit switch case
                    case '-':                       //current char is a negative sign
                        type = CharType.minus;      //number is negative, sign of a number
                        break;
                    case '.':                       //current char is a decimal point
                        type = CharType.point;      //number is a double
                        break;
                    case '\'':                      //current char is a quote
                        type = CharType.quote;      //beginning of a quote
                        break;
                }
            }
            return type;                            //send type of current char
        }
        /************************************************************************************************/
        /******************************** CHECK FOR A WHITE SPACE ***************************************/
        static bool isSpace(char c)
        {   return (c == ' ' || c == '\t' || c == '\n');  //current char is either a empty string, a line skip or a tab
        }
        /************************************************************************************************/
        /************************************* CHECK FOR A DIGIT ****************************************/
        static bool isDigit(char c)
        {   return (c >= '0' && c <= '9');                       //current char is a digit
        }
        /************************************************************************************************/
        /************************************ CHECK FOR A LETTE******************************************/
        static bool isAlpha(char c)
        {   return ((toUpper(c) >= 'A' && toUpper(c) <= 'Z'));    //current char is a letter
        }
        /************************************************************************************************/
        /******************************* CONVERT TO UPPER CASE LETTER ***********************************/
        static char toUpper(char c)
        {   if (c >= 'a' && c <= 'z')                              //current char is a letter
                c = (char)(c - ('a' - 'A'));                       //convert char to upper case using ASCII
            return c;
        }
        /************************************************************************************************/
        /*************************************** WHITE STATE ********************************************/
        static void WhiteState()
        {   while (state == StateType.white && k < len)    //state still is white + line still in progress
            {   switch (type)                              //get appropriate transition from white state to new state
                {   case CharType.lett:         //current char is a letter
                    case CharType.expo:         //current char is a 'e'
                        WhiteToWord();          //starting a word
                        break;                  //exit switch case
                    case CharType.digit:        //current char is a digit
                    case CharType.plus:         //current char is a plus sign
                    case CharType.minus:        //current char is a minus sign
                        WhiteToNum();           //starting an integer
                        break;
                    case CharType.point:        //current char is a decimal point
                        WhiteToDble();          //starting a double
                        break;
                    default:                    //type of char is stil the same
                        if (k < len - 1)        //line still in progress
                            ch = line[++k];     //get next char from line
                        else
                        {   k++;                // trigger exit
                            return;             //exit to the next line
                        }
                        type = getType(ch);     //check type of new char
                        break;                  //exit switch case
                }
            }
        }
        /************************************************************************************************/
        /*************************************** WHITE TO WORD ******************************************/
        static void WhiteToWord()       //start of a word + current char is a letter
        {   wlen = 0;                   //reset count for the word length
            state = StateType.word;     //starting a word
        }
        /************************************************************************************************/
        /*************************************** WHITE TO NUM *******************************************/
        static void WhiteToNum()         //start of a integer + current char is a digit
        {   type = getType(ch);          //determine type of current char 
            sign = CaptureSign(ch);      //determine the sign of the integer
            ival = 0;                    //reset integer value
            state = StateType.num;       //starting a integer
        }
        /************************************************************************************************/
        /*************************************** WHITE TO DBLE ******************************************/
        static void WhiteToDble()         //current char is a decimal point + starting a double
        {   if (k < len)                  //line still in progress
                ch = line[++k];           //get next char from line
            else
            {   k++;                      // Trigger Exit
                return;                   //exit from function
            }
            type = getType(ch);           //determine type of new char
            val = 0;                      //reset double value
            sign = 1;                     //reset sign of double value
            power = 1;                    //reset power of double value
            state = StateType.dble;       //starting a double
        }
        /************************************************************************************************/
        /*************************************** WORD STATE *********************************************/
        static void WordState()
        {   while (state == StateType.word && k < len)   //state still is word and line still in progress
            {   switch (type)                            //transition to either white state or stay in the same state
                {   case CharType.whsp:                  //current char is a white space
                        WordToWhite();                   //end of the word and beginning of a white space
                        break;                           //exit switch case
                    default:                             //current char is still a letter
                        wlen++;                          //increase count for the word length
                        if (k < len - 1)                 //line still in progress
                            ch = line[++k];              //get next char in the line
                        else
                        {   k++;                         //trigger exit
                            return;                      //exit to next line if any
                        }
                        type = getType(ch);              //get type of new char
                        break;
                }
            }
        }
        /************************************************************************************************/
        /*************************************** WORD TO WHITE ******************************************/
        static void WordToWhite()                        //word is finish
        {   for (int i = 0; i < mywords.Length; i++)     //find corresponding length to wlen
            {   if (wlen == i)                           //length of word at the right index
                    mywords[i]++;                        //increase frequency for the word length
            }
            state = StateType.white;                     //current char is a white space
        }
        /************************************************************************************************/
        /**************************************** NUM STATE *********************************************/
        static void NumState()                           //current char is a digit
        {   while (state == StateType.num && k < len)    //state still is num + line still in progress
            {   switch (type)                            //go to appropriate state according to type of current char
                {   case CharType.whsp:                  //current char is a white space
                        NumToWhite();                    //number is finish
                        break;                           //exit switch case
                    case CharType.point:                 //current char is a decimal point
                        NumToDbl();                      //interger is now a double
                        break;
                    case CharType.expo:                  //current char is a e
                        NumToExpo();                     //number is now an exponent 
                        break;
                    default:
                        ival = ival * 10 + (ch - '0');   //new value of the integer number
                        if (k < len - 1)                 //line still in progress
                            ch = line[++k];              //get next char
                        else
                        {   k++;                         // trigger exit
                            return;                      //exit to next line if any
                        }
                        type = getType(ch);              //get type of new char
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** NUM TO WHITE ******************************************/
        static void NumToWhite()              //number is finish
        {   ival = ival * sign;               //final value of number with the right sign
            myints[countI] = ival;            //save number to myints
            countI++;                         //go to next index of myints
            state = StateType.white;          //current char is a white space
        }
        /************************************************************************************************/
        /**************************************** NUM TO DBL ********************************************/
        static void NumToDbl()                //starting a double + current char is a decimal point
        {   if (k < len - 1)                  //line still in progress
                ch = line[++k];               //get next char after decimal point
            else
            {   k++;                          //trigger exit
                return;                       //exit to next line if any
            }
            type = getType(ch);               //get type of new char
            val = ival;                       //save integer value to double value
            power = 1;                        //reset power of double value
            state = StateType.dble;           //current value of number is now a double
        }
        /************************************************************************************************/
        /**************************************** NUM TO EXPO *******************************************/
        static void NumToExpo()               //starting a exponent + current char is a decimal point
        {   val = ival * sign;                //save integer value to exponent value
            ch = line[++k];                   // Not necessary to check bounds because the e of expo will neve be the last char of a line
            type = getType(ch);               //get type of new char
            esign = CaptureSign(ch);          //save sign to determine if decimal point is moved left or right
            expval = 0;                       //reset expval 
            state = StateType.expt;           //current number is now a exponent
        }
        /************************************************************************************************/
        /**************************************** DBL STATE *********************************************/
        static void DblState()                             //current char came after decimal point
        {   while (state == StateType.dble && k < len)     //state is still a double + line still in progress
            {   switch (type)                              //go to appropriate state according to type of current char
                {   case CharType.whsp:                    //current char is a white space
                        DblToWhite();                      //double is finish
                        break;                             //exit switch case
                    case CharType.expo:                    //current char is an e
                        DblToExpo();                       //double is now an exponent
                        break;
                    default:                               //current char is still a digit 
                        val = val * 10 + ch - '0';         //save new double value
                        power *= 10;                       //keep track of the shifting of decimal point
                        if (k < len - 1)                   //line still in progress
                            ch = line[++k];                //get next char
                        else
                        {   k++;                           //trigger exit
                            return;                        //exit to next line if any
                        }
                        type = getType(ch);                //get type of new char
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** DBL TO WHITE ******************************************/
        static void DblToWhite()          //double is finish
        {   val = val * sign / power;     //get final value of double with the right sign and correct position of decimal point
            mydbles[countD] = val;        //save double to array
            countD++;                     //go to next index
            state = StateType.white;      //current char is a white space
        }
        /************************************************************************************************/
        /**************************************** DBL TO EXPO *******************************************/
        static void DblToExpo()           //starting a exponent + current char is an e
        {   val = val * sign / power;     //save double value to exponent
            ch = line[++k];               //get next char
            type = getType(ch);           //get type of new char
            esign = CaptureSign(ch);      //save sign of the exponent
            expval = 0;                   //reset exponent value
            state = StateType.expt;       //current char are from a exponent
        }
        /************************************************************************************************/
        /**************************************** EXPO STATE ********************************************/
        static void ExpoState()           //current value of line is an exponent
        {   while (state == StateType.expt && k < len)          //state still is an exponent + line still in progress
            {   switch (type)                                   //go to appropriate state according to type of current char
                {   case CharType.whsp:                         //current char is a white space
                        ExpoToWhite();                          //exponent is finish
                        break;                                  //exit switch case
                    default:                                    //current char is still a digit that came after an e
                        expval = expval * 10 + (ch - '0');      //save new value of exponent
                        if (k < len - 1)                        //line still in progress
                            ch = line[++k];                     //get next char of line
                        else
                        {   k++;                                //trigger exit
                            return;                             //exit to next line if any
                        }
                        type = getType(ch);                     //get type of new char
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** EXPO TO WHITE *****************************************/
        static void ExpoToWhite()                     //exponent is finish
        {   if (esign == -1)                          //shift decimal point to the left
                for (int i = 0; i < expval; i++)      //determin amount time deciaml point is shifted
                    val = val / 10;                   //shift decimal point
            else                                      //shift decimal point to the right
                for (int i = 0; i < expval; i++)      //determin amount time deciaml point is shifted
                    val = val * 10;                   //shift decimal point
            mydbles[countD] = val;                    //save final exponent value to mydbles
            countD++;                                 //go to the next index
            state = StateType.white;                  //current char is a white space
        }
        /************************************************************************************************/
        /**************************************** CAPTURE SIGN ******************************************/
        static int CaptureSign(char c)          //determin esign for exponent value
        {   int s;                              //store value of esign
            if (type == CharType.minus || type == CharType.plus)    //check if there is a symbol after 'e'
            {   if (type == CharType.minus)     //symbol '-' is what came after 'e'
                    s = -1;                     //decimal will be shift to the left
                else                            //symbol '+' is what came after 'e'
                    s = 1;                      //decimal will be shift to the right 
                ch = line[++k];                 //get next char
                type = getType(ch);             //get type of new char
            }
            else                                //no symbol came after 'e'
                s = 1;                          //decimal will be shift to the right 
            return s;                           //return value of esign
        }
        /************************************************************************************************/
        /**************************************** PRINT TABLES ******************************************/
        static void PrintTables()        //print the results table following the anayling and parsing of data
        {   int rowLength = 25;          //max length of each row
            Console.WriteLine("     ANALYSIS RESULTS");                   //print title of results table
            Console.WriteLine("---------------------------------\n");     //print seperator

            /****************************************** WORD TABLE **************************************************/
            Console.WriteLine("     WORD RESULTS:");      //print title of word table
            TableHeadingRowWord(rowLength);               //print the heading of table
            for (int i = 0; i < mywords.Length; i++)      //iterate over each frequency (number) in mywords
            {   if (mywords[i] != 0)                      //frequency (number) is not zero for the word length
                {   TableRow(rowLength);                  //print each row seperator
                    Console.WriteLine(String.Format("{0,-1}{1,-5}{2,20}{3,1}", "║", i, mywords[i], "║"));  //print word in mywords
                }
            }
            TableClosingRow(rowLength);                   //print closing row of the table
            Console.WriteLine();
            /********************************************************************************************************/

            /********************************************** INTEGER TABLE *******************************************/
            Console.WriteLine("     INTEGER RESULTS:");   //print title of integer table
            rowLength = 30;                               //max length of each row

            TableHeadingRowNum(rowLength);                //print the heading of the table
            for (int i = 0; i < myints.Length; i++)       //iterate each integer in myints
            {   if (myints[i] != 0)                       //only print result with a value that's not zero
                {   TableRow(rowLength);                  //print each row seperator
                    Console.WriteLine(String.Format("{0,-1}{1,-5}{2,25}{3,1}", "║", i, myints[i], "║"));   //print integer in myints
                }
            }
            TableClosingRow(rowLength);                   //print closing row of the table
            Console.WriteLine();
            /********************************************************************************************************/

            /********************************************** DOUBLE TABLE ********************************************/
            Console.WriteLine("     DOUBLE RESULTS:");    //print title of double table
            TableHeadingRowNum(rowLength);                //print the heading of the table
            for (int i = 0; i < mydbles.Length; i++)      //iterate over each double mydbles
            {   if (mydbles[i] != 0)                      //only print result with a value that's not zero
                {   TableRow(rowLength);                  //print each row seperator
                    Console.WriteLine(String.Format("{0,-1}{1,-5}{2,25}{3,1}", "║", i, mydbles[i], "║"));   //print double in mydbles
                }
            }
            TableClosingRow(rowLength);                   //print closing row of the table
            /********************************************************************************************************/
        }
        /************************************************************************************************/

        /********************************* PRINT TABLE HEADING FOR WORDS ********************************/
        static void TableHeadingRowWord(int row)
        {   Console.Write("╔");                 //print top left corner border of table
            for (int i = 0; i < row; i++)       //repeat symbol("═") 'row' amount of time
                Console.Write("═");             //print symbol that will make the line of the border
            Console.WriteLine("╗");             //print top right corer border of table
            Console.WriteLine(String.Format("{0,-1}{1,5}{2,19}{3,1}", "║", "LENGTH", "FREQUENCY", "║")); //print table header
        }
        /************************************************************************************************/

        /******************************* PRINT TABLE HEADING FOR NUMBERS *******************************/
        static void TableHeadingRowNum(int row)
        {   Console.Write("╔");                 //print top left corner border of table
            for (int i = 0; i < row; i++)       //repeat symbol("═") 'row' amount of time            
                Console.Write("═");             //print symbol that will make the line of the border
            Console.WriteLine("╗");             //print top right corer border of table
            Console.WriteLine(String.Format("{0,-1}{1,5}{2,25}{3,1}", "║", "INDEX", "VALUE", "║"));  //print table header
        }
        /************************************************************************************************/

        /*********************************** PRINT TABLE ROW SEPERATOR **********************************/
        static void TableRow(int row)
        {   Console.Write("╠");                 //print left connector between each row
            for (int j = 0; j < row; j++)       //repeat symbol("═") 'row' amount of time
                Console.Write("═");             //print symbol that will make the line of the border
            Console.WriteLine("╣");             //print right connector between each row
        }
        /************************************************************************************************/

        /************************************** PRINT TABLE CLOSING *************************************/
        static void TableClosingRow(int row)
        {   Console.Write("╚");                 //print bottom left corner border of table
            for (int i = 0; i < row; i++)       //repeat symbol("═") 'row' amount of time
                Console.Write("═");             //print symbol that will make the line of the border
            Console.WriteLine("╝");             //print bottom right corner border of table
        }
        /************************************************************************************************/
    }
}
