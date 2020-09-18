/*********************************************************************************************
 * 
 * Due Date:                      December 7th, 2019
 * Software Designer:             Jennifer Hann
 * Course:                        420-306-AB (Fal 2019)
 * Delieverale:                   Assignment #5 --- Stacks & Expression Evaluation
 * 
 * Description:                   This program takes in a series of infix expressions.
 *                                Then the program display the operand symbols that are used in the infix expressions.
 *                                Afterwards, the corresponding operand values are display.
 *                                
 *                                Each infix expressions is converted into a postfix expression through a conversion algorithm.
 *                                The conversion algorithm scans the infix expression  and employs a stack of characters to store 
 *                                the operators encountered.
 *                                Once the postfix expression is finish, an evaluation algorithm will calculate the numerical value.
 *                                The evaluation algorithm store the values of symbolic operands and numerical values generated from 
 *                                successive arithmetic operations.
 *                                After each infix is converted and evaluated, the program display the infix expression, the postfix 
 *                                expression and the numerical value.
 *                                Once an empty line is entered by the user, the program will end.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5v5_hannj
{
    class Program
    {
        const int NMAX = 5;                 //maximum size of each name string
        const int LSIZE = 5;                //actual number of infix strings in the data array
        const int NOPNDS = 10;              //number of operand symbols in the operand array
        static int IDX;                     //index used to implement conversion stub
        static char[] opnd = new char[NOPNDS] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; //operand symbols
        static double[] opndval = new double[NOPNDS] { 3, 1, 2, 5, 2, 4, -1, 3, 7, 187 };           //operand values
        static List<double> OPNDstack = new List<double>();        //operand stack
        static List<char> OPRstack = new List<char>();             //operator stack 

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
            for (int i = 0; i < NOPNDS; i++)                  //print all the operands
                Console.Write(opnd[i].ToString().PadLeft(5));

            Console.WriteLine("\n\n\nCORRESPONDING OPERAND VALUES:\n");   //title
            for (int i = 0; i < NOPNDS; i++)       //print value of corresponding operands
                Console.Write(opndval[i].ToString().PadLeft(5));

            Console.WriteLine("\n\n");

            /*************************************************************************
                                            OUTPUT LINES
            *************************************************************************/
            Console.WriteLine("Infix Expression".PadRight(30) + "Postfix Expression".PadRight(30) + "Value".PadRight(20));
            OutLine(70, '=');                 //title

            for (IDX = 0; IDX < LSIZE; IDX++) //iterate over all infix expression
            {
                string postfix = ConvertToPostfix(infix[IDX]);  //convert current infix expression to postfix
                Console.WriteLine(infix[IDX].PadRight(30) + postfix.PadRight(30) + EvaluatePostfix(postfix));  //get current infix and postfix expression and calculate postfix, then print
            }

            Console.ReadLine();
        }



        /***************************************************************************************** 
                FUNCTION OutLine:   formatting function to print n repetitions of char ch
        ******************************************************************************************/
        static void OutLine(int n, char ch) //draw a seperator
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
            string postfix = string.Empty;              //string that save postfix expression
            char[] expression = infix.ToCharArray();    //store char from current infix expression
            char s;                                     //current symbol (char) from infix
            int indexExpression = 0;                    //keep track of current char
            char topsym = '+';                          //last symbol that was on stack
            bool und;                                   //keep track if topsym is empty or not

            while(indexExpression <  expression.Length) //symbol still remain in infix expression
            {
                s = expression[indexExpression];        //get current symbol
                if(s >= 'A' && s <= 'Z')                //symbol is an operand
                    postfix = postfix + s;              //save symbol to postfix expression 
                else
                {
                    und = OPRpopAndtest(ref topsym);    //if true, stack and topsym is empty
                    while(und == false && prcd(topsym,s))   //got something from stack and higher priority than current symbol
                    {
                        postfix = postfix + topsym;         //add operator to postfix expression
                        und = OPRpopAndtest(ref topsym);    //if true, stack and topsym is empty, get next symbol from stack
                    }
                    if (und == false)                    //topsym is not empty
                        OPRpush(topsym);                 //return topsym back to stack
                    if (und == true || s != ')')         //topsym is empty OR current symbol is ')'
                        OPRpush(s);                      //add symbol to the stack
                    else
                        topsym = OPRpop();               //topsym get last symbol in stack
                }

                indexExpression++;                        //go to next symbol in infix expression
            }
            for (int i = 0; i < OPRstack.Count; i++)      //iterate through stack
            {
                postfix = postfix + OPRpop();             //add last symbol in stack to postfix expression
                if(OPRstack.Count > 0)                    //stack is not empty
                    i = -1;                               //reset counter 
            }
            return postfix;                               //send converted infix expression
        }

        /*************************************************************************
                                     PRECEDENCE RULES     
        *************************************************************************/
        static bool prcd(char t, char s) //check if topsym is higher priority than current symbol
        {
            bool x;
            if (t == '(')                 //opening new scope
                x = false;                //left parenthesis is not an operator, thus no priority
            else if (s == '(')            //opening new scope 
                x = false;                //left parenthesis is not an operator, thus no priority
            else if (s == ')')            //current symbol is a closing paranthesis, closing a scope
                x = true;                 //all operator before s must be add to postfix
            else if (s == '$')            //exception: right $ higher priority than all operator
                x = false;                //s will be added first to the postfix expression
            else if (Rank(t) >= Rank(s))  //get and compare rank for t and s
                x = true;                 //t has higher priority than s
            else
                x = false;                //s has higher priority than t
            return x;
        }
        /*************************************************************************
                                     RANK FUNCTION     
        *************************************************************************/
        static int Rank(char a) //determine the rank of the operator
        {
            int r = 0;                      //rank of operator
            if (a == '+' || a == '-')       //lowest value
                r = 1;
            else if (a == '*' || a == '/')  //middle value
                r = 2;                      //higher priority than + and -
            else if (a == '$')              //high value
                r = 3;                      //highest priority over all operator
            return r;                       //return rank of operator
        }
        /*************************************************************************
                                EVALUATION FUNCTION  
        *************************************************************************/
        static double EvaluatePostfix(string postfix)   //calculate postfix to numeric value
        {
            char[] expression = postfix.ToCharArray(); //store each symbol (char) of the current postfix
            char s;                                    //current symbol (char)
            int j = 0;                                 //index of operand symbol
            int indexExpression = 0;                   //keep track of current symbol (char)
            double val = 0;                            //postfix numeric value
            double op1, op2;                           //value of operand from stack

            while (indexExpression < expression.Length) //symbol still remain in postfix expression
            {
                s = expression[indexExpression];     //get current symbol
                if (s >= 'A' && s <= 'Z')            //symbol is an operand
                {
                    j = s - 'A';                     //get index of current operand symbol
                    OPNDpush(opndval[j]);            //save value of operand to stack
                }
                else
                {
                    op2 = OPNDpop();                 //get last element in stack
                    op1 = OPNDpop();                 //get last element in stack
                    switch (s)                       //current symbol is an operator
                    {
                        case '+':                    //addition symbol
                            val = op1 + op2;         //calculate sum of two last element in stack
                            break;                   //exit switch-case
                        case '-':                    //substraction symbol
                            val = op1 - op2;         //calculate substraction of two last element in stack
                            break;
                        case '*':                    //multiplication symbol
                            val = op1 * op2;         //multiply two last element in stack
                            break;
                        case '/':                    //division symbol
                            val = op1 / op2;         //divide two last element in stack
                            break;
                        case '$':                    //exponent symbol
                            val = Math.Pow(op1, op2);//calculate exponent value of last two element in stack
                            break;
                    }
                    OPNDpush(val);    //return result of operation to stack
                }
                indexExpression++;    //go to next symbol in postfix expression
            }
            val = OPNDpop();          //get final numeric result of postfix from stack
            return val;               //return calculated value of postfix
        }

        /*************************************************************************
                            STACK FUNCTIONS:
		- the global object "OPNDstack" is an instance of the class "List"
		- ihe contents of "OPNDstack" are doubles
		- see its declaration immediately before the Main block

        *************************************************************************/
        /****************** OPERAND STACK FUNCTIONS *******************/
        static void OPNDpush(double opnd)   //add operand to the stack
        {
            OPNDstack.Add(opnd);            //add operand to list
        }

        static double OPNDpop()             //remove operand from stack
        {
            double last = OPNDstack[OPNDstack.Count - 1];    //get value of operand
            OPNDstack.RemoveAt(OPNDstack.Count - 1);         //remove last operand from stack
            return last;                     //return numeric value of operand
        }
        /****************** OPERATOR STACK FUNCTIONS ******************/
        static void OPRpush(char opr)        //add operator to stack
        {
            OPRstack.Add(opr);               //add operator to list
        }

        static char OPRpop()         //remove operator from stack
        {
            char last = OPRstack[OPRstack.Count - 1];    //get last operator
            OPRstack.RemoveAt(OPRstack.Count - 1);       //remove last operator from stack
            return last;             //return last operator
        }

        static bool OPRpopAndtest(ref char last)
        {
            bool und;                   //underflow
            if (OPRstack.Count != 0)    //stack is not empty
            {
                last = OPRstack[OPRstack.Count - 1];   //get last operator in stack and put it as topsym
                OPRstack.RemoveAt(OPRstack.Count - 1);  //remove last operator in stack
                if (last != '\0')       //stack is empty
                    und = false;        //topsym is not empty
                else
                    und = true;         //topsym is empty
            }
            else
                und = true;             //stack is empty

            return und;                 //if true, stack and topsym is empty
        }
    }
}
