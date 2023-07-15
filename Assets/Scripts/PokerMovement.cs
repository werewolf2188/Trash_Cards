using System.Collections;
using System.Collections.Generic;
using System.Linq;


public abstract class PokerMovement
{
    public static PokerMovement Validate(IList<Card> cards)
    {
        if (cards.Count != 5) throw new System.ArgumentException("Cards should be 5");

        ValidateDuplicates(cards);

        PokerMovement movement = new Movements.RoyalStraightFlushPokerMovement(); // This should be the biggest movement we can get
        while (movement != null)
        {
            if (movement.ValidateMovement(cards))
            {
                return movement;
            }
            movement = movement.nextMovementToValidate;
        }
        return null;
    }

    public bool IsHigherThan(PokerMovement movement)
    {
        if (this.Points > movement.Points) return true;
        else if (movement.Points > this.Points) return false;

        return this.HighestCard.IsHigherThan(movement.HighestCard);
    }

    private static void ValidateDuplicates(IList<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int duplicate = 0;
            Card card = cards[i];
            for (int j = 0; j < cards.Count; j++)
            {
                if (cards[j].Equals(card))
                    duplicate++;
            }
            if (duplicate > 1) throw new System.ArgumentException($"Card is duplicate: {card}");
        }
    }

    protected uint _points;
    public uint Points { get { return _points; } }

    protected Card _highestCard;
    public virtual Card HighestCard { get { return _highestCard; } }

    protected List<Card> _cardsToKeep = new List<Card>();
    public Card[] CardsToKeep { get { return _cardsToKeep.ToArray(); } }

    protected PokerMovement nextMovementToValidate;

    protected List<PokerMovement> relatedMovements;

    public abstract bool ValidateMovement(IList<Card> cards);    
}

namespace Movements
{
    public sealed class HighestPokerMovement : PokerMovement
    {
        public HighestPokerMovement()
        {
            _points = 1;
            nextMovementToValidate = null;
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            _highestCard = cards.OrderBy(e => e).Last();
            if (cards.Any(e => e.number == CardNumber.As))
            {
                _highestCard = cards.First(e => e.number == CardNumber.As);
            }
            _cardsToKeep.Add(_highestCard);
            return true;
        }

        public override string ToString()
        {
            return $"Highest card: {_highestCard.number.ToString()}";
        }
    }

    public sealed class PairPokerMovement : PokerMovement
    {
        private CardNumber repeatedNumber;

