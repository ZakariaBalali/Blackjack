using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackJackLibrary.Participants;
using BlackJackLibrary.CardDeck;

namespace BlackJackLibrary.Test.ParticipantsTests
{
    [TestClass]
    public class ParticipantTests
    {
        private Participant _sut = null!;
        private Shoe shoe = null!;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new Participant();
            shoe = new Shoe(numberOfDecks: 0);
        }

        private static Card CreateCard(string name, CardSuits suit, CardRanks rank, List<int> value, bool isFaceDown = false)
        {
            Card card = new Card(name, suit, rank, value);
            if (isFaceDown) card.FlipCard();
            return card;
        }

        [TestMethod]
        public void AddCardToHand_ShouldAddCard()
        {
            // Arrange
            Card card = CreateCard("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 });

            // Act
            _sut.AddCardToHand(card);

            // Assert
            Assert.AreEqual(1, _sut.CardsInHand.Count);
            Assert.AreEqual(10, _sut.CardsValue);
        }

        [TestMethod]
        public void UpdateCardsValue_ShouldUpdateValue()
        {
            // Arrange
            Card ace = CreateCard("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> { 1, 11 });
            Card ten = CreateCard("Ten of Diamonds", CardSuits.Diamonds, CardRanks.Ten, new List<int> { 10 });

            _sut.AddCardToHand(ace);
            _sut.AddCardToHand(ten);

            // Act
            _sut.UpdateCardsValue();

            // Assert
            Assert.AreEqual(21, _sut.CardsValue);
        }

        [TestMethod]
        public void Hit_ShouldAddCardFromShoe()
        {
            // Arrange
            Card card = CreateCard("Nine of Clubs", CardSuits.Clubs, CardRanks.Nine, new List<int> { 9 });
            shoe.Cards.Add(card);

            // Act
            _sut.Hit(shoe);

            // Assert
            Assert.AreEqual(1, _sut.CardsInHand.Count);
            Assert.AreEqual(9, _sut.CardsValue);
        }

        [TestMethod]
        public void Stand_ShouldSetHasStoodToTrue()
        {
            // Act
            _sut.Stand();

            // Assert
            Assert.IsTrue(_sut.HasStood);
        }

        [TestMethod]
        public void IsBust_ShouldReturnTrue_WhenValueOver21()
        {
            // Arrange
            Card card1 = CreateCard("King of Hearts", CardSuits.Hearts, CardRanks.King, new List<int> { 10 });
            Card card2 = CreateCard("Queen of Spades", CardSuits.Spades, CardRanks.Queen, new List<int> { 10 });
            Card card3 = CreateCard("Three of Diamonds", CardSuits.Diamonds, CardRanks.Three, new List<int> { 3 });

            _sut.AddCardToHand(card1);
            _sut.AddCardToHand(card2);
            _sut.AddCardToHand(card3);

            // Act
            bool isBust = _sut.IsBust(0);

            // Assert
            Assert.IsTrue(isBust);
        }

        [TestMethod]
        public void HasValueOfTwentyOne_ShouldReturnTrue_WhenValueIs21()
        {
            // Arrange
            Card ace = CreateCard("Ace of Clubs", CardSuits.Clubs, CardRanks.Ace, new List<int> { 1, 11 });
            Card king = CreateCard("King of Clubs", CardSuits.Clubs, CardRanks.King, new List<int> { 10 });

            _sut.AddCardToHand(ace);
            _sut.AddCardToHand(king);

            // Act
            bool has21 = _sut.HasValueOfTwentyOne(0);

            // Assert
            Assert.IsTrue(has21);
        }

        [TestMethod]
        public void ClearValuesForNewRound_ShouldResetHandsAndCurrentHandIndex()
        {
            // Arrange
            Card card = CreateCard("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 });
            _sut.AddCardToHand(card);

            // Act
            _sut.ClearHands();

            // Assert
            Assert.AreEqual(1, _sut.hands.Count);
            Assert.AreEqual(0, _sut.CardsInHand.Count);
            Assert.AreEqual(0, _sut.currentHandIndex);
        }
    }
}
