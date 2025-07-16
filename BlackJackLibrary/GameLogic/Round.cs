using BlackJackLibrary.CardDeck;
using BlackJackLibrary.Participants;
using System.Collections.Generic;
using System.Linq;
using UI.UI;

namespace BlackJackLibrary.GameLogic
{
    internal class Round
    {
        private readonly Player player;
        private readonly Shoe shoe;
        private readonly Dealer dealer;
        private readonly IGameUI ui;

        // Keeps track of which turn we're on (each player/dealer action increases it)
        private int turn = 1;

        public Round(Player player, Shoe shoe, Dealer dealer, IGameUI ui)
        {
            this.player = player;
            this.shoe = shoe;
            this.dealer = dealer;
            this.ui = ui;
        }

        // Starts the round: shuffles the shoe, resets state, deals cards
        public void StartRound()
        {
            shoe.Shuffle();
            player.ClearHands();
            dealer.ClearHands();
            player.PlaceBet();
            HandOutStartingCards();
            Play();
        }

        // Main game loop for player and dealer actions
        private void Play()
        {
            ui.ShowNewRoundStart();

            // Loop through player hands (in case of split)
            do
            {
                DisplayInformation();
                int handIndex = player.currentHandIndex;
                ui.ShowPlayerHandStart(handIndex);

                // While current hand is active (not bust, not 21, not stood)
                while (!player.IsBust(handIndex) && !player.HasValueOfTwentyOne(handIndex) && !player.HasStood)
                {
                    ui.ShowPlayerHandValue(player.CardsValue);
                    player.ChoosePlayOption(shoe);
                    turn++;

                    if (!player.HasStood)
                    {
                        DisplayInformation();
                    }
                }

            } while (player.AdvanceToNextHand());

            DealerTurn();
            EvaluateResults();
        }

        // Deals 2 cards to player and dealer; dealer's second card is face down
        internal void HandOutStartingCards()
        {
            for (int i = 0; i < 2; i++)
            {
                Card playerCard = shoe.TakeCard();
                player.AddCardToHand(playerCard);

                Card dealerCard = shoe.TakeCard();
                if (i == 1) dealerCard.FlipCard(); // Hide dealer's second card
                dealer.AddCardToHand(dealerCard);
            }
        }

        // Displays both dealer and player hand info
        private void DisplayInformation()
        {
    
            ui.ShowTurnHeader(turn);
            // Display Dealer info
            List<string> dealerCards = GetCardDescriptions(dealer.CardsInHand);
            ui.ShowDealerInfo(dealerCards, dealer.CardsValue);

            // Display Player info
            Hand currentHand = player.hands[player.currentHandIndex];
            List<string> playerCards = GetCardDescriptions(currentHand.CardsInHand);
            ui.ShowPlayerInfo(player.currentHandIndex, playerCards, currentHand.CardsValue, player.BetAmount);
        }

        // Converts each card into a displayable string (and hides name of face down cards)
        private List<string> GetCardDescriptions(List<Card> cards)
        {
            return cards.Select(c => c.IsFaceDown ? ui.FaceDownDisplayName : c.Name).ToList();
        }

        // Checks results of all player hands and checks win/loss/draw based on dealer's hand
        internal void EvaluateResults()
        {
            for (int index = 0; index < player.hands.Count; index++)
            {
                Hand hand = player.hands[index];
                ui.ShowEvaluationMessage(index);

                switch (true)
                {
                    case var _ when player.IsBust(index):
                        Loss();
                        break;

                    case var _ when dealer.IsBust(0):
                        Win(index);
                        break;

                    case var _ when hand.CardsValue > dealer.CardsValue:
                        Win(index);
                        break;

                    case var _ when hand.CardsValue == dealer.CardsValue:
                        Draw();
                        break;

                    default:
                        Loss();
                        break;
                }
            }
        }

        // Called when player draws with dealer
        private void Draw()
        {
            ui.ShowDraw();
        }

        // Called when player wins, calculates payout based on blackjack
        private void Win(int handIndex)
        {
            bool isBlackJack = player.IsBlackJack(handIndex);
            int winnings = (int)(player.BetAmount * (isBlackJack ? 1.5 : 1));
            player.AddToBalance(isBlackJack);
            ui.ShowWin(winnings);
        }

        // Called when player loses, reduces balance
        private void Loss()
        {
            player.ReduceBalance();
            ui.ShowLoss(player.BetAmount);
        }

        // Dealer reveals hidden card and draws until reaching 17 or more
        private void DealerTurn()
        {
            dealer.TurnFaceDownCardsUp();
            dealer.UpdateCardsValue();
            DisplayInformation();

            while (dealer.CardsValue < 17)
            {
                ui.ShowDealerTurnMessage();
                dealer.Hit(shoe);
                turn++;
                DisplayInformation();
            }
        }
    }
}
