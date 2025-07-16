using BlackJackLibrary.CardDeck;

namespace BlackJackLibrary.Test.CardDeckTests
{
    [TestClass]
    public class CardTests
    {
        private Card _sut = null!;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new Card("Ace of Spades", CardSuits.Spades, CardRanks.Ace, new List<int> { 1, 11 });
        }

        [TestMethod]
        public void Constructor_SetsCardPropertiesCorrectly()
        {
            // Assert
            Assert.AreEqual("Ace of Spades", _sut.Name);
            Assert.AreEqual(CardSuits.Spades, _sut.Suit);
            Assert.AreEqual(CardRanks.Ace, _sut.Rank);
            Assert.IsFalse(_sut.IsFaceDown);
            CollectionAssert.AreEqual(new List<int> { 1, 11 }, _sut.Value);
        }

        [TestMethod]
        public void FlipCard_TogglesIsFaceDown()
        {
            // Default value
            Assert.IsFalse(_sut.IsFaceDown);

            // Act & Assert
            _sut.FlipCard();
            Assert.IsTrue(_sut.IsFaceDown);

            // Act & Assert
            _sut.FlipCard();
            Assert.IsFalse(_sut.IsFaceDown);
        }
    }
}
