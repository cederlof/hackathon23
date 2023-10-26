using System;
using System.Collections.Generic;
namespace Blackjack
{
    class Program
    {
            static void Main(string[] args)
            {
                Console.WriteLine("Welcome to Blackjack!");
                bool playAgain = true;
                while (playAgain)
                {
                    PlayGame();
                    Console.WriteLine("Do you want to play again? (yes/no): ");
                    string playAgainInput = Console.ReadLine().ToLower();
                    playAgain = (playAgainInput == "yes" || playAgainInput == "y");
                }
                Console.WriteLine("Thanks for playing!");
                Console.ReadLine();
            }
            static void PlayGame()
            {
                // Create a new deck and shuffle it
                Deck deck = new Deck();
                deck.Shuffle();
                // Create player and dealer hands
                Hand playerHand = new Hand();
                Hand dealerHand = new Hand();
                // Deal initial cards
                playerHand.AddCard(deck.DrawCard());
                dealerHand.AddCard(deck.DrawCard());
                playerHand.AddCard(deck.DrawCard());
                dealerHand.AddCard(deck.DrawCard());
                // Play the game
                bool gameOver = false;
                while (!gameOver)
                {
                    // Show player's hand and ask for action
                    Console.WriteLine("Your cards: ");
                    foreach (Card card in playerHand.Cards)
                    {
                        Console.WriteLine(card.ToString());
                    }
                    Console.WriteLine("Total value: " + playerHand.GetValue());
                    Console.WriteLine("Do you want to hit or stand? ('h' for hit, 's' for stand): ");
                    string action = Console.ReadLine();
                    if (action.ToLower() == "h")
                    {
                        // Player wants to hit, draw a card
                        Card card = deck.DrawCard();
                        playerHand.AddCard(card);
                        Console.WriteLine("You drew: " + card.ToString());
                        if (playerHand.GetValue() > 21)
                        {
                            gameOver = true;
                            Console.WriteLine("Player busts! Dealer wins.");
                        }
                    }
                    else if (action.ToLower() == "s")
                    {
                        // Player wants to stand, dealer's turn
                        gameOver = true;
                        // Dealer draws cards until their hand value is at least 17
                        while (dealerHand.GetValue() < 17)
                        {
                            Card card = deck.DrawCard();
                            dealerHand.AddCard(card);
                        }
                        Console.WriteLine("Dealer's hand: ");
                        foreach (Card card in dealerHand.Cards)
                        {
                            Console.WriteLine(card.ToString());
                        }
                        Console.WriteLine("Dealer's total value: " + dealerHand.GetValue());
                        if (dealerHand.GetValue() > 21)
                        {
                            Console.WriteLine("Dealer busts! Player wins.");
                        }
                        else if (playerHand.GetValue() > dealerHand.GetValue())
                        {
                            Console.WriteLine("Player wins.");
                        }
                        else if (playerHand.GetValue() < dealerHand.GetValue())
                        {
                            Console.WriteLine("Dealer wins.");
                        }
                        else
                        {
                            Console.WriteLine("It's a tie!");
                        }
                    }
                }
            }
        }





