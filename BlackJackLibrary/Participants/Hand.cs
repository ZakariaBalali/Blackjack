using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackLibrary.CardDeck;

namespace BlackJackLibrary.Participants
{
    public class Hand
    {
        public List<Card> CardsInHand { get; private set; }
        public int CardsValue { get; private set; }
        public bool HasStood { get; set; }

        public Hand()
        {
            CardsInHand = new List<Card>();
            HasStood = false;
        }
        // Adds a card to the hand and updates its value
        public void AddCard(Card card)
        {
            CardsInHand.Add(card);
            UpdateCardsValue();
        }

        // Flips all face-down cards face-up
        public void TurnFaceDownCardsUp()
        {
            var FaceDownCards = CardsInHand.Where(c => c.IsFaceDown == true);

            foreach (var card in FaceDownCards)
            {
                card.FlipCard();
            }
        }
        // Calculates the total value of the hand (handles Aces as 1 or 11)
        public void UpdateCardsValue()
        {
            int total = 0;
            int aceCount = 0;

            foreach (var card in CardsInHand)
            {
                if (card.IsFaceDown)
                    continue;

                switch (card.Value.Count)
                {
                    case 1:
                        total += card.Value[0];
                        break;
                    case 2:
                        total += 11; // Assumes 11 as default, adjusts it later if needed
                        aceCount++;
                        break;
                }
            }

            // Check for ace value if over 21
            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }

            CardsValue = total;
        }
    }
}