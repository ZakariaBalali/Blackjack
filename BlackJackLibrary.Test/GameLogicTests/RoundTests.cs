using Moq;
using BlackJackLibrary.CardDeck;
using BlackJackLibrary.Participants;
using BlackJackLibrary.GameLogic;
using UI.UI;

namespace BlackJackLibrary.Test.GameLogicTests
{
    [TestClass]
    public class RoundTests
    {
        // Test subclass of Shoe with predictable cards

        private Mock<IGameUI> mockUI = null!;
        private Player player = null!;
        private Shoe shoe = null!;
        private Dealer dealer = null!;
        private Round round = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Setup mock UI and test objects
            mockUI = new Mock<IGameUI>();

            // Create predictable shoe card set
            List<Card> Cards = new List<Card>
            {
                new Card("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> {1, 11}),
                new Card("Two of Hearts", CardSuits.Hearts, CardRanks.Two, new List<int> {2}),
                new Card("Three of Diamonds", CardSuits.Diamonds, CardRanks.Three, new List<int> {3}),
                new Card("Five of Hearts", CardSuits.Hearts, CardRanks.Five, new List<int> {5}),
                new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> {7}),
                new Card("Ten of Diamonds", CardSuits.Diamonds, CardRanks.Ten, new List<int> {10}),
                new Card("King of Clubs", CardSuits.Clubs, CardRanks.King, new List<int> {10}),

            };

            // Initialize shoe with the above predefined cards
            shoe = new Shoe();
            shoe.Cards = Cards;

            // Initialize player, dealer and round with mocked UI
            player = new Player(ui: mockUI.Object, startingBalance: 20);
            dealer = new Dealer();
            round = new Round(player, shoe, dealer, mockUI.Object);

        }


        [TestMethod]
        public void HandOutStartingCards_HandsEqualToTwo()
        {
            // Arrange

            // Act
            round.HandOutStartingCards();

            // Assert

            // Player should have 2 cards initially
            Assert.AreEqual(2, player.CurrentHand.CardsInHand.Count);
            // Dealer should have 2 cards (second one face down)
            Assert.AreEqual(2, dealer.CardsInHand.Count);
            Assert.IsTrue(dealer.CardsInHand[1].IsFaceDown);
        }

