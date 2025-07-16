using BlackJackLibrary.Participants;
using BlackJackLibrary.CardDeck;

namespace BlackJackLibrary.Test.ParticipantsTests;


[TestClass]
public class DealerTests
{
    [TestMethod]
    public void TurnFaceDownCardsUp_FlipsAllFaceDownCards()
    {
        // Arrange
        Dealer dealer = new Dealer();

        Card card1 = new Card("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> { 1, 11 });
        Card card2 = new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 });
        Card card3 = new Card("Five of Clubs", CardSuits.Clubs, CardRanks.Five, new List<int> { 5 });

        // Flip cards 1 and 3 face down
        card1.FlipCard();
        card3.FlipCard();

        dealer.AddCardToHand(card1);
        dealer.AddCardToHand(card2);
        dealer.AddCardToHand(card3);

        // Act
        dealer.TurnFaceDownCardsUp();

        // Assert
        foreach (Card card in dealer.CardsInHand)
        {
            Assert.IsFalse(card.IsFaceDown, $"Card {card.Name} should be face up.");
        }
    }

    [TestMethod]
    public void AddCardToHand_AddsCardCorrectly()
    {
        // Arrange
        Dealer dealer = new Dealer();
        Card card = new Card("King of Diamonds", CardSuits.Diamonds, CardRanks.King, new List<int> { 10 });

        // Act
        dealer.AddCardToHand(card);

        // Assert
        Assert.AreEqual(1, dealer.CardsInHand.Count);
        Assert.AreSame(card, dealer.CardsInHand[0]);
    }
}
