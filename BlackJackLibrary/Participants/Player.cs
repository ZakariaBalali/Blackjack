using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BlackJackLibrary.CardDeck;
using UI.UI;

namespace BlackJackLibrary.Participants
{
    public class Player : Participant
    {
        private int _balance; 
        public int Balance { get => _balance; set => _balance = value; } // Player's current balance
        public int BetAmount { get; internal set; } // Amount the player has bet for the round
        public bool HasMoreHandsToPlay => currentHandIndex + 1 < hands.Count; // Checks if player can play more hands

        private readonly IGameUI ui;

        public Player(IGameUI ui, int startingBalance = 20)
        {
            this.ui = ui;
            Balance = startingBalance;
        }
        
        // Balance methods
        public void ReduceBalance()
        {
            Balance -= BetAmount;
        }

        // Adds winnings to balance based on blackjack or regular win
        public void AddToBalance(bool isBlackjack)
        {
            Balance += (int)(BetAmount * (isBlackjack ? 1.5 : 1));
        }

        // Extra play options for the player (double down and split pairs)
        internal void DoubleDown(Shoe shoe)
        {
            BetAmount *= 2;
            Hit(shoe);
            Stand();
        }

        internal void SplitPairs(Shoe shoe)
        {
            var hand = CurrentHand;

            // Take second card from current hand
            Card secondCard = hand.CardsInHand[1];
            hand.CardsInHand.RemoveAt(1);

            // Add a new card to current hand
            Card cardForCurrentHand = shoe.TakeCard();
            hand.AddCard(cardForCurrentHand);

            // Create new hand, add card taken from current hand and adds a new card taken from the shoe
            Card cardForNewHand = shoe.TakeCard();
            Hand newHand = new();
            newHand.AddCard(secondCard);
            newHand.AddCard(cardForNewHand);

            // Add to hands list and update values
            hands.Add(newHand);
            hand.UpdateCardsValue();
        }

        // Checks if a hand is a blackjack (exactly two cards, total 21)
        public bool IsBlackJack(int handIndex)
        {
            var hand = hands[handIndex];
            return hand.CardsValue == 21 && hand.CardsInHand.Count == 2;
        }


        // Checks if you can advance to a next hand and then does so
        public bool AdvanceToNextHand()
        {
            if (HasMoreHandsToPlay)
            {
                currentHandIndex++;
                return true;
            }
            return false;
        }

        // Prompts the user to place a bet
        public void PlaceBet()
        {
            ui.ShowPlaceBetPrompt();
            GetValidBetInput();
        }

        // Handles input validation for the bet amount
        private void GetValidBetInput()
        {
            while (true)
            {
                string input = ui.GetBetInput().Trim();

                if (int.TryParse(input, out int betAmount) &&
                    betAmount > 0 &&
                    betAmount <= 10 &&
                    betAmount <= Balance)
                {
                    BetAmount = betAmount;
                    break;
                }

                ui.ShowInvalidBet();
            }
        }

        // Lets the player choose an action: Hit, Stand, Double Down, or Split
        public virtual void ChoosePlayOption(Shoe shoe)
        {
            while (true)
            {
                List<Card> hand = CurrentHand.CardsInHand;
                int handSize = hand.Count;

                string input = ui.GetPlayOptionInput(handSize);

                switch (input)
                {
                    case "H":
                        Hit(shoe); 
                        return;

                    case "S":
                        Stand(); 
                        return;

                    case "D":
                        if (handSize != 2)
                        {
                            ui.ShowDoubleDownNotAllowedMessage();
                            break;
                        }
                        decimal doubleAmount = BetAmount * 2;
                        if (doubleAmount > Balance)
                        {
                            ui.ShowNotEnoughBalanceForDoubleDown();
                            break;
                        }

                        DoubleDown(shoe); 
                        return;

                    case "P":
                        // Check if hand is a pair (both cards have same rank)
                        bool isPair = handSize == 2 && hand.All(card => card.Rank == hand[0].Rank);
                        if (!isPair)
                        {
                            ui.ShowSplitPairsNotAllowed();
                            break;
                        }

                        SplitPairs(shoe);
                        return;

                    default:
                        ui.ShowUnknownAction(); 
                        break;
                }
            }
        }
    }
}
