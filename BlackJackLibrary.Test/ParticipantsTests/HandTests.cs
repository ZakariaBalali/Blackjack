using BlackJackLibrary.Participants;
using BlackJackLibrary.CardDeck;

namespace BlackJackLibrary.Test.ParticipantsTests
{
    [TestClass]
    public class HandTests
    {
        private Hand _sut = null!;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new Hand();
        }

        [TestMethod]
        public void AddCard_ShouldAddCardToHand()
        {
            // Arrange
            Card card = new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 });

            // Act
            _sut.AddCard(card);

            // Assert
            Assert.AreEqual(1, _sut.CardsInHand.Count);
            Assert.AreEqual(card, _sut.CardsInHand[0]);
        }

        [TestMethod]
        public void AddCard_ShouldUpdateCardsValue()
        {
            // Arrange
            Card card = new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 });

            // Act
            _sut.AddCard(card);

            // Assert
            Assert.AreEqual(10, _sut.CardsValue);
        }

        [TestMethod]
        public void UpdateCardsValue_ShouldHandleSingleAceProperly()
        {
            // Arrange
            Card ace = new Card("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> { 1, 11 });
            Card ten = new Card("Ten of Clubs", CardSuits.Clubs, CardRanks.Ten, new List<int> { 10 });

            // Act
            _sut.AddCard(ace);
            _sut.AddCard(ten);

            // Assert
            Assert.AreEqual(21, _sut.CardsValue);
        }

        [TestMethod]
        public void UpdateCardsValue_ShouldHandleMultipleAces()
        {
            // Arrange
            Card ace1 = new Card("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> { 1, 11 });
            Card ace2 = new Card("Ace of Diamonds", CardSuits.Diamonds, CardRanks.Ace, new List<int> { 1, 11 });
            Card nine = new Card("Nine of Hearts", CardSuits.Hearts, CardRanks.Nine, new List<int> { 9 });

            // Act
            _sut.AddCard(ace1);
            _sut.AddCard(ace2);
            _sut.AddCard(nine);

            // Assert
            Assert.AreEqual(21, _sut.CardsValue);
        }

        [TestMethod]
        public void UpdateCardsValue_ShouldIgnoreFaceDownCards()
        {
            // Arrange
            Card visible = new Card("Ten of Hearts", CardSuits.Hearts, CardRanks.Ten, new List<int> { 10 });
            Card hidden = new Card("Ten of Spades", CardSuits.Spades, CardRanks.Ten, new List<int> { 10 }, isFaceDown: true);

            // Act
            _sut.AddCard(visible);
            _sut.AddCard(hidden);

            // Assert
            Assert.AreEqual(10, _sut.CardsValue);
        }

        [TestMethod]
        public void TurnFaceDownCardsUp_ShouldFlipAllCardsFaceUp()
        {
            // Arrange
            Card faceDown1 = new Card("Queen of Hearts", CardSuits.Hearts, CardRanks.Queen, new List<int> { 10 }, isFaceDown: true);
            Card faceDown2 = new Card("King of Spades", CardSuits.Spades, CardRanks.King, new List<int> { 10 }, isFaceDown: true);
            Card faceUp = new Card("Jack of Clubs", CardSuits.Clubs, CardRanks.Jack, new List<int> { 10 });

            _sut.AddCard(faceDown1);
            _sut.AddCard(faceDown2);
            _sut.AddCard(faceUp);

            // Act
            _sut.TurnFaceDownCardsUp();

            // Assert
            Assert.IsFalse(_sut.CardsInHand[0].IsFaceDown);
            Assert.IsFalse(_sut.CardsInHand[1].IsFaceDown);
            Assert.IsFalse(_sut.CardsInHand[2].IsFaceDown);
        }
    }
}
