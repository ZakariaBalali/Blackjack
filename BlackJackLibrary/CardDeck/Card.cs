namespace BlackJackLibrary.CardDeck
{
    public class Card
    {
        public string Name { get; }           // For example "Ace of Spades"
        public CardSuits Suit { get; }        // (Heart, Diamond, Club Spades)
        public CardRanks Rank { get; }        // (Ace, Five, King, etc.)
        public List<int> Value { get; }       // Possible values (For example Ace = 1 or 11)

        public bool IsFaceDown { get; private set; } = false; 

        public Card(string name, CardSuits suit, CardRanks rank, List<int> value, bool isFaceDown = false)
        {
            Name = name;
            Suit = suit;
            Rank = rank;
            Value = value;
            IsFaceDown = isFaceDown;
        }

        // Toggles card between face up and face down
        public void FlipCard()
        {
            IsFaceDown = !IsFaceDown;
        }
    }
}
