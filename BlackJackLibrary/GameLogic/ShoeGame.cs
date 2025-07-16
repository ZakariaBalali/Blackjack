using BlackJackLibrary.Participants;
using UI.UI;

namespace BlackJackLibrary.GameLogic
{
    public class ShoeGame
    {
        private readonly IGameUI ui;
        private readonly Player Player;
        private readonly Dealer Dealer;
        private Round Round;
        private readonly Shoe Shoe;

        // Initializes the game with UI, player, dealer, shoe, and round
        public ShoeGame(IGameUI gameUI)
        {
            Player = new Player(ui: gameUI, startingBalance: 20);
            Dealer = new Dealer();
            Shoe = new Shoe();
            Round = new Round(Player, Shoe, Dealer, gameUI);
            this.ui = gameUI;
        }

        // Starts the game loop
        public void Start()
        {
            ui.ShowPlayerBalance(Player.Balance);

            // Game continues while player has enough balance
            while (Player.Balance >= 1)
            {
                Round = new Round(Player, Shoe, Dealer, ui);
                Round.StartRound();

                // Ask player if they want to play another round
                while (true)
                {
                    ui.ShowPlayerBalance(Player.Balance);
                    string input = ui.AskForNewRound().Trim().ToUpper();

                    switch (input)
                    {
                        case "Y":
                            break; // start another round
                        case "N":
                            ui.ThanksForPlaying();
                            return; // Ends game loop
                        default:
                            ui.InvalidOption(); // ask again on invalid input
                            continue;
                    }
                    break;
                }
            }

            // If balance is too low, show message
            if (Player.Balance < 1)
            {
                ui.ShowPlayerBalance(Player.Balance);
                ui.NotEnoughBalance();
            }
        }
    }
}
