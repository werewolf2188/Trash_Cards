using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CardTestSuite
{
    // A Test behaves as an ordinary method
    [Test]
    public void CardsAreEqual()
    {
        Card c1 = new Card(CardType.Clubs, CardNumber.Five);
        Card c2 = new Card(CardType.Clubs, CardNumber.Five);

        Assert.AreEqual(c1, c2);
    }

    [Test]
    public void CardsAreNotEqual()
    {
        Card c1 = new Card(CardType.Clubs, CardNumber.Five);
        Card c2 = new Card(CardType.Clubs, CardNumber.Six);

        Assert.AreNotEqual(c1, c2);

        c1 = new Card(CardType.Clubs, CardNumber.Five);
        c2 = new Card(CardType.Diamond, CardNumber.Five);

        Assert.AreNotEqual(c1, c2);
    }

    [Test]
    public void CardsIsHigher()
    {
        Card c1 = new Card(CardType.Diamond, CardNumber.Nine);
        Card c2 = new Card(CardType.Clubs, CardNumber.Six);

        Assert.IsTrue(c1.IsHigherThan(c2));
    }

    [Test]
    public void CardsIsHigherWithAs()
    {
        Card c1 = new Card(CardType.Diamond, CardNumber.Nine);
        Card c2 = new Card(CardType.Clubs, CardNumber.As);

        Assert.IsFalse(c1.IsHigherThan(c2));
    }

    [Test]
    public void CardsIsLower()
    {
        Card c1 = new Card(CardType.Diamond, CardNumber.Two);
        Card c2 = new Card(CardType.Clubs, CardNumber.Six);

        Assert.IsFalse(c1.IsHigherThan(c2));
    }
}
