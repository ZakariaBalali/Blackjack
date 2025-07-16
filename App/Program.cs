using UI.UI;
using BlackJackLibrary.GameLogic;
namespace App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            IGameUI ui = new GameUI();
            ShoeGame Game = new ShoeGame(ui);
            Game.Start();
        }
    }
}
