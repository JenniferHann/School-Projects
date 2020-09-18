using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card_Project
{
    class Card
    {
        //fields
        private Suits _suits;
        private Value _value;

        //constructor
        public Card(Suits suits, Value value)
        {
            _suits = suits;
            _value = value;
        }

        //propertie for suits of the card
        public Suits Suits
        {
            get
            {
                return _suits;
            }
        }

        //propertie for value of the card
        public Value Value
        {
            get
            {
                return _value;
            }
        }

        //print the card
        public override string ToString()
        {
            return String.Format(_value + " of " + _suits);
        }
    }

    //Enum to give the suits to the card
    public enum Suits { Clubs = 1, Diamonds, Hearts, Spades };
    //Enum to give the suits to the card
    public enum Value { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
}
