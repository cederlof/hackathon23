# BlackJack

## Fråga till AI

give me code for a c# console app. i want to implement a black jack game where i as a user get handed cards and can take action.

## Svar

```
using System;
using System.Collections.Generic;
namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Blackjack!");
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
                }
                // Check if player or dealer has gone bust
                if (playerHand.GetValue() > 21)
                {
                    gameOver = true;
                    Console.WriteLine("Player busts! Dealer wins.");
                }
                else if (dealerHand.GetValue() > 21)
                {
                    gameOver = true;
                    Console.WriteLine("Dealer busts! Player wins.");
                }
            }
            // Show the final hands
            Console.WriteLine("Player's hand: ");
            foreach (Card card in playerHand.Cards)
            {
                Console.WriteLine(card.ToString());
            }
            Console.WriteLine("Player's total value: " + playerHand.GetValue());
            Console.WriteLine();
            Console.WriteLine("Dealer's hand: ");
            foreach (Card card in dealerHand.Cards)
            {
                Console.WriteLine(card.ToString());
            }
            Console.WriteLine("Dealer's total value: " + dealerHand.GetValue());
            Console.ReadLine();
        }
    }
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
```


## Fråga till AI

when game is over, ask if i want to play again and restart the game if so

## Svar

```
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
    // Rest of the code remains the same
    // ...
}
```

## Fråga till AI

what can i do to make this game have more functionality

## Svar

To add more functionality to the game, here are some ideas:

1. Betting System: Allow the player to place bets on each round. Keep track of their balance and adjust it based on the outcome of each round.

2. Splitting Pairs: Implement the ability for the player to split their hand if they receive a pair of cards with the same face value. This will allow them to play two separate hands and potentially increase their chances of winning.

3. Doubling Down: Add the option for the player to double their initial bet after receiving their first two cards. In return, they will receive only one more card and then automatically stand.

4. Insurance Bet: Provide the player with the option to make an insurance bet if the dealer's face-up card is an Ace. This bet protects the player's initial bet in case the dealer has a blackjack.

5. Multiple Decks: Modify the deck class to support multiple decks of cards. This will make the game more realistic, as blackjack is typically played with multiple decks.

6. Splitting Aces: Allow the player to split their hand even if they receive a pair of Aces. In this case, they will receive only one more card for each Ace and then automatically stand.

7. Surrender: Implement the surrender option, where the player can choose to surrender and forfeit half of their bet instead of playing out the hand.

8. Graphics: Enhance the game by adding graphical elements and animations. You can use libraries like WinForms or WPF to create a visually appealing user interface.

These are just a few ideas to add more functionality to your blackjack game. Feel free to choose one or combine multiple ideas to make your game more interesting and challenging.