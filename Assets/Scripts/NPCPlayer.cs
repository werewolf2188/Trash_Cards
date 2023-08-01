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
    private bool? won = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI moneyAmountText;

    private NPCSpeech npcSpeech;
    private NPCFace npcFace;
    private NPCSound nPCSound;

    void Awake()
    {
        npcSpeech = GetComponent<NPCSpeech>();
        npcFace = GetComponent<NPCFace>();
        nPCSound = GetComponent<NPCSound>();
        GetCards();
    }

    void Start()
    {
        NPCInfo info = NPCInfo.Default;
        Characters character = info != null ? info.GetFace() : Characters.HeroBlonde;
        npcSpeech.Initialize(character, Emotions.Generic);
        npcFace.Initialize(character, Emotions.Generic);
        nPCSound.Initialize(character, Emotions.Generic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void React(Emotions emotion)
    {
        npcSpeech.React(emotion);
        npcFace.React(emotion);
        nPCSound.React(emotion);
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
        if (won == null)
        {
            React(Emotions.ZZ);
        }
        bool _won = won ?? false;
        if (_won && moneyAmount > player.GetMoney())
            React(Emotions.Smile);
        else if (_won && moneyAmount == player.GetMoney())
            React(Emotions.Generic);
        else if (_won && moneyAmount < player.GetMoney())
            React(Emotions.Smirk);
        else if (!_won && moneyAmount >= player.GetMoney())
            React(Emotions.Sad);
        else if (!_won && moneyAmount < player.GetMoney())
            React(Emotions.Angry);

        won = null;
    }

    public override void SetInitialMoneyAmount(float money)
    {
        base.SetInitialMoneyAmount(money);
        SetAmount();
    }

    public override void Win()
    {
        moneyAmount += Dealer.MainDealer.Bet;
        won = true;
        SetAmount();
    }

    public override void Lose()
    {
        moneyAmount -= Dealer.MainDealer.Bet;
        won = false;
        SetAmount();
    }

    private void SetAmount()
    {
        moneyAmountText.text = $"NPC: ${moneyAmount}";
    }

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(100, 30, 50, 30), "Angry"))
    //    {
    //        React(Emotions.Angry);
    //    }
    //    if (GUI.Button(new Rect(160, 30, 50, 30), "Sad"))
    //    {
    //        React(Emotions.Sad);
    //    }
    //    if (GUI.Button(new Rect(220, 30, 60, 30), "Generic"))
    //    {
    //        React(Emotions.Generic);
    //    }
    //    if (GUI.Button(new Rect(290, 30, 50, 30), "Smirk"))
    //    {
    //        React(Emotions.Smirk);
    //    }
    //    if (GUI.Button(new Rect(350, 30, 50, 30), "Smile"))
    //    {
    //        React(Emotions.Smile);
    //    }
    //    if (GUI.Button(new Rect(410, 30, 50, 30), "ZZ"))
    //    {
    //        React(Emotions.ZZ);
    //    }
    //}
}
