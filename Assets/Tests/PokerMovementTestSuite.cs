using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PokerMovementTestSuite
{
    [Test]
    public void UserHasLessThan5Cards()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Seven)
        };

        // When
        System.ArgumentException ex = Assert.Throws<System.ArgumentException>(() => PokerMovement.Validate(cards));


        // Then        
        Assert.That(ex.Message, Is.EqualTo("Cards should be 5"));
    }

    [Test]
    public void UserHasMoreThan5Cards()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Spade, CardNumber.Four),
            new Card(CardType.Spade, CardNumber.Five)
        };

        // When
        System.ArgumentException ex = Assert.Throws<System.ArgumentException>(() => PokerMovement.Validate(cards));


        // Then        
        Assert.That(ex.Message, Is.EqualTo("Cards should be 5"));
    }

    [Test]
    public void UserHasDuplicateCards()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Heart, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Spade, CardNumber.Four)
        };

        // When
        System.ArgumentException ex = Assert.Throws<System.ArgumentException>(() => PokerMovement.Validate(cards));


        // Then        
        Assert.That(ex.Message, Is.EqualTo("Card is duplicate: Seven of Clubs."));
    }

    [Test]
    public void UserOnlyHasAHighCard()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Spade, CardNumber.Four)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.IsNotNull(movement as Movements.HighestPokerMovement);
    }

    [Test]
    public void UserOnlyHasAHighCardAndItIsAnAs()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Spade, CardNumber.Four)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards) as Movements.HighestPokerMovement;

        // Then
        
        Assert.IsNotNull(movement);
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(1));
        Assert.That(movement.ToString(), Is.EqualTo("Highest card: As"));
    }

    [Test]
    public void UserHasAPair()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Spade, CardNumber.King)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(2));
        Assert.IsNotNull(movement as Movements.PairPokerMovement);
    }

    [Test]
    public void UserHasTwoPairs()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Ten),
            new Card(CardType.Spade, CardNumber.King)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(4));
        Assert.IsNotNull(movement as Movements.TwoPairPokerMovement);
    }

    [Test]
    public void UserHasAThreeOfAKind()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.King),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Spade, CardNumber.King)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(3));
        Assert.IsNotNull(movement as Movements.ThreeOfAKindPokerMovement);
    }

    [Test]
    public void UserHasAStraight()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Six),
            new Card(CardType.Clubs, CardNumber.Five),
            new Card(CardType.Heart, CardNumber.Four),
            new Card(CardType.Clubs, CardNumber.Three),
            new Card(CardType.Spade, CardNumber.Two)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(5));
        Assert.IsNotNull(movement as Movements.StraightPokerMovement);
    }

    [Test]
    public void UserHasAStraightWithAnAsAtTheBottom()
    {
        //Debug.Log("UserHasAStraightWithAnAsAtTheBottom");
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Clubs, CardNumber.Five),
            new Card(CardType.Heart, CardNumber.Four),
            new Card(CardType.Clubs, CardNumber.Three),
            new Card(CardType.Spade, CardNumber.Two)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(5));
        Assert.IsNotNull(movement as Movements.StraightPokerMovement);
    }

    [Test]
    public void UserHasAStraightWithAnAsAtTheTop()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.Queen),
            new Card(CardType.Clubs, CardNumber.Jack),
            new Card(CardType.Spade, CardNumber.Ten)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(5));
        Assert.IsNotNull(movement as Movements.StraightPokerMovement);
    }

    [Test]
    public void UserHasNotStraightButHasAs()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Clubs, CardNumber.Nine),
            new Card(CardType.Heart, CardNumber.Queen),
            new Card(CardType.Clubs, CardNumber.Jack),
            new Card(CardType.Spade, CardNumber.Ten)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.IsNotNull(movement as Movements.HighestPokerMovement);
    }

    [Test]
    public void UserOnlyHasAFlush()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Clubs, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Clubs, CardNumber.Ten),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Clubs, CardNumber.Four)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(5));
        Assert.IsNotNull(movement as Movements.FlushPokerMovement);
    }

    [Test]
    public void UserHasAFourOfAKind()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.King),
            new Card(CardType.Clubs, CardNumber.King),
            new Card(CardType.Heart, CardNumber.King),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Spade, CardNumber.King)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(4));
        Assert.IsNotNull(movement as Movements.FourOfAKindPokerMovement);
    }

    [Test]
    public void UserHasAStraightFlush()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Six),
            new Card(CardType.Diamond, CardNumber.Five),
            new Card(CardType.Diamond, CardNumber.Four),
            new Card(CardType.Diamond, CardNumber.Three),
            new Card(CardType.Diamond, CardNumber.Two)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(5));
        Assert.IsNotNull(movement as Movements.StraightFlushPokerMovement);
    }

    [Test]
    public void UserHasARoyalStraightFlush()
    {
        // Given
        List<Card> cards = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Diamond, CardNumber.King),
            new Card(CardType.Diamond, CardNumber.Queen),
            new Card(CardType.Diamond, CardNumber.Jack),
            new Card(CardType.Diamond, CardNumber.Ten)
        };

        // When
        PokerMovement movement = PokerMovement.Validate(cards);

        // Then
        Assert.That(movement.CardsToKeep.Length, Is.EqualTo(5));
        Assert.IsNotNull(movement as Movements.RoyalStraightFlushPokerMovement);
    }
}