    //static void Main(string[] args)
    //    {
    //        Console.WriteLine("Welcome to Blackjack!");
    //        // Create a new deck and shuffle it
    //        Deck deck = new Deck();
    //        deck.Shuffle();
    //        // Create player and dealer hands
    //        Hand playerHand = new Hand();
    //        Hand dealerHand = new Hand();
    //        // Deal initial cards
    //        playerHand.AddCard(deck.DrawCard());
    //        dealerHand.AddCard(deck.DrawCard());
    //        playerHand.AddCard(deck.DrawCard());
    //        dealerHand.AddCard(deck.DrawCard());
    //        // Play the game
    //        bool gameOver = false;
    //        while (!gameOver)
    //        {
    //            // Show player's hand and ask for action
    //            Console.WriteLine("Your cards: ");
    //            foreach (Card card in playerHand.Cards)
    //            {
    //                Console.WriteLine(card.ToString());
    //            }
    //            Console.WriteLine("Total value: " + playerHand.GetValue());
    //            Console.WriteLine("Do you want to hit or stand? ('h' for hit, 's' for stand): ");
    //            string action = Console.ReadLine();
    //            if (action.ToLower() == "h")
    //            {
    //                // Player wants to hit, draw a card
    //                Card card = deck.DrawCard();
    //                playerHand.AddCard(card);
    //                Console.WriteLine("You drew: " + card.ToString());
    //            }
    //            else if (action.ToLower() == "s")
    //            {
    //                // Player wants to stand, dealer's turn
    //                gameOver = true;
    //                // Dealer draws cards until their hand value is at least 17
    //                while (dealerHand.GetValue() < 17)
    //                {
    //                    Card card = deck.DrawCard();
    //                    dealerHand.AddCard(card);
    //                }
    //            }
    //            // Check if player or dealer has gone bust
    //            if (playerHand.GetValue() > 21)
    //            {
    //                gameOver = true;
    //                Console.WriteLine("Player busts! Dealer wins.");
    //            }
    //            else if (dealerHand.GetValue() > 21)
    //            {
    //                gameOver = true;
    //                Console.WriteLine("Dealer busts! Player wins.");
    //            }
    //        }
    //        // Show the final hands
    //        Console.WriteLine("Player's hand: ");
    //        foreach (Card card in playerHand.Cards)
    //        {
    //            Console.WriteLine(card.ToString());
    //        }
    //        Console.WriteLine("Player's total value: " + playerHand.GetValue());
    //        Console.WriteLine();
    //        Console.WriteLine("Dealer's hand: ");
    //        foreach (Card card in dealerHand.Cards)
    //        {
    //            Console.WriteLine(card.ToString());
    //        }
    //        Console.WriteLine("Dealer's total value: " + dealerHand.GetValue());
    //        Console.ReadLine();
    //    }
    //}
    class Card
    {
        public enum Suit { Clubs, Diamonds, Hearts, Spades }
        public enum FaceValue { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }
        public Suit CardSuit { get; set; }
        public FaceValue CardFaceValue { get; set; }
        public Card(Suit suit, FaceValue faceValue)
        {
            CardSuit = suit;
            CardFaceValue = faceValue;
        }
        public int GetValue()
        {
            if (CardFaceValue == FaceValue.Ace)
            {
                return 11; // An Ace can be worth 11 or 1, we assume 11 here
            }
            else if (CardFaceValue >= FaceValue.Ten)
            {
                return 10; // All face cards (Jack, Queen, King) have a value of 10
            }
            else
            {
                return (int)CardFaceValue; // Numeric cards have a value equal to their face value
            }
        }
        public override string ToString()
        {
            return CardFaceValue + " of " + CardSuit;
        }
    }
    class Deck
    {
        private List<Card> cards;
        public Deck()
        {
            cards = new List<Card>();
            foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
            {
                foreach (Card.FaceValue faceValue in Enum.GetValues(typeof(Card.FaceValue)))
                {
                    cards.Add(new Card(suit, faceValue));
                }
            }
        }
        public void Shuffle()
        {
            Random rnd = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int randomIndex = rnd.Next(n + 1);
                Card card = cards[randomIndex];
                cards[randomIndex] = cards[n];
                cards[n] = card;
            }
        }
        public Card DrawCard()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }
    class Hand
    {
        public List<Card> Cards { get; set; }
        public Hand()
        {
            Cards = new List<Card>();
        }
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
        public int GetValue()
        {
            int value = 0;
            bool hasAce = false;
            // Calculate the total value of the hand, taking into account possible Aces
            foreach (Card card in Cards)
            {
                value += card.GetValue();
                if (card.CardFaceValue == Card.FaceValue.Ace)
                {
                    hasAce = true;
                }
            }
            // If the hand has an Ace and the total value is over 21, adjust the value of the Ace to 1
            if (hasAce && value > 21)
            {
                value -= 10;
            }
            return value;
        }
    }
}