        [TestMethod]
        public void EvaluateResults_PlayerBust_ShowsLoss()
        {

            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // Bust hand: 10 + King + Queen = 30
            player.AddCardToHand(new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 }));
            player.AddCardToHand(new Card("King of Clubs", CardSuits.Clubs, CardRanks.King, new List<int> { 10 }));
            player.AddCardToHand(new Card("Queen of Diamonds", CardSuits.Diamonds, CardRanks.Queen, new List<int> { 10 }));

            // Dealer has a safe low hand
            dealer.AddCardToHand(new Card("Five of Diamonds", CardSuits.Diamonds, CardRanks.Five, new List<int> { 5 }));
            dealer.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            bool lossCalled = false;
            mockUI.Setup(ui => ui.ShowLoss(It.IsAny<int>()))
                  .Callback(() => lossCalled = true);

            round.EvaluateResults();

            // Assert
            Assert.IsTrue(lossCalled, "Player bust should trigger ShowLoss");
        } 

        [TestMethod]
        public void EvaluateResults_PlayerLosesByValue_ShowsLoss()
        {

            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // Player has a value of 16
            player.AddCardToHand(new Card("Nine of Hearts", CardSuits.Hearts, CardRanks.Nine, new List<int> { 9 }));
            player.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            // Dealer has a value of 21
            dealer.AddCardToHand(new Card("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> { 1, 11 }));
            dealer.AddCardToHand(new Card("King of Diamonds", CardSuits.Diamonds, CardRanks.King, new List<int> { 10 }));


            bool lossCalled = false;
            mockUI.Setup(ui => ui.ShowLoss(It.IsAny<int>()))
                  .Callback(() => lossCalled = true);

            round.EvaluateResults();

            // Assert
            Assert.IsTrue(lossCalled, "Player bust should trigger ShowLoss");
        } 
        
        [TestMethod]
        public void EvaluateResults_PlayerLosesByValue_SubtractsBetFromBalance()
        {
            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // Player has a value of 16
            player.AddCardToHand(new Card("Nine of Hearts", CardSuits.Hearts, CardRanks.Nine, new List<int> { 9 }));
            player.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            // Dealer has a value of 21
            dealer.AddCardToHand(new Card("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> { 1, 11 }));
            dealer.AddCardToHand(new Card("King of Diamonds", CardSuits.Diamonds, CardRanks.King, new List<int> { 10 }));

            round.EvaluateResults();

            // Assert
            Assert.AreEqual(10, player.Balance);
        }

        [TestMethod]
        public void EvaluateResults_PlayerBust_SubtractsBetFromBalance()
        {

            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // Bust hand: 10 + King + Queen = 30
            player.AddCardToHand(new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 }));
            player.AddCardToHand(new Card("King of Clubs", CardSuits.Clubs, CardRanks.King, new List<int> { 10 }));
            player.AddCardToHand(new Card("Queen of Diamonds", CardSuits.Diamonds, CardRanks.Queen, new List<int> { 10 }));

            // Dealer has a safe low hand
            dealer.AddCardToHand(new Card("Five of Diamonds", CardSuits.Diamonds, CardRanks.Five, new List<int> { 5 }));
            dealer.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            round.EvaluateResults();

            // Assert
            Assert.AreEqual(10, player.Balance);
        }

        [TestMethod]
        public void EvaluateResults_PlayerWin_ShowsWin()
        {

            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // High hand: 10 + King = 20
            player.AddCardToHand(new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 }));
            player.AddCardToHand(new Card("King of Clubs", CardSuits.Clubs, CardRanks.King, new List<int> { 10 }));

            // Dealer has a low hand
            dealer.AddCardToHand(new Card("Five of Diamonds", CardSuits.Diamonds, CardRanks.Five, new List<int> { 5 }));
            dealer.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            bool winCalled = false;
            mockUI.Setup(ui => ui.ShowWin(It.IsAny<int>()))
                  .Callback(() => winCalled = true);

            round.EvaluateResults();

            // Assert
            Assert.IsTrue(winCalled, "Player win should trigger ShowWin");
        }

        [TestMethod]
        public void EvaluateResults_PlayerWin_AddsBetToBalance()
        {

            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // High hand: 10 + King = 20
            player.AddCardToHand(new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 }));
            player.AddCardToHand(new Card("King of Clubs", CardSuits.Clubs, CardRanks.King, new List<int> { 10 }));

            // Dealer has a low hand
            dealer.AddCardToHand(new Card("Five of Diamonds", CardSuits.Diamonds, CardRanks.Five, new List<int> { 5 }));
            dealer.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            round.EvaluateResults();

            // Assert
            Assert.AreEqual(30, player.Balance);
        }

        [TestMethod]
        public void EvaluateResults_PlayerBlackJack_AddsBetToBalance()
        {

            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // High hand: Ace + King = 21
            player.AddCardToHand(new Card("Ace of Hearts", CardSuits.Hearts, CardRanks.Ace, new List<int> { 1, 11 }));
            player.AddCardToHand(new Card("King of Clubs", CardSuits.Clubs, CardRanks.King, new List<int> { 10 }));

            // Dealer has a low hand
            dealer.AddCardToHand(new Card("Five of Diamonds", CardSuits.Diamonds, CardRanks.Five, new List<int> { 5 }));
            dealer.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            round.EvaluateResults();

            // Assert
            Assert.AreEqual(35, player.Balance);
        }

        [TestMethod]
        public void EvaluateResults_Draw_ShowsDraw()
        {

            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // Player Hand of 17
            player.AddCardToHand(new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 }));
            player.AddCardToHand(new Card("Four of Clubs", CardSuits.Clubs, CardRanks.Four, new List<int> { 4 }));
            player.AddCardToHand(new Card("Three of Hearts", CardSuits.Hearts, CardRanks.Three, new List<int> { 3 }));

            // Dealer also has a hand of 17
            dealer.AddCardToHand(new Card("Queen of Diamonds", CardSuits.Diamonds, CardRanks.Queen, new List<int> { 10 }));
            dealer.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            bool drawCalled = false;
            mockUI.Setup(ui => ui.ShowDraw())
                  .Callback(() => drawCalled = true);

            round.EvaluateResults();

            // Assert
            Assert.IsTrue(drawCalled, "Draw should trigger ShowDraw");
        }
        [TestMethod]
        public void EvaluateResults_Draw_ShouldNotChangeBalance()
        {

            // Arrange
            mockUI.Setup(ui => ui.GetBetInput()).Returns("10");
            player.PlaceBet();

            // Player Hand of 17
            player.AddCardToHand(new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 }));
            player.AddCardToHand(new Card("Four of Clubs", CardSuits.Clubs, CardRanks.Four, new List<int> { 4 }));
            player.AddCardToHand(new Card("Three of Hearts", CardSuits.Hearts, CardRanks.Three, new List<int> { 3 }));

            // Dealer also has a hand of 17
            dealer.AddCardToHand(new Card("Queen of Diamonds", CardSuits.Diamonds, CardRanks.Queen, new List<int> { 10 }));
            dealer.AddCardToHand(new Card("Seven of Clubs", CardSuits.Clubs, CardRanks.Seven, new List<int> { 7 }));

            round.EvaluateResults();

            // Assert
            Assert.AreEqual(20, player.Balance);
        }

    }
}
