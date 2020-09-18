using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card_Project
{
    class Deck
    {
        //field
        private const int NUMBER_CARD_IN_DECK = 52;
        private const int NUMB_SUITS = 4;
        private const int NUMB_VALUE = 13;

        private int theDealCardInDeck = 0;
        private Card[] deck = new Card[NUMBER_CARD_IN_DECK];
        private string printDeck = string.Empty;

        //deals the next card
        public Card NextCard
        {
            get
            {
                Card c = deck[theDealCardInDeck];
                theDealCardInDeck++;
                return c;
            }
        }

        //constructor
        public Deck()
        {
            Suits suits = Suits.Clubs;
            Value value = Value.Ace;
            int deckCounter = 0;

            for (int s = 0; s < NUMB_SUITS; s++)
            {
                for (int v = 0; v < NUMB_VALUE; v++)
                {
                    Card card = new Card(suits, value);
                    value++;
                    deck[deckCounter] = card;
                    deckCounter++;
                }
                value = Value.Ace;
                suits++;
            }
            Shuffle();
        }

        //print the deck
        public override string ToString()
        {
            printDeck = string.Empty;
            
            for (int i = 0; i < NUMBER_CARD_IN_DECK; i++)
            {
                Card card = deck[i];
                printDeck += card.ToString() + "\n";
            }
            

            return String.Format(printDeck);
        }

        //mix the cards around in the deck
        public void Shuffle()
        {
            const int MIN = 1;
            const int NUMBER_OF_TIME_SHUFFLE = 45;

            Random random = new Random();
            int swap = random.Next(NUMBER_CARD_IN_DECK);

            for (int i = 0; i < NUMBER_OF_TIME_SHUFFLE; i++)
            {
                Card card = deck[i];
                deck[i] = deck[swap];
                deck[swap] = card;
                swap = random.Next(MIN, NUMBER_CARD_IN_DECK);
            }
            theDealCardInDeck = 0;
        }

        //deal the hand for the player
        public void Deal(Card[] hand)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = deck[i];
                theDealCardInDeck++;
            }
        }
    }
}
