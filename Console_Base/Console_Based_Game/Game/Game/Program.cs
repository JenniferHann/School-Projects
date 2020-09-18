using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatAndRunGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //change the size of the window width
            Console.WindowWidth = 180;

            //size of playing screen + position of gameover
            int height = 25;
            int width = 100;
            int a = 40, b = 30;

            //my position
            int x = 55, y = 13;
            int points = 0;
            int life = 3;
            int limitScreen = 2;

            //limits for enemy + speed
            int limity1 = 2;
            int limity2 = 25;
            int speed = 10;

            //enemy positions
            int x1 = 10;
            int y1 = 2;
            int i1 = 0, j1 = 0;

            //enemy positions
            int x2 = 20;
            int y2 = 10;
            int i2 = 0, j2 = 0, t2 = 8;

            //enemy positions
            int x3 = 30;
            int y3 = 25;
            int i3 = 0, j3 = 0;

            //enemy positions
            int x4 = 40;
            int y4 = 8;
            int i4 = 0, j4 = 0, t4 = 6;

            //enemy positions
            int x5 = 50;
            int y5 = 15;
            int i5 = 0, j5 = 0, t5 = 15;

            //enemy positions
            int x6 = 60;
            int y6 = 20;
            int i6 = 0, j6 = 0, t6 = 18;

            //enemy positions
            int x7 = 70;
            int y7 = 5;
            int i7 = 0, j7 = 0, t7 = 5;

            //enemy positions
            int x8 = 80;
            int y8 = 7;
            int i8 = 0, j8 = 0, t8 = 5;

            //enemy positions
            int x9 = 90;
            int y9 = 13;
            int i9 = 0, j9 = 0, t9 = 13;

            //Arrays for my targets
            const int ARRAYSIZET = 5;
            int[] thingsArrayX = new int[ARRAYSIZET];
            int[] thingsArrayY = new int[ARRAYSIZET];

            //Arrays for enemies
            const int ARRAYSIZEEX = 9;
            const int ARRAYSIZEEY = 24;
            int[] thingsArrayXE = new int[ARRAYSIZEEX];
            int[] thingsArrayYE = new int[ARRAYSIZEEY];

            //fill array enemy for x position
            fillenemyX(thingsArrayXE);

            //fill aray enemy for y position
            fillenemyY(thingsArrayYE);

            //boder 
            border(height, width);

            //instructions
            instructions();

            //targets 
            myobject(thingsArrayX, thingsArrayY);

            //start position
            Console.SetCursorPosition(x, y);
            Console.Write("x");

            while (!gameover(life, ref points))
            {
                //write life
                score(ref points, ref life);

                //move me
                move(ref x, ref y, height, width, limitScreen);

                //enemies moves
                enemyMove(ref x1, ref y1, ref i1, ref j1, ref x2, ref y2, ref i2, ref j2, ref t2, ref x3, ref y3, ref i3, ref j3, ref x4, ref y4, ref i4, ref j4, ref t4, ref x5, ref y5, ref i5, ref j5, ref t5, ref x6, ref y6, ref i6, ref j6, ref t6, ref x7, ref y7, ref i7, ref j7, ref t7, ref x8, ref y8, ref i8, ref j8, ref t8, ref x9, ref y9, ref i9, ref j9, ref t9, limity1, limity2, height, speed);

                //when me touch the enemies
                //touchE(ref x, ref y,thingsArrayXE, thingsArrayYE, ARRAYSIZEEX, ARRAYSIZEEY, ref life);
                touchE(ref x, ref y, ref x1, ref y1, ref x2, ref y2, ref x3, ref y3, ref x4, ref y4, ref x5, ref y5, ref x6, ref y6, ref x7, ref y7, ref x8, ref y8, ref x9, ref y9, ref life);

                //when me touch the target
                touchT(thingsArrayX, thingsArrayY, ARRAYSIZET, ref x, ref y, ref points);
            }

            //display game over
            Console.SetCursorPosition(a, b);
            Console.WriteLine("GAME OVER");
            b = b + 1;
            a = a - 11;
            Console.SetCursorPosition(a, b);
            Console.WriteLine("Press on the escape key to exit");
            var exit = Console.ReadKey().Key;
            switch (exit)
            {
                case ConsoleKey.Escape:
                    break;
            }
        }

        //gameover rules
        static bool gameover(int life, ref int points)
        {
            int endL = 0; //end of life
            int endP = 249; //end  of points
            bool game = false;
            if (life == endL)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                game = true;
            }
            if (points > endP)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                game = true;
            }
            return game;
        }

        //instruction
        static void instructions()
        {
            int x = 105, y = 7;

            Console.SetCursorPosition(x, y);
            Console.WriteLine("EAT&RUN");
            y = y + 1; //to skip line
            Console.SetCursorPosition(x, y);
            Console.Write("You are playing as 'X'.");
            y = y + 1;
            Console.SetCursorPosition(x, y);
            Console.Write("To move 'X' use the arrow keys.");
            y = y + 1;
            Console.SetCursorPosition(x, y);
            Console.Write("You must eat the 'O' in order to gain points and avoid the '#'.");
            y = y + 1;
            Console.SetCursorPosition(x, y);
            Console.Write("If you touch the '#' you will lose a life.");
            y = y + 1;
            Console.SetCursorPosition(x, y);
            Console.Write("To win the game, you need to eat all of the 'O'.");
            y = y + 1;
            Console.SetCursorPosition(x, y);
            Console.Write("If you lose all of your lives, it will be game over.");
            y = y + 1;
            Console.SetCursorPosition(x, y);
            Console.Write("When you start to move, the enemies will appear.");
        }

        //score board
        static void score(ref int points, ref int life)
        {
            int x1 = 28, y1 = 28;
            int x2 = 58, y2 = 28;

            Console.SetCursorPosition(x1, y1);
            Console.Write("Points: " + points);

            Console.SetCursorPosition(x2, y2);
            Console.Write("Life: " + life);
        }

        //fill the array with the position x of the enemies 
        static void fillenemyX(int[] thingsArrayXE)
        {
            int a;
            int numE = 9; //number of enemies
            int enePos = 0; //enemy's positions

            for (a = 0; a < numE; a++)
            {
                enePos = enePos + 10;
                thingsArrayXE[a] = enePos;
            }
        }

        //fill the array with the position y of the enemies
        static void fillenemyY(int[] thingsArrayYE)
        {
            int a;
            int numEpos = 24; //number of enemy position
            int enePos = 2; //enemy's position

            for (a = 0; a < numEpos; a++)
            {
                enePos = enePos + 1;
                thingsArrayYE[a] = enePos;
            }
        }

        //make me move
        static void move(ref int x, ref int y, int height, int width, int limit)
        {
            var command = Console.ReadKey().Key;
            switch (command)
            {
                case ConsoleKey.UpArrow:
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                    y--;
                    if (y < limit)
                    {
                        y = height;
                    }
                    Console.SetCursorPosition(x, y);
                    Console.Write("X");
                    break;

                case ConsoleKey.DownArrow:
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                    y++;
                    if (y > height)
                    {
                        y = limit;
                    }
                    Console.SetCursorPosition(x, y);
                    Console.Write("X");
                    break;

                case ConsoleKey.RightArrow:
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                    x++;
                    if (x > width - 1)
                    {
                        x = limit;
                    }
                    Console.SetCursorPosition(x, y);
                    Console.Write("X");
                    break;

                case ConsoleKey.LeftArrow:
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                    x--;
                    if (x < limit)
                    {
                        x = width - 1;
                    }
                    Console.SetCursorPosition(x, y);
                    Console.Write("X");
                    break;
            }
        }

        //draw the border
        static void border(int height, int width)
        {
            int i;
            int x1 = 1, y1 = 1;
            int x2 = 101, y2 = 26;

            //change border design search: alt symbols

            //left border
            for (i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x1, y1);
                Console.Write("|");
                y1++;
            }

            //right border
            for (i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x2, y2);
                Console.Write("|");
                y2--;
            }

            //bottom border
            for (i = 0; i < width; i++)
            {
                Console.SetCursorPosition(x1, y1);
                Console.Write("/");
                x1++;
            }

            //top border
            for (i = 0; i < width; i++)
            {
                Console.SetCursorPosition(x2, y2);
                Console.Write("/");
                x2--;
            }
        }

        //spawn my targets
        static void myobject(int[] TArrayX, int[] TArrayY)
        {
            Random things = new Random();
            int x, y, i;
            const int MYTHINGS = 5;

            //limits
            int width = 99;
            int height = 26;

            for (i = 0; i < MYTHINGS; i++)
            {
                x = things.Next(2, width);
                TArrayX[i] = x;

                y = things.Next(2, height);
                TArrayY[i] = y;

                Console.SetCursorPosition(x, y);

                Console.Write("O");
            }

        }

        //functions calling each enemies to move
        static void enemyMove(ref int x1, ref int y1, ref int i1, ref int j1, ref int x2, ref int y2, ref int i2, ref int j2, ref int t2, ref int x3, ref int y3, ref int i3, ref int j3, ref int x4, ref int y4, ref int i4, ref int j4, ref int t4, ref int x5, ref int y5, ref int i5, ref int j5, ref int t5, ref int x6, ref int y6, ref int i6, ref int j6, ref int t6, ref int x7, ref int y7, ref int i7, ref int j7, ref int t7, ref int x8, ref int y8, ref int i8, ref int j8, ref int t8, ref int x9, ref int y9, ref int i9, ref int j9, ref int t9, int limity1, int limity2, int height, int speed)
        {
            //enemy moving from the top 
            enemyMove1(ref x1, ref y1, ref i1, ref j1, limity1, limity2, height, speed);

            //enemy not moving from middle going down then up
            enemyMove2(ref x2, ref y2, ref i2, ref j2, ref t2, limity1, limity2, height, speed);

            //enemy moving from the bottom
            enemyMove3(ref x3, ref y3, ref i3, ref j3, limity1, limity2, height, speed);

            enemyMove2(ref x4, ref y4, ref i4, ref j4, ref t4, limity1, limity2, height, speed);

            //enemy moving from anywhere going up then down
            enemyMove4(ref x5, ref y5, ref i5, ref j5, ref t5, limity1, limity2, height, speed);

            enemyMove2(ref x6, ref y6, ref i6, ref j6, ref t6, limity1, limity2, height, speed);

            enemyMove4(ref x7, ref y7, ref i7, ref j7, ref t7, limity1, limity2, height, speed);

            enemyMove2(ref x8, ref y8, ref i8, ref j8, ref t8, limity1, limity2, height, speed);

            enemyMove4(ref x9, ref y9, ref i9, ref j9, ref t9, limity1, limity2, height, speed);
        }

        //enemy moving up
        static void routdown(ref int x1, ref int y1, int limity1, int limity2, int speed)
        {
            System.Threading.Thread.Sleep(Convert.ToInt32(speed));
            Console.SetCursorPosition(x1, y1);
            Console.Write(" ");
            y1++;
            Console.SetCursorPosition(x1, y1);
            Console.Write("#");
        }

        //enemy moving down
        static void routup(ref int x1, ref int y1, int limity1, int limity2, int speed)
        {
            System.Threading.Thread.Sleep(Convert.ToInt32(speed));
            Console.SetCursorPosition(x1, y1);
            Console.Write(" ");
            y1--;
            Console.SetCursorPosition(x1, y1);
            Console.Write("#");
        }

        //enemy moving from the top
        static void enemyMove1(ref int x, ref int y, ref int i, ref int j, int limity1, int limity2, int height, int speed)
        {
            i++;

            if (i < height - 1)
            {
                routdown(ref x, ref y, limity1, limity2, speed);
                if (y == height - 1)
                {
                    j = 0;
                }
            }
            else
            {
                j++;

                if (j < height - 1)
                {
                    routup(ref x, ref y, limity1, limity2, speed);
                }

                if (y == height - 23)
                {
                    i = 0;
                }
            }
        }

        //enemy moving from anywhere going from down to up
        static void enemyMove2(ref int x, ref int y, ref int i, ref int j, ref int t, int limity1, int limity2, int height, int speed)
        {
            t++;
            if (t < height - 1)
            {
                routdown(ref x, ref y, limity1, limity2, speed);
            }
            else
            {
                i++;

                if (i < height - 1)
                {
                    routup(ref x, ref y, limity1, limity2, speed);
                    if (y == height - 23)
                    {
                        j = 0;
                    }
                }
                else
                {
                    j++;

                    if (j < height - 1)
                    {
                        routdown(ref x, ref y, limity1, limity2, speed);
                    }

                    if (y == height)
                    {
                        i = 0;
                    }
                }
            }
        }

        //enemy moving from the bottom
        static void enemyMove3(ref int x, ref int y, ref int i, ref int j, int limity1, int limity2, int height, int speed)
        {
            i++;

            if (i < height - 1)
            {
                routup(ref x, ref y, limity1, limity2, speed);
                if (y == height - 23)
                {
                    j = 0;
                }
            }
            else
            {
                j++;

                if (j < height - 1)
                {
                    routdown(ref x, ref y, limity1, limity2, speed);
                }

                if (y == height)
                {
                    i = 0;
                }
            }
        }

        //enemy move from anywhere going up then down
        static void enemyMove4(ref int x, ref int y, ref int i, ref int j, ref int t, int limity1, int limity2, int height, int speed)
        {
            t--;
            if (t > height - 24)
            {
                routup(ref x, ref y, limity1, limity2, speed);
            }
            else
            {
                i++;

                if (i < height - 1)
                {
                    routdown(ref x, ref y, limity1, limity2, speed);
                    if (y == height - 2)
                    {
                        j = 0;
                    }
                }
                else
                {
                    j++;

                    if (j < height - 1)
                    {
                        routup(ref x, ref y, limity1, limity2, speed);
                    }

                    if (y == height - 23)
                    {
                        i = 0;
                    }
                }
            }
        }

        //when me touch the enemies
        static void touchE(ref int x, ref int y, ref int x1, ref int y1, ref int x2, ref int y2, ref int x3, ref int y3, ref int x4, ref int y4, ref int x5, ref int y5, ref int x6, ref int y6, ref int x7, ref int y7, ref int x8, ref int y8, ref int x9, ref int y9, ref int life)
        {
            if (x == x1)
            {
                if (y == y1)
                {
                    life = life - 1;
                }
            }

            if (x == x2)
            {
                if (y == y2)
                {
                    life = life - 1;
                }
            }

            if (x == x3)
            {
                if (y == y3)
                {
                    life = life - 1;
                }
            }

            if (x == x4)
            {
                if (y == y4)
                {
                    life = life - 1;
                }
            }

            if (x == x5)
            {
                if (y == y5)
                {
                    life = life - 1;
                }
            }

            if (x == x6)
            {
                if (y == y6)
                {
                    life = life - 1;
                }
            }

            if (x == x7)
            {
                if (y == y7)
                {
                    life = life - 1;
                }
            }

            if (x == x8)
            {
                if (y == y8)
                {
                    life = life - 1;
                }
            }

            if (x == x9)
            {
                if (y == y9)
                {
                    life = life - 1;
                }
            }
        }

        //me touch the targets
        static void touchT(int[] ArrayX, int[] ArrayY, int NumTimes, ref int x, ref int y, ref int points)
        {
            int i;

            for (i = 0; i < NumTimes; i++)
            {
                if (ArrayX[i] == x)
                {
                    if (ArrayY[i] == y)
                    {
                        points = points + 50;
                    }
                }
            }

        }
    }
}
