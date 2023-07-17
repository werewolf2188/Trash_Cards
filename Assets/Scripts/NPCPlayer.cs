using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface INPCReaction
{
    void React(NPCPlayer.Emotions emotion);
    void Initialize(NPCPlayer.Characters character, NPCPlayer.Emotions initialEmotion);
}

public class NPCPlayer : Player
{
    public enum Emotions
    {
        Angry, Generic, Sad, Smile, Smirk, ZZ
    }

    public enum Characters
    {
        Afro, Baldy, Clown, Cop, FireFighter, GenericLongHair, HeroBlonde, HeroGreen, HeroPurple, LongHair, OneEyed,
        Pirate, Punk, PurpleHat, Rapper, RedHatBoy, RedIndian, Soldier, Thief, WrestlerA, WrestlerB
    }

    private bool hideCards = true;
    // TODO: This will be deleted since the final assertion will be with money
    private uint points = 0;
    // TODO: This will come from the user
    [SerializeField]
    private Characters character;
    // TODO: This will be deleted since the final assertion will be with money
    [SerializeField]
    [Range(2, 5)]
    private int differenceOfPoints; // Difference of points to allow different emotions

    private NPCSpeech npcSpeech;
    private NPCFace npcFace;

    void Awake()
    {
        npcSpeech = GetComponent<NPCSpeech>();
        npcFace = GetComponent<NPCFace>();
        GetCards();
    }

    void Start()
    {
        npcSpeech.Initialize(character, Emotions.Generic);
        npcFace.Initialize(character, Emotions.Generic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void React(Emotions emotion)
    {
        npcSpeech.React(emotion);
        npcFace.React(emotion);
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

    public void CompareWith(UserPlayer player)
    {
        // TODO: Change logic based on money, not points
        if (points == player.GetPoints())
        {
            React(Emotions.Generic);
        }
        else if (points > player.GetPoints() && points <= (player.GetPoints() + differenceOfPoints))
        {
            React(Emotions.Smirk);
        }
        else if (points > (player.GetPoints() + differenceOfPoints))
        {
            React(Emotions.Smile);
        }
        else if (points < (player.GetPoints() - differenceOfPoints))
        {
            React(Emotions.Angry);
        }
        else if (points < (player.GetPoints()))
        {
            React(Emotions.Sad);
        }

    }

    public override void Win()
    {
        points++;
    }

    public override void Lose()
    {

    }
}
