using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PokerTestSuite
{
    // A Test behaves as an ordinary method
    [Test]
    public void UserAWinsWithAHighCard()
    {
        // Given
        List<Card> cardsA = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Heart, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.Five),
            new Card(CardType.Spade, CardNumber.Four)
        };

        List<Card> cardsB = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Three),
            new Card(CardType.Clubs, CardNumber.Six),
            new Card(CardType.Heart, CardNumber.Jack),
            new Card(CardType.Clubs, CardNumber.Four),
            new Card(CardType.Spade, CardNumber.Ten)
        };

        // When
        PokerMovement movementA = PokerMovement.Validate(cardsA);
        PokerMovement movementB = PokerMovement.Validate(cardsB);

        // Then
        Assert.IsTrue(movementA.IsHigherThan(movementB));
    }

    [Test]
    public void UserBWinsWithAPair()
    {
        // Given
        List<Card> cardsA = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Heart, CardNumber.Two),
            new Card(CardType.Clubs, CardNumber.Five),
            new Card(CardType.Spade, CardNumber.Four)
        };

        List<Card> cardsB = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Three),
            new Card(CardType.Clubs, CardNumber.Six),
            new Card(CardType.Heart, CardNumber.Jack),
            new Card(CardType.Clubs, CardNumber.Three),
            new Card(CardType.Spade, CardNumber.Ten)
        };

        // When
        PokerMovement movementA = PokerMovement.Validate(cardsA);
        PokerMovement movementB = PokerMovement.Validate(cardsB);

        // Then
        Assert.IsFalse(movementA.IsHigherThan(movementB));
    }

    [Test]
    public void UserAWinsWithTwoPairs()
    {
        // Given
        List<Card> cardsA = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Heart, CardNumber.Seven),
            new Card(CardType.Clubs, CardNumber.Five),
            new Card(CardType.Spade, CardNumber.Four)
        };

        List<Card> cardsB = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Three),
            new Card(CardType.Clubs, CardNumber.Six),
            new Card(CardType.Heart, CardNumber.Jack),
            new Card(CardType.Clubs, CardNumber.Three),
            new Card(CardType.Spade, CardNumber.Ten)
        };

        // When
        PokerMovement movementA = PokerMovement.Validate(cardsA);
        PokerMovement movementB = PokerMovement.Validate(cardsB);

        // Then
        Assert.IsTrue(movementA.IsHigherThan(movementB));
    }

    [Test]
    public void UserBWinsWithFourOfAKind()
    {
        // Given
        List<Card> cardsA = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.As),
            new Card(CardType.Clubs, CardNumber.Seven),
            new Card(CardType.Heart, CardNumber.Seven),
            new Card(CardType.Clubs, CardNumber.Five),
            new Card(CardType.Spade, CardNumber.Four)
        };

        List<Card> cardsB = new List<Card>
        {
            new Card(CardType.Diamond, CardNumber.Three),
            new Card(CardType.Clubs, CardNumber.Six),
            new Card(CardType.Heart, CardNumber.Three),
            new Card(CardType.Clubs, CardNumber.Three),
            new Card(CardType.Spade, CardNumber.Three)
        };

        // When
        PokerMovement movementA = PokerMovement.Validate(cardsA);
        PokerMovement movementB = PokerMovement.Validate(cardsB);

        // Then
        Assert.IsFalse(movementA.IsHigherThan(movementB));
    }
}
