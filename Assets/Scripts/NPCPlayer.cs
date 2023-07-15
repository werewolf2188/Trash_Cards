using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCPlayer : Player
{
    private bool hideCards = true;
    void Awake()
    {
        GetCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetCards(IList<Card> cards)
    {
        if (hideCards)
        {
            base.SetCards(cards);
            for (int index = 0; index < Dealer.NUMBER_OF_CARDS; index++)
            {
                CardSpaces[index].HideCard();
            }
        }
        else
        {
            ReplaceSelectedCards(cards);

            ValidateMovement("NPC");

            base.SetCards(this.CardSpaces.Select(e => e.Card).ToList());
            Dealer.MainDealer.ValidateMovements();
        }
    }

    // Test
    public void ProcessCards()
    {
        hideCards = false;
        CurrentMovement = PokerMovement.Validate(this.CardSpaces.Select(e => e.Card).ToList());

        foreach(CardSpace cardSpace in this.CardSpaces)
        {
            if (!CurrentMovement.CardsToKeep.Contains(cardSpace.Card))
            {
                cardSpace.Select();
            }
        }
        Dealer.MainDealer.DealCards(this, CardSpaces.Where(e => e.IsSelected).Count());
    }

    public override void Restart()
    {
        base.Restart();
        hideCards = true;
    }
}
