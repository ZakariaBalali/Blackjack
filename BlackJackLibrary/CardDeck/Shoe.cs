using BlackJackLibrary.CardDeck;

public class Shoe
{
    public List<Card> Cards { get; internal set; } = []; // All the cards that are handed out in the game
    private int CardIndex { get; set; } // Keeps track of what next card to hand out

    // Constructor creates the shoe with a specific number of decks (default is 6)
    public Shoe(int numberOfDecks = 6)
    {
        for (int i = 0; i < numberOfDecks; i++)
        {
            Cards.AddRange(GenerateDeck());
        }
    }
    // Method that returns the next card to hand out
    public Card TakeCard()
    {
        return Cards[CardIndex++];
    }

    // Generates a deck of a unique 52 cards using the card rank and card suit enums
    private static List<Card> GenerateDeck()
    {
        List<Card> deck = new List<Card>(); ;
        foreach (CardRanks rank in Enum.GetValues<CardRanks>())
        {
            foreach (CardSuits suit in Enum.GetValues<CardSuits>())
            {
                string name = $"{rank} of {suit}";
                List<int> value = GetCardValue(rank);
                deck.Add(new Card(name, suit, rank, value));
            }
        }
        return deck;
    }

    // Fisher Yates algorithm
    public void Shuffle()
    {
        Random rand = new();
        for (int index = Cards.Count - 1; index > 0; index--)
        {
            int randomIndex = rand.Next(index + 1);
            (Cards[index], Cards[randomIndex]) = (Cards[randomIndex], Cards[index]);
        }
    }

    // Gets the card value using the dictionary
    private static List<int> GetCardValue(CardRanks rank) => RankValues[rank];

    // Dictionary to keep track of all the values
    private static readonly Dictionary<CardRanks, List<int>> RankValues = new()
    {
        { CardRanks.Ace, new List<int> { 1, 11 } },
        { CardRanks.Two, new List<int> { 2 } },
        { CardRanks.Three, new List<int> { 3 } },
        { CardRanks.Four, new List<int> { 4 } },
        { CardRanks.Five, new List<int> { 5 } },
        { CardRanks.Six, new List<int> { 6 } },
        { CardRanks.Seven, new List<int> { 7 } },
        { CardRanks.Eight, new List<int> { 8 } },
        { CardRanks.Nine, new List<int> { 9 } },
        { CardRanks.Ten, new List<int> { 10 } },
        { CardRanks.Jack, new List<int> { 10 } },
        { CardRanks.Queen, new List<int> { 10 } },
        { CardRanks.King, new List<int> { 10 } },
    };
}
