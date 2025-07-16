using BlackJackLibrary.Participants;
using BlackJackLibrary.CardDeck;
using Moq;
using UI.UI;

namespace BlackJackLibrary.Test.ParticipantsTests;

[TestClass]
public class PlayerTests
{
    private Player player = null!;
    private Mock<IGameUI> mockUI = null!;
    private Shoe shoe = null!;

    [TestInitialize]
    public void Initialize()
    {
        // Arrange
        mockUI = new Mock<IGameUI>();
        player = new Player(ui: mockUI.Object, startingBalance: 100);
        shoe = new Shoe(numberOfDecks: 0);
    }

    [TestMethod]
    public void ReduceBalance_DecreasesBalanceByBetAmount()
    {
        // Arrange
        player.BetAmount = 30;

        // Act
        player.ReduceBalance();

        // Assert
        Assert.AreEqual(70, player.Balance);
    }

    [DataTestMethod]
    [DataRow(false, 120)]
    [DataRow(true, 130)]
    public void AddToBalance_AddsCorrectAmount(bool isBlackjack, int expectedBalance)
    {
        // Arrange
        player.BetAmount = 20;

        // Act
        player.AddToBalance(isBlackjack);

        // Assert
        Assert.AreEqual(expectedBalance, player.Balance);
    }

    [TestMethod]
    public void AdvanceToNextHand_ReturnsFalse_WhenOnlyOneHand()
    {
        // Act
        bool result = player.AdvanceToNextHand();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void AdvanceToNextHand_ReturnsTrue_WhenMoreThanOneHand()
    {
        // Arrange
        player.hands.Add(new Hand());

        // Act
        bool result = player.AdvanceToNextHand();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, player.currentHandIndex);
    }

    [TestMethod]
    public void IsBlackJack_ReturnsTrue_WhenTwoCardsEqual21()
    {
        // Arrange
        Hand hand = player.CurrentHand;
        hand.CardsInHand.Add(new Card("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> { 1, 11 }));
        hand.CardsInHand.Add(new Card("King of Hearts", CardSuits.Hearts, CardRanks.King, new List<int> { 10 }));
        hand.UpdateCardsValue();

        // Act
        bool result = player.IsBlackJack(player.currentHandIndex);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsBlackJack_ReturnsFalse_WhenMoreThanTwoCards()
    {
        // Arrange
        Hand hand = player.CurrentHand;
        hand.CardsInHand.Add(new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 }));
        hand.CardsInHand.Add(new Card("Five of Clubs", CardSuits.Clubs, CardRanks.Five, new List<int> { 5 }));
        hand.CardsInHand.Add(new Card("Six of Diamonds", CardSuits.Diamonds, CardRanks.Six, new List<int> { 6 }));
        hand.UpdateCardsValue();

        // Act
        bool result = player.IsBlackJack(player.currentHandIndex);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ClearHands_ResetsHandsToOneEmptyHand()
    {
        // Arrange
        player.hands.Add(new Hand());

        // Act
        player.ClearHands();

        // Assert
        Assert.AreEqual(1, player.hands.Count);
        Assert.AreEqual(0, player.hands[0].CardsInHand.Count);
        Assert.AreEqual(0, player.currentHandIndex);
    }

    [TestMethod]
    public void SplitPairs_SplitsHandIntoTwoHands()
    {
        // Arrange
        shoe.Cards = new List<Card>
        {
            new Card("Three of Hearts", CardSuits.Hearts, CardRanks.Three, new List<int>{3}),
            new Card("Four of Spades", CardSuits.Spades, CardRanks.Four, new List<int>{4})
        };

        Hand hand = player.CurrentHand;
        hand.AddCard(new Card("Seven of Hearts", CardSuits.Hearts, CardRanks.Seven, new List<int> { 7 }));
        hand.AddCard(new Card("Seven of Diamonds", CardSuits.Diamonds, CardRanks.Seven, new List<int> { 7 }));

        // Act
        player.SplitPairs(shoe);

        // Assert
        Assert.AreEqual(2, player.hands.Count);
        Assert.AreEqual(2, player.hands[0].CardsInHand.Count);
        Assert.AreEqual(2, player.hands[1].CardsInHand.Count);
        Assert.IsTrue(player.hands[0].CardsInHand.Any(c => c.Rank == CardRanks.Seven));
        Assert.IsTrue(player.hands[1].CardsInHand.Any(c => c.Rank == CardRanks.Seven));
        Assert.IsTrue(player.hands[0].CardsInHand.Any(c => c.Rank == CardRanks.Three));
        Assert.IsTrue(player.hands[1].CardsInHand.Any(c => c.Rank == CardRanks.Four));
    }

    [TestMethod]
    public void DoubleDown_DoublesBetAndHitsThenStands()
    {
        // Arrange
        shoe.Cards = new List<Card>
        {
            new Card("Three of Hearts", CardSuits.Hearts, CardRanks.Three, new List<int>{3}),
            new Card("Four of Spades", CardSuits.Spades, CardRanks.Four, new List<int>{4})
        };

        Hand hand = player.CurrentHand;
        hand.AddCard(new Card("Five of Hearts", CardSuits.Hearts, CardRanks.Five, new List<int> { 5 }));
        hand.AddCard(new Card("Six of Clubs", CardSuits.Clubs, CardRanks.Six, new List<int> { 6 }));

        player.BetAmount = 10;

        // Act
        player.DoubleDown(shoe);

        // Assert
        Assert.AreEqual(20, player.BetAmount);
        Assert.AreEqual(3, hand.CardsInHand.Count);
        Assert.AreEqual(CardRanks.Three, hand.CardsInHand.Last().Rank);
    }
}
