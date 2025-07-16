using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;

namespace UI.UI
{
    public class GameUI : IGameUI
    {
        public string FaceDownDisplayName => "## Face Down ##";

         public void ShowMessage(string message, ConsoleColor color = ConsoleColor.White)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(10);
            }

            Console.WriteLine();
            Console.ForegroundColor = previousColor;
        }

        public string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }

        public void Wait(string prompt, int milliseconds = 1000)
        {
            Console.WriteLine(prompt);
            Thread.Sleep(milliseconds);
        }

        // UI used by multiple classes
        public void ShowPlayerBalance(int balance)
        {
            ShowMessage($"Player has €{balance}");
        }

        // UI for round class
        public void ShowNewRoundStart()
        {
            ShowMessage("New Round");
            ShowMessage("===============");
            ShowMessage("Start of Player Turn");
        }

        public void ShowPlayerHandStart(int index)
        {
            ShowMessage($"Playing Hand {index + 1}");
        }

        public void ShowPlayerHandValue(int value)
        {
            ShowMessage($"Player Hand Value: {value}");
        }

        public void ShowDraw() => ShowMessage("Draw");

        public void ShowWin(int winnings) =>
            ShowMessage($"You win €{winnings}!");

        public void ShowLoss(int betAmount) =>
            ShowMessage($"You lost €{betAmount}...");

        public void ShowTurnHeader(int turn)
        {
            ShowMessage($"\n========== Turn {turn} ==========\n");
        }

        public void ShowDealerInfo(List<string> cardNames, int totalValue)
        {
            ShowMessage("♠ Dealer's Hand:", ConsoleColor.Cyan);
            foreach (string card in cardNames)
                ShowMessage(card?.ToString() ?? "null");
            ShowMessage($"Total Value: {totalValue}\n", ConsoleColor.Cyan);
        }


        public void ShowPlayerInfo(int handIndex, List<string> cardNames, int value, int bet)
        {
            ShowMessage($"♥ Player's Hand({handIndex + 1}):", ConsoleColor.Yellow);
            foreach (string card in cardNames)
                ShowMessage(card?.ToString() ?? "null");
            ShowMessage($"Total Value: {value}", ConsoleColor.Yellow);
            ShowMessage($"Current Bet: €{bet}\n", ConsoleColor.Yellow);
        }
        public void ShowDealerTurnMessage(int milliseconds = 1000)
        {
            Wait("Dealer's turn...", milliseconds);
        }
        public void ShowEvaluationMessage(int index, int milliseconds = 1000)
        {
            Wait($"Evaluating Hand {index + 1}...");
        }

        // UI used by shoe game class
        public string AskForNewRound()
        {
            return GetUserInput("Play another round? (Y/N):");
        }

        public void InvalidOption()
        {
            ShowMessage("Invalid option, please enter Y or N.", ConsoleColor.Red);
        }

        public void ThanksForPlaying()
        {
            ShowMessage("Thanks for playing!", ConsoleColor.Green);
        }

        public void NotEnoughBalance()
        {
            ShowMessage("You don't have enough balance to continue. Game over!", ConsoleColor.Red);
        }

        // UI used by player class

        public void ShowPlaceBetPrompt()
        {
            ShowMessage("Place your bet (1-10):");
        }

        public void ShowInvalidBet()
        {
            ShowMessage("Invalid bet. Please enter a number between 1 and 10, within your balance.", ConsoleColor.Red);
        }

        public string GetBetInput()
        {
            return GetUserInput("> ");
        }

        public string GetPlayOptionInput(int handSize)
        {
            StringBuilder sb = new("(H)it or (S)tand");
            if (handSize == 2)
            {
                sb.Append(" or (D)ouble Down or Split (P)airs");
            }
            return GetUserInput($"{sb}? ").ToUpper().Trim();
        }

        public void ShowDoubleDownNotAllowedMessage()
        {
            ShowMessage("You can only double down on your first two cards.", ConsoleColor.Red);
        }

        public void ShowNotEnoughBalanceForDoubleDown()
        {
            ShowMessage("You don't have enough balance to double down.", ConsoleColor.Red);
        }

        public void ShowSplitPairsNotAllowed()
        {
            ShowMessage("You can only split pairs if you have exactly two cards of the same rank.", ConsoleColor.Red);
        }

        public void ShowUnknownAction()
        {
            ShowMessage("Unknown action, please try again.", ConsoleColor.Red);
        }

    }
}
