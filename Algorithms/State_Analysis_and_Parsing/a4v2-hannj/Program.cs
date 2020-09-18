using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V1_BASELINE
{   class Program
    {   enum StateType {white, word, num, dble, expo};
        enum CharType {whsp, lett, expo, digits, plus, minus, point, quote, endstr};
        const int LMAX = 10000000;
        static char[] line = new char [LMAX];
        static char ch;
        static CharType chtype;
        static StateType state;
        static int wlen;
        static int k;
        static int len;
        static bool StateJustChanged = false;
        static bool hyphen = false;
        /**************************************************************************** MAIN **************************************************************************************/
        static void Main(string[] args)
        {   int nLines = 4;
            int[] llen = new int[nLines];
            string[] lines = new string[] {
                "    first 123		and then -.1234 but you'll need 123.456		 and 7e-4 plus one like +321. all quite avant-",
                "garde   whereas ellen's true favourites are 123.654E-2	exponent-form which can also be -54321E-03 or this -.9E+5",
                "We'll prefer items like			fmt1-decimal		+.1234567e+05 or fmt2-dec -765.3245 or fmt1-int -837465 and vice-",
                "versa or even format2-integers -19283746   making one think of each state's behaviour for 9 or even 3471e-7 states " };

            Console.WriteLine("HERE ARE THE TEXT LINES PRINTED OUT AS SINGLE STRINGS ... EACH FOLLOWED BY ITS LENGTH.");
            Console.WriteLine();

            for (int i = 0; i < nLines; i++)
            {   Console.WriteLine(lines[i]);
                line = lines[i].ToCharArray();
                for (int j = 0; j < line.Length; j++)
                {   wlen++;
                }
                Console.WriteLine(wlen);
                wlen = 0;
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("HERE ARE THE SAME LINES AGAIN ...  this time printed character by character.");
            Console.WriteLine();

            for (int i = 0; i < nLines; i++) //line loop
            {   line = lines[i].ToCharArray(); //convert to char array
                Console.WriteLine(line);
                Console.WriteLine();
                if (hyphen)
                    state = StateType.word;
                else
                    state = StateType.white;
                
                len = line.Length;             //length of current line
                k = 0;                         //set subscript
                ch = line[0];                  //get 1rst char
                chtype = getType(ch);            //char loop for current line
                while(k<len-1)                 // line still in progress
                {   switch(state) //branch to state-handler
                    {   case StateType.white:
                            WhiteState();
                            break;
                        case StateType.word:
                            WordState();
                            break;
                        case StateType.num:
                            NumState();
                            break;
                        case StateType.dble:
                            DbleState();
                            break;
                        case StateType.expo:
                            ExptState();
                            break;
                    }
                }
                
                if (state == StateType.word && ch == '-')
                    hyphen = true;
                else
                    hyphen = false;
                Console.Write("\n Press any key to continue...\n");
                Console.ReadKey();
            }
        }

        static CharType getType(char ch)
        {   CharType type = CharType.whsp;
            if (isSpace(ch))
                type = CharType.whsp;
            else if (isAlpha(ch))
            {   if (toUpper(ch) == 'E')
                    type = CharType.expo;
                else
                    type = CharType.lett;
            }       
            else if (isDigit(ch))
                type = CharType.digits;
            else
            {   switch (ch)
                {   case '+':
                        type = CharType.plus;
                        break;
                    case '-':
                        type = CharType.minus;
                        break;
                    case '.':
                        type = CharType.point;
                        break;
                    case '\'':
                        type = CharType.quote;
                        break;
                }
            }
            return type;
        }

        static char toUpper(char ch)
        {   if (ch >= 'a' && ch <= 'z')
                ch = (char)(ch - ('a'-'A'));
            return ch;
        }

        static bool isSpace(char ch)
        {   return (ch == ' ' || ch == '\t' || ch == '\n');
        }

        static bool isDigit(char ch)
        {   return (ch >= '0' && ch <= '9');
        }

        static bool isAlpha(char ch)
        {   return (toUpper(ch) >= 'A' && toUpper(ch) <= 'Z');
        }

        static void WhiteState()
        {   while(state == StateType.white && k < len) // state still is white and line still in progress
                {   switch(chtype)
                    {   case CharType.lett:
                        case CharType.expo:
                            WhiteToWord(); //words start with a letter
                            break;
                        case CharType.digits:
                        case CharType.plus:
                        case CharType.minus:
                            WhiteToNum(); //starting an integer
                            break;
                        case CharType.point:
                            WhiteToDble(); // starting a double
                            break;
                        default:
                            Console.Write(ch);
                            StateJustChanged = false;
                            if (k < len - 1)
                            {   ch = line[++k];
                                chtype = getType(ch);
                            }
                            else
                                return;
                            break;
                    }
                }
        }

        static void WordState()
        {   while (state == StateType.word && k < len) // state still is word and line still in progress
            {   switch (chtype)
                {   case CharType.whsp:
                        WordToWhite(); 
                        break;
                    default:
                        Console.Write(ch);
                        StateJustChanged = false;
                        if (k < len - 1)
                        {   ch = line[++k];
                            chtype = getType(ch);
                        }
                        else
                            return;
                        break;
                }
            }
        }

        static void NumState()
        {   while (state == StateType.num && k < len) // state still is num and line still in progress
            {   switch (chtype)
                {   case CharType.whsp:
                        NumToWhite(); //starting an integer
                        break;
                    case CharType.point:
                        NumToDble(); // starting a double
                        break;
                    case CharType.expo:
                        NumToExpo();
                        break;
                    default:
                        Console.Write(ch);
                        StateJustChanged = false;
                        if (k < len - 1)
                        {   ch = line[++k];
                            chtype = getType(ch);
                        }
                        else
                            return;
                        break;
                }
            }
        }

        static void DbleState()
        {   while (state == StateType.dble && k < len) // state still is double and line still in progress
            {   switch (chtype)
                {   case CharType.whsp:
                        DbleToWhite(); //starting an integer
                        break;
                    case CharType.expo:
                        DdleToExpo(); // starting a double
                        break;
                    default:
                        Console.Write(ch);
                        StateJustChanged = false;
                        if (k < len - 1)
                        {   ch = line[++k];
                            chtype = getType(ch);
                        }
                        else
                            return;
                        break;
                }
            }
        }

        static void ExptState()
        {   while (state == StateType.expo && k < len) // state still is expo and line still in progress
            {   switch (chtype)
                {   case CharType.whsp:
                        ExptToWhite(); //starting an integer
                        break;
                    default:
                        Console.Write(ch);
                        StateJustChanged = false;
                        if (k < len - 1)
                        {   ch = line[++k];
                            chtype = getType(ch);
                        }
                        else
                            return;
                        break;
                }
            }
        }

        static void WhiteToWord()
        {   if(!StateJustChanged)
            {   Console.Write("\n");
            }
            StateJustChanged = true;
            Console.Write(ch + "ST0-1\n");
            state = StateType.word;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }

        static void WordToWhite()
        {   if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write(ch + "ST1-0\n");
            state = StateType.white;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }

        static void WhiteToNum()
        {   if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write(ch + "ST0-2\n");
            state = StateType.num;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }

        static void NumToWhite()
        {   if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write( ch + "ST2-0\n");
            state = StateType.white;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }

        static void NumToDble()
        {    if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write(ch + "ST2-3\n");
            state = StateType.dble;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }

        static void WhiteToDble()
        {   if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write(ch + "ST0-3\n");
            state = StateType.dble;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }

        static void NumToExpo()
        {   if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write(ch + "ST2-4\n");
            state = StateType.expo;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        } 

        static void DbleToWhite()
        {   if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write(ch + "ST3-0\n");
            state = StateType.white;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }

        static void DdleToExpo()
        {   if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write(ch + "ST3-4\n");
            state = StateType.expo;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }

        static void ExptToWhite()
        {   if (!StateJustChanged)
                Console.Write("\n");
            StateJustChanged = true;
            Console.Write(ch + "ST4-0\n");
            state = StateType.white;
            if (k < len - 1)
            {   ch = line[++k];
                chtype = getType(ch);
            }
            else
                return;
        }
    }
}
