using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card_Project
{
    //Dipesh Patel check my code
    class Program
    {
        static void Main(string[] args)
        {
            int choice = 0;
            const int MIN = 1;
            const int MAX_INPUT_VALUE = 4;

            string question = string.Empty;        
            question = "Please enter your desired option: ";

            

            while (choice != MAX_INPUT_VALUE)
            {
                PrintTitle();

                Console.WriteLine("1. Play");
                Console.WriteLine("2. Automatic Testing");
                Console.WriteLine("3. Manual Testing");
                Console.WriteLine("4. Exit");

                choice = Validation(question, MAX_INPUT_VALUE, MIN);
                switch (choice)
                {
                    case 1:
                        Play();
                        break;
                    case 2:
                        AutomaticTesting();
                        break;
                    case 3:
                        ManualTesting();
                        break;
                    case 4:
                        break;
                }
            }
            Console.WriteLine("Press on any key to continue...");
            Console.ReadKey();
        }

        /*PrintTitle
         * Input: none
         * Output: none
         * This method prints out the title.
         */
        public static void PrintTitle()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("{0} {1,32} {2,20}", "|", "Combine & Win", "|");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        /* Validation
         * Input: string message, get the question to be ask to the user
         * Output: int input, returns the number the user enters
         * The method Validation receive the question and display it. Then it checks the user input.
         * If the input is not valid it returns an error and ask the user for another input until the input
         * is correct. Finally, it return the validate input.*/
        public static int Validation(string message, int MAX, int MIN)
        {
            bool valid;
            int input;

            Console.WriteLine(message);
            valid = int.TryParse(Console.ReadLine(), out input);

            while(!valid || input < MIN || input > MAX)
            {
                Console.WriteLine("Error! Please choose one of the following. \n " + message);
                valid = int.TryParse(Console.ReadLine(), out input);
            }

            return input;
        }

        /*Play
         * Input: none
         * Output: none
         * The method Play allows the user to play the game. It calls all the method needed to play the game.*/
        public static void Play()
        {
            const int MIN = 0;
            const int CARDS_IN_HAND = 5;

            Deck deck = new Deck();
            Card[] hand = new Card[CARDS_IN_HAND];
            int points = 1000;
            int addOrLossPoints = 0;
            ConsoleKey choice;
            string combination = string.Empty;

            Console.Clear();
            PrintTitle();

            while (points > MIN)
            {
                deck.Shuffle();
                addOrLossPoints = 0;
                deck.Deal(hand);
                SortingCard(hand);

                Console.WriteLine("Game Score: {0} points", points);

                Console.WriteLine();
                PrintHand(hand);
                Console.WriteLine();

                DiscardingRound(hand, deck);

                combination = WinningValidation(hand, ref addOrLossPoints);

                PrintResult(points, addOrLossPoints, combination);

                points += addOrLossPoints;

                if (points > MIN)
                {
                    Console.WriteLine("Press any key to play again, or 'N' to quit: ");
                    choice = Console.ReadKey().Key;

                    if (choice == ConsoleKey.N)
                    {
                        break;
                    }
                }
            }

            Console.WriteLine("Thank you for playing! Have a nice day!");
            Console.ReadKey();
            Console.Clear();
        }

        /*PrintHand
         * Intput: Card[] hand, get a array filled with 5 cards
         * Output: none
         * The method PrintHand takes the array and display each card in the array on the console.
         */
        public static void PrintHand(Card[] hand)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                Console.Write("[{0}] ", (i + 1));
                Console.WriteLine(hand[i].ToString());
            }
        }

        /*SortingCard
         * Intput: Card[] hand, get a array filled with 5 cards
         * Output: none
         * The method SortingCard sort all the cards in the array of card according to the value 
         * of the card.
         */
        public static void SortingCard(Card[] hand)
        {
            Card card;

            for (int write = 0; write < hand.Length; write++)
            {
                for (int sort = 0; sort < hand.Length - 1; sort++)
                {
                    if (hand[sort].Value > hand[sort + 1].Value)
                    {
                        card = hand[sort + 1];
                        hand[sort + 1] = hand[sort];
                        hand[sort] = card;
                    }
                }
            }
        }

        /*DiscardingRound
         * Input: Card[] hand, Card[] deck, get the deck of cards and the hand
         * Output: none
         * The method
         */
        public static void DiscardingRound(Card[] hand, Deck deck)
        {
            int input = 1;
            int counterCardDiscarded = 0;
            int counterRound = 1;
            int counterArray = 0;
            int min = 0;
            string question = string.Empty;

            const int STOP_DISCARD_NUMBER = 0;
            const int MAX_ROUND = 3;
            const int MAX_DISCARD = 4;
            const int MAX_INPUT_VALUE = 5;
            
            int[] previousDiscardedCard = new int[] { 11, 11, 11 };

            //rounds of discarding
            while (counterRound <= MAX_ROUND)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Discarding Round {0}", counterRound);
                Console.ResetColor();


                //numbers of cards discarding
                while (counterCardDiscarded < MAX_DISCARD)
                {
                    min = 0;
                    question = "Choose the card to replace, or 0 to continue: ";
                    input = Validation(question, MAX_INPUT_VALUE, min);
                    if (input == STOP_DISCARD_NUMBER)
                    {
                        break;
                    }
                    while (previousDiscardedCard[counterArray] == input)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Card already discarded...");
                        Console.ResetColor();
                        min = 0;
                        input = Validation(question, MAX_INPUT_VALUE, min);
                    }
                    if (input == STOP_DISCARD_NUMBER)
                    {
                        break;
                    }
                    Discard(hand, deck, input);
                    if (counterCardDiscarded > STOP_DISCARD_NUMBER && counterCardDiscarded != MAX_ROUND)
                    {
                        counterArray++;
                    }
                    
                    previousDiscardedCard[counterArray] = input;
                    counterCardDiscarded++;

                }
                Console.Clear();

                PrintTitle();

                SortingCard(hand);
                if (counterCardDiscarded == STOP_DISCARD_NUMBER && input == STOP_DISCARD_NUMBER)
                {
                    break;
                }
                Console.WriteLine();
                PrintHand(hand);
                Console.WriteLine();
                counterCardDiscarded = 0;
                counterRound++;
                counterArray = 0;

                //reset the array
                previousDiscardedCard[0] = 11;
                previousDiscardedCard[1] = 11;
                previousDiscardedCard[2] = 11;

                
            }
        }

        /*Discard
         * Input: Card[] hand, Deck deck, int choice, get the array of card for the hand, the deck of card and which card chosen by the user
         * Output: none
         * The method Discard takes which card the user want to discard and replace it with a new card from the deck.
         */
        public static void Discard(Card[] hand, Deck deck, int choice)
        {
            int exit = 0;
            try
            {
                if (choice == exit)
                {
                    return;
                }
                choice -= 1;
                hand[choice] = deck.NextCard;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Card successfully discarded and replaced...");
                Console.ResetColor();
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Nothing entered. Try again:");
                Console.ResetColor();
            }            
        }

        /*WinningValidation
         * Input: Card[] hand, ref int points, get the cards from the hand and the points that 
         * will be added to the total points
         * Output: string combination, return the name of the winning combination or nothing 
         * if its a losing combination
         * The method WinningValidation receive the hand and check with each winning combination. 
         * The name of the winning combination and the number of points gains will be return if 
         * there is a winning combination. If there are no winning combination, it will return
         * an empty string and the amount of deducted points.
         */
        public static string WinningValidation(Card[] hand, ref int points)
        {
            bool win = false;
            string combination = string.Empty;

            combination = "High-Five";
            win = HighFive(hand, ref points);

            if (!win)
            {
                combination = "Sequence";
                win = Sequence(hand, ref points);
                if (!win)
                {
                    combination = "Quadruplets";
                    win = Quadruplets(hand, ref points);
                    if (!win)
                    {
                        combination = "Family";
                        win = Family(hand, ref points);
                        if (!win)
                        {
                            combination = "Mixed Sequence";
                            win = MixedSequence(hand, ref points);
                            if (!win)
                            {
                                combination = "Double Twins";
                                win = DoubleTwins(hand, ref points);
                            } 
                        }  
                    } 
                }
            }

            if (win == false)
            {
                points = -100;
                combination = "";
            }

            return combination;
        }

        /*HighFive
         * Input: Card[] hand, int points, get the array with the hand and the number of points the player has
         * Output: return a bool indicating if the player won or lost
         * The method HighFive receive the array of card that the user has and compared the suit and the value of 
         * each card to the winning combination of High-Five. If the hand match with the winning combination, the method
         * will return a bool indicated the player won and the number  of points gain. To have a winning combination, 
         * all the cards in the hand need to be of the same suit and need to contains a Ten, a Jack, a Queen, a King and a Ace.
         */
        public static bool HighFive(Card[] hand, ref int points)
        {
            Suits suits = hand[0].Suits;
            const int winningPoints = 800;

            if (hand[1].Value == Value.Ten && hand[1].Suits == suits)
            {
                if (hand[2].Value == Value.Jack && hand[2].Suits == suits)
                {
                    if (hand[3].Value == Value.Queen && hand[3].Suits == suits)
                    {
                        if (hand[4].Value == Value.King && hand[4].Suits == suits)
                        {
                            if (hand[0].Value == Value.Ace && hand[0].Suits == suits)
                            {
                                points = winningPoints;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /*Sequence
         * Input: Card[] hand, int points, get the array with the hand and the number of points the player has
         * Output: return a bool indicating if the player won or lost
         * The method Sequence receive the array of card that the user has and compare the suit and the value of 
         * each card to the winning combination of Sequence. If the hand match with the winning combination, the method
         * will return a bool indicated the player won and the number of points gain. To have a winning combination, 
         * all the cards in the hand need to be of the same suit and need to show consecutive numbers.
         */
        public static bool Sequence(Card[] hand, ref int points)
        {
            Suits suits = hand[0].Suits;
            Value value = hand[0].Value;
            const int winningPoints = 600;

            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i].Suits != suits || hand[i].Value != value)
                {
                    return false;
                }
                value++;
                if (value > Value.King)
                {
                    return false;
                }
            }
            points = winningPoints;
            return true;
        }

        /*Quadruplets
         *Input: Card[] hand, int points, get the array with the hand and the number of points the player has
         * Output: return a bool indicating if the player won or lost
         * The method Sequence receive the array of card that the user has and compare the suit and the value of 
         * each card to the winning combination of Sequence. If the hand match with the winning combination, the method
         * will return a bool indicated the player won and the number of points gain. To have a winning combination, 
         * the hand needs to have a card with a value that repeats four times and any other card.
         */
        public static bool Quadruplets(Card[] hand, ref int points)
        {
            int cardsToCheck = 3;
            Value value = hand[0].Value;
            bool winning = false;
            const int winningPoints = 400;

            for (int i = 1; i == cardsToCheck; i++)
            {
                if (hand[i].Value !=  value)
                {
                    break;
                }
                else
                {
                    winning = true;
                }
            }

            if (winning == false)
            {
                value = hand[1].Value;
                for (int i = 2; i < cardsToCheck; i++)
                {
                    if (hand[i].Value != value)
                    {
                        return false;
                    }
                }
                points = winningPoints;
                return true;
            }
            else
            {
                points = winningPoints;
                return true;
            }
        }

        /*Family
         *Input: Card[] hand, int points, get the array with the hand and the number of points the player has
         * Output: return a bool indicating if the player won or lost
         * The method Sequence receive the array of card that the user has and compare the suit and the value of 
         * each card to the winning combination of Sequence. If the hand match with the winning combination, the method
         * will return a bool indicated the player won and the number of points gain. To have a winning combination, 
         * all the cards in the hand need to be of the same suit.
         */
        public static bool Family(Card[] hand, ref int points)
        {
            Suits suits = hand[0].Suits;
            const int winningPoints = 200;

            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i].Suits != suits)
                {
                    return false;
                }
            }
            points = winningPoints;
            return true;
        }

        /*MixedSequence
         *Input: Card[] hand, int points, get the array with the hand and the number of points the player has
         * Output: return a bool indicating if the player won or lost
         * The method Sequence receive the array of card that the user has and compare the suit and the value of 
         * each card to the winning combination of Sequence. If the hand match with the winning combination, the method
         * will return a bool indicated the player won and the number of points gain. To have a winning combination, 
         * all the cards in the hand need to be in consecutive number.
         */
        public static bool MixedSequence(Card[] hand, ref int points)
        {
            Value value = hand[0].Value;
            const int winningPoints = 100;

            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i].Value != value)
                {
                    return false;
                }
                value++;
                if (value > Value.King)
                {
                    return false;
                }
            }
            points = winningPoints;
            return true;
        }

        /*DoubleTwins
         *Input: Card[] hand, int points, get the array with the hand and the number of points the player has
         * Output: return a bool indicating if the player won or lost
         * The method Sequence receive the array of card that the user has and compare the suit and the value of 
         * each card to the winning combination of Sequence. If the hand match with the winning combination, the method
         * will return a bool indicated the player won and the number of points gain. To have a winning combination, 
         * the hand need to contains two cards showing the same numbers and another two cards showing the same numbers 
         * (but not al four numbers are the same) plus any other card.
         */
        public static bool DoubleTwins(Card[] hand, ref int points)
        {
            const int winningPoints = 50;

            if (hand[1].Value == hand[0].Value)
            {
                if (hand[3].Value == hand[2].Value)
                {
                    points = winningPoints;
                    return true;
                }
            }

            if (hand[2].Value == hand[1].Value)
            {
                if (hand[4].Value == hand[3].Value)
                {
                    points = winningPoints;
                    return true;
                }
            }

            if (hand[1].Value == hand[0].Value)
            {
                if (hand[4].Value == hand[3].Value)
                {
                    points = winningPoints;
                    return true;
                }
            }

            return false;
        }

        /*PrintResult
         * Input: int totalPoints, int points, string combination, get the number of points the player has and 
         * the winning combination to be display
         * Output: none
         * The method PrintResult get the player total points, the amount of points they gain or lost in that round and the 
         * winning combination (if there is any) and print it out on the console.
         */
        public static void PrintResult(int totalPoints, int points, string combination)
        {
            int lossingPoints = -100;

            Console.WriteLine();

            totalPoints += points;
            if (points == lossingPoints)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(">>> No winning combination" + ":" + "{0} points. <<<", points);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(">>> Winning Combination - {0} : +{1} points. <<<", combination, points);
                Console.ResetColor();
            }
            Console.WriteLine(">>> Game Score : {0} points. <<<", totalPoints);
        }

        /*AutomaticTesting
         * Input: none
         * Output: none
         * The method AutomaticTesting give the user a list of all the winning combination to be tested.
         */
        public static void AutomaticTesting()
        {
            string question = "1. High Five \n2. Sequence \n3. Quadruplets \n4. Family \n5. Mixed Sequence \n6. Double Twins \n7. QUIT \n\nPlease enter you desired option: ";
            int choice = 0;
            const int MAX = 7;
            const int MIN = 1;

            Console.WriteLine(">>> Automatic Testing");
            Console.WriteLine();

            while (choice != MAX)
            {
                Console.WriteLine();
                choice = Validation(question, MAX, MIN);

                switch (choice)
                {
                    case 1:
                        AutoTestHighFive();
                        break;
                    case 2:
                        AutoTestSequence();
                        break;
                    case 3:
                        AutoTestQuadruplets();
                        break;
                    case 4:
                        AutoTestFamily();
                        break;
                    case 5:
                        AutoTestMixedSequence();
                        break;
                    case 6:
                        AutoTestDoubleTwins();
                        break;
                    case 7:
                        break;
                }
            }
            Console.Clear();
        }

        /*AutoTestHighFive
         * Input: none
         * Output: none
         * The method AutoTestingHighFive check if the HighFive method work properly.
         */
        public static void AutoTestHighFive()
        {
            const int NUMBER_CARD_HAND = 5;
            Card[] hand = new Card[NUMBER_CARD_HAND];
            Suits suits = Suits.Diamonds;
            Value value = Value.Ten;
            int points = 0;
            int totalPoints = 0;
            string combination = string.Empty;

            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = new Card(suits, value);
                value++;
                if (value > Value.King)
                {
                    value = Value.Ace;
                }
            }

            SortingCard(hand);

            PrintHand(hand);

            combination = WinningValidation(hand, ref points);

            PrintResult(totalPoints, points, combination);
        }

        /*AutoTestSequence
         * Input: none
         * Output: none
         * The method AutoTestSequence check if the Sequence method work properly.
         */
        public static void AutoTestSequence()
        {
            const int NUMBER_CARD_HAND = 5;
            Card[] hand = new Card[NUMBER_CARD_HAND];
            Suits suits = Suits.Diamonds;
            Value value = Value.Five;
            int points = 0;
            int totalPoints = 0;
            string combination = string.Empty;

            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = new Card(suits, value);
                value++;
            }

            PrintHand(hand);

            combination = WinningValidation(hand, ref points);

            PrintResult(totalPoints, points, combination);
        }

        /*AutoTestQuadruplets
         * Input: none
         * Output: none
         * The method AutoTestQuadruplets check if the Quadruplets method work properly.
         */
        public static void AutoTestQuadruplets()
        {
            const int NUMBER_CARD_HAND = 5;
            Card[] hand = new Card[NUMBER_CARD_HAND];
            int points = 0;
            int totalPoints = 0;
            string combination = string.Empty;

            hand[0] = new Card(Suits.Diamonds,Value.Ace);
            hand[1] = new Card(Suits.Clubs, Value.Ace);
            hand[2] = new Card(Suits.Hearts, Value.Ace);
            hand[3] = new Card(Suits.Spades, Value.Ace);
            hand[4] = new Card(Suits.Hearts, Value.Five);

            PrintHand(hand);

            combination = WinningValidation(hand, ref points);

            PrintResult(totalPoints, points, combination);
        }

        /*AutoTestFamily
         * Input: none
         * Output: none
         * The method AutoTestFamily check if the Family method work properly.
         */
        public static void AutoTestFamily()
        {
            const int NUMBER_CARD_HAND = 5;
            Card[] hand = new Card[NUMBER_CARD_HAND];
            Suits suits = Suits.Diamonds;
            Value value = Value.Ten;
            int points = 0;
            int totalPoints = 0;
            string combination = string.Empty;

            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = new Card(suits, value);
                value++;
                if (value == Value.King)
                {
                    value = Value.Ace;
                }
            }

            SortingCard(hand);
            PrintHand(hand);

            combination = WinningValidation(hand, ref points);

            PrintResult(totalPoints, points, combination);
        }

        /*AutoTestMixedSequence
         * Input: none
         * Output: none
         * The method AutoTestMixedSequence check if the MixedSequence method work properly.
         */
        public static void AutoTestMixedSequence()
        {
            const int NUMBER_CARD_HAND = 5;
            Card[] hand = new Card[NUMBER_CARD_HAND];
            Suits suits = Suits.Diamonds;
            Value value = Value.Ten;
            int points = 0;
            int totalPoints = 0;
            string combination = string.Empty;

            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = new Card(suits, value);
                value++;
                if (value == Value.King)
                {
                    value = Value.Ace;
                }
            }

            PrintHand(hand);

            combination = WinningValidation(hand, ref points);

            PrintResult(totalPoints, points, combination);
        }

        /*AutoTestDoubleTwins
         * Input: none
         * Output: none
         * The method AutoTestingDoubleTwins check if the DoubleTwins method work properly.
         */
        public static void AutoTestDoubleTwins()
        {
            const int NUMBER_CARD_HAND = 5;
            Card[] hand = new Card[NUMBER_CARD_HAND];
            Suits suits = Suits.Diamonds;
            Value value = Value.Ten;
            int points = 0;
            int totalPoints = 0;
            string combination = string.Empty;

            hand[0] = new Card(suits, value);
            hand[1] = new Card(suits = Suits.Clubs, value);
            hand[2] = new Card(suits = Suits.Hearts, value = Value.Ace);
            hand[3] = new Card(suits = Suits.Spades, value = Value.Ace);
            hand[4] = new Card(suits, value = Value.Eight);

            PrintHand(hand);

            combination = WinningValidation(hand, ref points);

            PrintResult(totalPoints, points, combination);
        }

        /*ManualTesting
         * Input: none
         * Output: none
         * The method ManualTesting ask the user to chose which card they want in the hand and validate it the 
         * hand is a winning combination.
         */
        public static void ManualTesting()
        {
            Console.WriteLine(">>> Pick your hand:");
            Console.WriteLine();

            const int MIN = 1;
            const int lossingPoints = -100;
            const int NUMBER_CARD_HAND = 5;

            Card[] hand = new Card[NUMBER_CARD_HAND];
            string question = "Pick a suit: \n[1] Heart | [2] Diamond | [3] Club | [4] Spade | >>> ";
            string winningCombination = string.Empty;
            int choice;
            int MAX = 4;
            int points = 0;
            int j = 0;

            Suits suits = Suits.Clubs;
            Value value = Value.Ace;

            for (int i = 0; i < hand.Length; i++)
            {
                j = i + 1;
                Console.WriteLine("Pick card #{0}", j);
                choice = Validation(question, MAX, MIN);

                switch (choice)
                {
                    case 1:
                        suits = Suits.Hearts;
                        break;
                    case 2:
                        suits = Suits.Diamonds;
                        break;
                    case 3:
                        suits = Suits.Clubs;
                        break;
                    case 4:
                        suits = Suits.Spades;
                        break;
                }

                MAX = 13;
                question = "Pick a value: \n[1] Ace \n[2] Two \n[3] Three \n[4] Four \n[5] Five \n[6] Six \n[7] Seven \n[8] Eight \n[9] Nine \n[10] Ten \n[11] Jack \n[12] Queen \n[13] King \n>>>";
                choice = Validation(question, MAX, MIN);
                switch (choice)
                {
                    case 1:
                        value = Value.Ace;
                        break;
                    case 2:
                        value = Value.Two;
                        break;
                    case 3:
                        value = Value.Three;
                        break;
                    case 4:
                        value = Value.Four;
                        break;
                    case 5:
                        value = Value.Five;
                        break;
                    case 6:
                        value = Value.Six;
                        break;
                    case 7:
                        value = Value.Seven;
                        break;
                    case 8:
                        value = Value.Eight;
                        break;
                    case 9:
                        value = Value.Nine;
                        break;
                    case 10:
                        value = Value.Ten;
                        break;
                    case 11:
                        value = Value.Jack;
                        break;
                    case 12:
                        value = Value.Queen;
                        break;
                    case 13:
                        value = Value.King;
                        break;
                }

                hand[i] = new Card(suits, value);

                Console.WriteLine("Chosen card: {0}", hand[i].ToString());
                Console.WriteLine();
            }

            PrintHand(hand);

            winningCombination = WinningValidation(hand, ref points);

            if (points == lossingPoints)
            {
                Console.WriteLine("No winning combination {0} points. <<<", points);
            }
            else
            {
                Console.WriteLine("Winning Combination - {0} +{1} points. <<<", winningCombination, points);
            }

            Console.WriteLine("Press any key to go back to the main menu");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
