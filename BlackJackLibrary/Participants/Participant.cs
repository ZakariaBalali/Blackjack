using BlackJackLibrary.CardDeck;

namespace BlackJackLibrary.Participants
{
    public class Participant
    {
        public readonly List<Hand> hands; // Multiple hands in case of split pairs
        public int currentHandIndex; // Index to keep track of which hand is being used
        public Hand CurrentHand => hands[currentHandIndex]; // Hand that is being used now
        public List<Card> CardsInHand => CurrentHand.CardsInHand; // Cards of the hand that is being used now
        public int CardsValue => CurrentHand.CardsValue; // Card value of the current hand
        public bool HasStood => CurrentHand.HasStood; // Checks if the current hand has chosen to stand

        public Participant()
        {
            hands = new List<Hand> { new Hand() };  // Start with one hand by default
            currentHandIndex = 0;
        }

        // Method to add a card to the current hand
        public void AddCardToHand(Card card)
        {
            CurrentHand.AddCard(card);
        }

        // Method to update the card value of the current hand
        public void UpdateCardsValue()
        {
            CurrentHand.UpdateCardsValue();
        }

        // Hit play option (takes card from shoe, adds it to current hand)
        internal void Hit(Shoe shoe)
        {
            var card = shoe.TakeCard();
            CurrentHand.AddCard(card);
        }

        // Stand play option (changes value only on the current hand)
        internal void Stand()
        {
            CurrentHand.HasStood = true;
        }

        // Reusable method to check if a specific hand has busted
        public bool IsBust(int handIndex)
        {
            return hands[handIndex].CardsValue > 21;
        }

        // Reusable method to check if a specific hand has a value of 21
        public bool HasValueOfTwentyOne(int handIndex)
        {
            return hands[handIndex].CardsValue == 21;
        }

        // Resets hands for a new round
        public void ClearHands()
        {
            hands.Clear();
            hands.Add(new Hand());
            currentHandIndex = 0;
        }
    }
}