        public PairPokerMovement()
        {
            _points = 2;
            nextMovementToValidate = new HighestPokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            foreach (var cardNumber in System.Enum.GetValues(typeof(CardNumber)))
            {
                if (cards.Where(e => e.number == ((CardNumber)cardNumber)).Count() == 2)
                {
                    repeatedNumber = (CardNumber)cardNumber;
                    _highestCard = cards.Where(e => e.number == ((CardNumber)cardNumber)).First();
                    _cardsToKeep.AddRange(cards.Where(e => e.number == ((CardNumber)cardNumber)));
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"Pair of {repeatedNumber.ToString()}";
        }
    }

    public sealed class TwoPairPokerMovement : PokerMovement
    {
        public TwoPairPokerMovement()
        {
            _points = 3;
            relatedMovements = new List<PokerMovement>
            {
                new PairPokerMovement(),
                new PairPokerMovement()
            };
            nextMovementToValidate = new PairPokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {

            List<Card> tempCards = cards.ToList();
            uint pairs = 0;

            foreach (PokerMovement movement in relatedMovements)
            {
                if (movement.ValidateMovement(tempCards))
                {
                    tempCards.RemoveAll(e => e.number == movement.HighestCard.number);
                    _cardsToKeep.AddRange(movement.CardsToKeep);
                    pairs++;
                }
            }

            return pairs == 2;
        }

        public override Card HighestCard
        {
            get
            {
                if (_highestCard == null)
                {
                    foreach (PokerMovement movement in relatedMovements)
                    {
                        if (_highestCard == null)
                        {
                            _highestCard = movement.HighestCard;
                        }
                        else if (movement.HighestCard.number > _highestCard.number)
                        {
                            _highestCard = movement.HighestCard;
                        }
                    }
                }

                return _highestCard;
            }
        }

        public override string ToString()
        {
            return $"Two pairs. " + relatedMovements[0] + ". " + relatedMovements[1];
        }
    }

    public sealed class ThreeOfAKindPokerMovement : PokerMovement
    {
        private CardNumber repeatedNumber;

        public ThreeOfAKindPokerMovement()
        {
            _points = 4;
            nextMovementToValidate = new TwoPairPokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            foreach (var cardNumber in System.Enum.GetValues(typeof(CardNumber)))
            {
                if (cards.Where(e => e.number == ((CardNumber)cardNumber)).Count() == 3)
                {
                    repeatedNumber = (CardNumber)cardNumber;
                    _highestCard = cards.Where(e => e.number == ((CardNumber)cardNumber)).First();
                    _cardsToKeep.AddRange(cards.Where(e => e.number == ((CardNumber)cardNumber)));
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"Three of a kind of {repeatedNumber.ToString()}";
        }
    }

    public sealed class StraightPokerMovement : PokerMovement
    {
        public bool IsRoyal { get; set; }
        public StraightPokerMovement()
        {
            _points = 5;
            IsRoyal = false;
            nextMovementToValidate = new ThreeOfAKindPokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            // Need to address Royal flush
            List<Card> ordered = cards.OrderBy(e => e).ToList();
            

            int count = 0;

            for (int i = 1; i < ordered.Count; i++)
            {
                if ((((int)ordered[i - 1].number) + 1) == (((int)ordered[i].number)))
                {
                    count++;
                }
            }
            _highestCard = ordered.Last();
            if (count == (cards.Count - 2) && ordered.First().number == CardNumber.As && ordered.Last().number == CardNumber.King)
            {
                IsRoyal = true;
                _highestCard = ordered.First();
                count++;
            }

            if (count == (cards.Count - 1)) _cardsToKeep.AddRange(ordered);
            return count == (cards.Count - 1);
        }

        public override string ToString()
        {
            return $"Straight";
        }
    }

    public sealed class FlushPokerMovement : PokerMovement
    {
        private CardType repeatedType;
        public FlushPokerMovement()
        {
            _points = 6;
            nextMovementToValidate = new StraightPokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            List<Card> ordered = cards.OrderBy(e => e).ToList();
            foreach (var cardType in System.Enum.GetValues(typeof(CardType)))
            {
                if (cards.Where(e => e.type == ((CardType)cardType)).Count() == cards.Count)
                {
                    repeatedType = (CardType)cardType;
                    _highestCard = ordered.Last();
                    _cardsToKeep.AddRange(ordered);
                    if (ordered[0].number == CardNumber.As)
                        _highestCard = ordered.First();
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"Flush";
        }
    }

    public sealed class FullHousePokerMovement : PokerMovement
    {
        public FullHousePokerMovement()
        {
            _points = 7;
            relatedMovements = new List<PokerMovement>
            {
                new ThreeOfAKindPokerMovement(),
                new PairPokerMovement()
            };
            nextMovementToValidate = new FlushPokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            List<Card> tempCards = cards.ToList();
            uint pairs = 0;

            foreach (PokerMovement movement in relatedMovements)
            {
                if (movement.ValidateMovement(tempCards))
                {
                    tempCards.RemoveAll(e => e.number == movement.HighestCard.number);
                    pairs++;
                }
            }

            if (pairs == 2)
            {
                _cardsToKeep.AddRange(tempCards);
            }
            return pairs == 2;
        }

        public override Card HighestCard
        {
            get
            {
                if (_highestCard == null)
                {
                    foreach (PokerMovement movement in relatedMovements)
                    {
                        if (_highestCard == null)
                        {
                            _highestCard = movement.HighestCard;
                        }
                        else if (movement.HighestCard.IsHigherThan(_highestCard))
                        {
                            _highestCard = movement.HighestCard;
                        }
                    }
                }

                return _highestCard;
            }
        }

        public override string ToString()
        {
            return $"Full House. " + relatedMovements[0] + ". " + relatedMovements[1];
        }
    }

    public sealed class FourOfAKindPokerMovement : PokerMovement
    {
        private CardNumber repeatedNumber;

        public FourOfAKindPokerMovement()
        {
            _points = 8;
            nextMovementToValidate = new FullHousePokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            foreach (var cardNumber in System.Enum.GetValues(typeof(CardNumber)))
            {
                if (cards.Where(e => e.number == ((CardNumber)cardNumber)).Count() == 4)
                {
                    repeatedNumber = (CardNumber)cardNumber;
                    _highestCard = cards.Where(e => e.number == ((CardNumber)cardNumber)).First();
                    _cardsToKeep.AddRange(cards.Where(e => e.number == ((CardNumber)cardNumber)));
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"Four of a kind of {repeatedNumber.ToString()}";
        }
    }

    public sealed class StraightFlushPokerMovement : PokerMovement
    {
        public StraightFlushPokerMovement()
        {
            _points = 9;
            relatedMovements = new List<PokerMovement>
            {
                new StraightPokerMovement(),
                new FlushPokerMovement()
            };
            nextMovementToValidate = new FourOfAKindPokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            uint pairs = 0;

            foreach (PokerMovement movement in relatedMovements)
            {
                if (movement.ValidateMovement(cards))
                {
                    pairs++;
                }
            }

            if (pairs == 2)
            {
                _cardsToKeep.AddRange(cards);
            }
            return pairs == 2;
        }

        public override Card HighestCard
        {
            get
            {
                if (_highestCard == null)
                {
                    foreach (PokerMovement movement in relatedMovements)
                    {
                        if (_highestCard == null)
                        {
                            _highestCard = movement.HighestCard;
                        }
                        else if (movement.HighestCard.IsHigherThan(_highestCard))
                        {
                            _highestCard = movement.HighestCard;
                        }
                    }
                }

                return _highestCard;
            }
        }

        public override string ToString()
        {
            return $"Straight Flush";
        }
    }

    public sealed class RoyalStraightFlushPokerMovement : PokerMovement
    {
        StraightPokerMovement straightPokerMovement;
        public RoyalStraightFlushPokerMovement()
        {
            _points = 10;
            straightPokerMovement = new StraightPokerMovement();
            relatedMovements = new List<PokerMovement>
            {
                straightPokerMovement,
                new FlushPokerMovement()
            };
            
            nextMovementToValidate = new StraightFlushPokerMovement();
        }

        public override bool ValidateMovement(IList<Card> cards)
        {
            uint pairs = 0;

            foreach (PokerMovement movement in relatedMovements)
            {
                if (movement.ValidateMovement(cards))
                {
                    pairs++;
                }
            }

            if (pairs == 2 && straightPokerMovement.IsRoyal)
            {
                _cardsToKeep.AddRange(cards);
            }
            return pairs == 2 && straightPokerMovement.IsRoyal;
        }

        public override Card HighestCard
        {
            get
            {
                if (_highestCard == null)
                {
                    foreach (PokerMovement movement in relatedMovements)
                    {
                        if (_highestCard == null)
                        {
                            _highestCard = movement.HighestCard;
                        }
                        else if (movement.HighestCard.IsHigherThan(_highestCard))
                        {
                            _highestCard = movement.HighestCard;
                        }
                    }
                }

                return _highestCard;
            }
        }

        public override string ToString()
        {
            return $"Royal Straight Flush";
        }
    }
}

