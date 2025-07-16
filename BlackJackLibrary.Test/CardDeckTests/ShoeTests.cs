using BlackJackLibrary.CardDeck;

namespace BlackJackLibrary.Test.CardDeckTests
{
    [TestClass]
    public class ShoeTests
    {
        private Shoe _sut = null!;
        private const int DeckSize = 52;
        private const int DecksToGenerate = 6;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new Shoe(DecksToGenerate);
        }

        [TestMethod]
        public void Constructor_GeneratesCorrectNumberOfCards()
        {
            // Arrange
            
            int expectedCards = DecksToGenerate * DeckSize;

            // Assert
            Assert.AreEqual(expectedCards, _sut.Cards.Count);
        }

        [TestMethod]
        public void TakeCard_ReturnsNextCard()
        {
            // Arrange
            Card firstCard = _sut.Cards[0];
            Card secondCard = _sut.Cards[1];

            // Act
            Card takenCard1 = _sut.TakeCard();
            Card takenCard2 = _sut.TakeCard();

            // Assert
            Assert.AreEqual(firstCard, takenCard1);
            Assert.AreEqual(secondCard, takenCard2);
        }

        [TestMethod]
        public void Shuffle_ChangesCardOrder()
        {
            // Arrange
            List<string> beforeShuffle = _sut.Cards.Select(c => c.Name).ToList();

            // Act
            _sut.Shuffle();
            List<string> afterShuffle = _sut.Cards.Select(c => c.Name).ToList();

            // Assert 
            bool isOrderDifferent = beforeShuffle.Zip(afterShuffle, (a, b) => a == b).Any(equal => !equal);
            
            Assert.IsTrue(isOrderDifferent, "Deck was not shuffled.");
        }

        [DataTestMethod]
        [DataRow(CardRanks.Ace, new int[] { 1, 11 })]
        [DataRow(CardRanks.Two, new int[] { 2 })]
        [DataRow(CardRanks.Three, new int[] { 3 })]
        [DataRow(CardRanks.Four, new int[] { 4 })]
        [DataRow(CardRanks.Five, new int[] { 5 })]
        [DataRow(CardRanks.Six, new int[] { 6 })]
        [DataRow(CardRanks.Seven, new int[] { 7 })]
        [DataRow(CardRanks.Eight, new int[] { 8 })]
        [DataRow(CardRanks.Nine, new int[] { 9 })]
        [DataRow(CardRanks.Ten, new int[] { 10 })]
        [DataRow(CardRanks.Jack, new int[] { 10 })]
        [DataRow(CardRanks.Queen, new int[] { 10 })]
        [DataRow(CardRanks.King, new int[] { 10 })]
        public void GetCardValue_ReturnsExpectedValues(CardRanks rank, int[] expectedValues)
        {
            // Arrange
            Card card = _sut.Cards.First(c => c.Rank == rank);

            // Assert
            CollectionAssert.AreEqual(expectedValues.ToList(), card.Value);
        }

    }
}
