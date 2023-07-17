using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public List<CardSpace> CardSpaces { get; private set; }
    public PokerMovement CurrentMovement { get; protected set; }

    protected void GetCards()
    {
        CardSpaces = GetComponentsInChildren<CardSpace>().ToList();
        Debug.Assert(CardSpaces.Count() == Dealer.NUMBER_OF_CARDS, $"The player should have {Dealer.NUMBER_OF_CARDS} card spaces");
    }

    public virtual void SetCards(IList<Card> cards)
    {
        Debug.Assert(cards.Count() == Dealer.NUMBER_OF_CARDS, $"The player should get {Dealer.NUMBER_OF_CARDS} cards");
        for (int index = 0; index < Dealer.NUMBER_OF_CARDS; index++)
        {
            CardSpaces[index].SetCard(cards[index]);
        }
    }

    protected void ReplaceSelectedCards(IList<Card> cards)
    {
        Debug.Assert(CardSpaces.Where(e => e.IsSelected).Count() == cards.Count, "Request cards are not the same as received.");
        IList<CardSpace> SelectedCardSpaces = CardSpaces.Where(e => e.IsSelected).ToList();
        for (int index = 0; index < cards.Count; index++)
        {
            SelectedCardSpaces[index].SetCard(cards[index]);
            SelectedCardSpaces[index].Deselect();
        }
    }

    protected void ValidateMovement(string name)
    {
        CurrentMovement = PokerMovement.Validate(CardSpaces.Select(e => e.Card).ToList());

        if (CurrentMovement != null)
        {
            Debug.Log(name + " Player has: " + CurrentMovement + $". Highest card: {CurrentMovement.HighestCard}");
        }

        foreach(CardSpace space in CardSpaces)
        {
            if (CurrentMovement.CardsToKeep.Contains(space.Card))
            {
                space.Select();
            }
        }
    }

    public virtual void Restart()
    {
        CardSpaces.ForEach(e => e.Deselect());
    }

    public abstract void Win();
    public abstract void Lose();
}