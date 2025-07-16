using System;

namespace UI.UI
{
    public interface IGameUI
    {
        string FaceDownDisplayName { get; }

        // Default options
        void ShowMessage(string message, ConsoleColor color = ConsoleColor.White);
        string GetUserInput(string prompt);
        void Wait(string prompt, int milliseconds = 1000);

        // UI used by multiple classes
        void ShowPlayerBalance(int balance);

        // UI options for Round class
        void ShowNewRoundStart();
        void ShowPlayerHandStart(int index);
        void ShowPlayerHandValue(int value);
        void ShowDraw();
        void ShowWin(int winnings);
        void ShowLoss(int betAmount);
        void ShowTurnHeader(int turn);
        void ShowDealerInfo(List<string> cardDescriptions, int totalValue);
        void ShowPlayerInfo(int handIndex, List<string> cardDescriptions, int value, int bet);
        void ShowDealerTurnMessage(int milliseconds = 1000);
        void ShowEvaluationMessage(int index, int milliseconds = 1000);

        // UI options for Shoe game class
        string AskForNewRound();
        void InvalidOption();
        void ThanksForPlaying();
        void NotEnoughBalance();

        // UI options for player class
        void ShowPlaceBetPrompt();
        void ShowInvalidBet();
        string GetBetInput();
        string GetPlayOptionInput(int handsize);
        void ShowDoubleDownNotAllowedMessage();
        void ShowNotEnoughBalanceForDoubleDown();
        void ShowSplitPairsNotAllowed();
        void ShowUnknownAction();
    }
}
