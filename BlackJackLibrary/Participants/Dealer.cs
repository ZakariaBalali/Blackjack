namespace BlackJackLibrary.Participants
{
    // Represents the dealer in the game, inherits shared logic from Participant
    internal class Dealer : Participant
    {
        // Turns over all face-down cards and updates the dealer's hand value
        public void TurnFaceDownCardsUp()
        {
            CurrentHand.TurnFaceDownCardsUp();
            UpdateCardsValue();
        }
    }
}
