using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5v4_hannj
{
    class Program
    {
        const int NMAX = 5;        //maximum size of each name string
        const int LSIZE = 5;       //actual number of infix strings in the data array
        const int NOPNDS = 10;     //number of operand symbols in the operand array
        static int IDX;                   //index used to implement conversion stub
        static char[] opnd = new char[NOPNDS] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; //operand symbols
        static double[] opndval = new double[NOPNDS] { 3, 1, 2, 5, 2, 4, -1, 3, 7, 187 };         //operand values
        static List<double> OPNDstack = new List<double>();
        static List<char> OPRstack = new List<char>();


        static void Main()
        {
            Console.WindowWidth = 120;
            Console.WindowHeight = Console.WindowWidth * 9 / 25;
            /*************************************************************************
                                      KEY DECLARATIONS            
            *************************************************************************/
            string[] infix = new string[LSIZE] { "C$A$E",    //array of infix strings
                             "(A+B)*(C-D)",
                             "A$B*C-D+E/F/(G+H)",
                             "((A+B)*C-(D-E))$(F+G)",
                             "A-B/(C*D$E)"  };



            /*************************************************************************
                   PRINT OUT THE OPERANDS AND THEIR VALUES            
             *************************************************************************/
            Console.WriteLine("\nOPERAND SYMBOLS USED:\n");   //title
            for (int i = 0; i < NOPNDS; i++)
                Console.Write(opnd[i].ToString().PadLeft(5));

            Console.WriteLine("\n\n\nCORRESPONDING OPERAND VALUES:\n");   //title
            for (int i = 0; i < NOPNDS; i++)
                Console.Write(opndval[i].ToString().PadLeft(5));

            Console.WriteLine("\n\n");

            /*************************************************************************
                                            OUTPUT LINES
            *************************************************************************/
            Console.WriteLine("Infix Expression".PadRight(30) + "Postfix Expression".PadRight(30) + "Value".PadRight(20));
            OutLine(70, '=');

            for (IDX = 0; IDX < LSIZE; IDX++)
            {
                string postfix = ConvertToPostfix(infix[IDX]);
                Console.WriteLine(infix[IDX].PadRight(30) + postfix.PadRight(30) + EvaluatePostfix(postfix));
            }

            Console.ReadLine();
            Console.ReadLine();

            testStack();                // uses function dumpOPNDstack() to test our stack functions
            Console.ReadLine();         // testStack() does 5 pushes, then 5 pops
            Console.ReadLine();         // the entire stack is displayed ofter each action 

            testStackOPR();
            Console.ReadLine();

            Console.ReadLine();
        }



        /***************************************************************************************** 
                FUNCTION OutLine:   formatting function to print n repetitions of char ch
        ******************************************************************************************/
        static void OutLine(int n, char ch)
        {
            for (int q = 0; q < n; q++)
                Console.Write(ch.ToString());

            Console.WriteLine("\n");
        }
        /*************************************************************************
                                CONVERSION FUNCTION     
        *************************************************************************/
        static string ConvertToPostfix(string infix)
        {
            string[] postfix = new string[LSIZE] { "CAE$$",  //array of postfix strings
                             "AB+CD-*",
                             "AB$C*D-EF/GH+/+",
                             "AB+C*DE--FG+$",
                             "ABCDE$*/-"  };
            return postfix[IDX];
        }
        /*************************************************************************
                                EVALUATION FUNCTION  
        *************************************************************************/
        static double EvaluatePostfix(string postfix)
        {
            char[] expression = postfix.ToCharArray();
            char s;
            int j = 0;
            int indexExpression = 0;
            double val = 0, op1, op2;

            while (indexExpression < expression.Length)
            {
                s = expression[indexExpression];
                if (s >= 'A' && s <= 'Z')
                {
                    j = s - 'A';
                    OPNDpush(opndval[j]);
                }
                else
                {
                    op2 = OPNDpop();
                    op1 = OPNDpop();
                    switch (s)
                    {
                        case '+':
                            val = op1 + op2;
                            break;
                        case '-':
                            val = op1 - op2;
                            break;
                        case '*':
                            val = op1 * op2;
                            break;
                        case '/':
                            val = op1 / op2;
                            break;
                        case '$':
                            val = Math.Pow(op1, op2);
                            break;
                    }
                    OPNDpush(val);
                }
                indexExpression++;
            }
            val = OPNDpop();
            return val;
            // return new Random(postfix.GetHashCode()).Next(100000, 500000) / 100;
        }

        static void testStack()
        {
            var rand = new Random();

            Console.WriteLine("\n\nOperation".PadRight(15) + "Value Pushed/Popped".PadRight(25) + "Stack After Operation");
            OutLine(70, '=');

            for (int i = 0; i < 5; i++)
            {
                double value = ((double)rand.Next(1000, 10000)) / 100; 	// create random double with 2 decimal places
                OPNDpush(value);					// push this value onto the stack
                Console.Write("PUSH".PadRight(15) + value.ToString().PadRight(25));
                dumpOPNDstack();					// display the entire stack after each push
                Console.WriteLine("\n");
            }

            for (int i = 0; i < 5; i++)
            {
                double value = OPNDpop();			// pop the current top-element off the stack	
                Console.Write("POP".PadRight(15) + value.ToString().PadRight(25));
                dumpOPNDstack();					// display the entire stack after each pop
                Console.WriteLine("\n");
            }

        }

        static void testStackOPR()
        {
            Console.WriteLine("TESTING WITH PUSH AND POP");
            Console.WriteLine("\n\nOperation".PadRight(15) + "Value Pushed/Popped".PadRight(25) + "Stack After Operation");
            OutLine(70, '=');

            for (int i = 0; i < 5; i++)
            {
                char value = opnd[i]; 
                OPRpush(value);					// push this value onto the stack
                Console.Write("PUSH".PadRight(15) + value.ToString().PadRight(25));
                dumpOPRstack();					// display the entire stack after each push
                Console.WriteLine("\n");
            }

            for (int i = 0; i < 5; i++)
            {
                char value = OPRpop();			// pop the current top-element off the stack	
                Console.Write("POP".PadRight(15) + value.ToString().PadRight(25));
                dumpOPRstack();					// display the entire stack after each pop
                Console.WriteLine("\n");
            }
            Console.WriteLine();
            Console.WriteLine("TESTING WITH PUSH, POP AND TEST");

            Console.WriteLine("\n\nOperation".PadRight(15) + "Value Pushed/Popped".PadRight(25) + "Stack After Operation");
            OutLine(70, '=');

            for (int i = 0; i < 5; i++)
            {
                char value = opnd[i];
                OPRpush(value);					// push this value onto the stack
                Console.Write("PUSH".PadRight(15) + value.ToString().PadRight(25));
                dumpOPRstack();					// display the entire stack after each push
                Console.WriteLine("\n");
            }

            for (int i = 0; i < 5; i++)
            {
                char value = '\0';
                bool und = OPRpopAndtest(ref value); // pop the current top-element off the stack	
                Console.Write("POP".PadRight(15) + value.ToString().PadRight(25));
                if(und == true)
                {
                    Console.Write("EMPTY");
                    break;
                }
                dumpOPRstack();					// display the entire stack after each pop
                Console.WriteLine("\n");
            }

        }

        /*************************************************************************
                            STACK FUNCTIONS:
		- the global object "OPNDstack" is an instance of the class "List"
		- ihe contents of "OPNDstack" are doubles
		- see its declaration immediately before the Main block

        *************************************************************************/
        static void OPNDpush(double opnd)
        {
            OPNDstack.Add(opnd);
        }

        static double OPNDpop()
        {
            double last = OPNDstack[OPNDstack.Count - 1];
            OPNDstack.RemoveAt(OPNDstack.Count - 1);
            return last;
        }

        static void dumpOPNDstack()
        {
            foreach (double value in OPNDstack)
                Console.Write(value + " | ");
            if (OPNDstack.Count == 0)
                Console.Write("EMPTY");
        }

        static void OPRpush(char opr)
        {
            OPRstack.Add(opr);
        }

        static char OPRpop()
        {
            char last = OPRstack[OPRstack.Count - 1];
            OPRstack.RemoveAt(OPRstack.Count - 1);
            return last;
        }
        
        static bool OPRpopAndtest(ref char last)
        {
            bool und;
            if (OPRstack.Count != 0)
            {
                last = OPRstack[OPRstack.Count - 1];
                OPRstack.RemoveAt(OPRstack.Count - 1);

                if (OPRstack.Count != 0)
                    und = false;
                else
                    und = true;
            }
            else
                und = false;

            return und;
        }

        static void dumpOPRstack()
        {
            foreach (char value in OPRstack)
                Console.Write(value + " | ");
            if (OPRstack.Count == 0)
                Console.Write("EMPTY");
        }
    }
}
