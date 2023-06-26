using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType: int
{
    Clubs,
    Diamond,
    Heart,
    Spade,
    Unknown
}

public enum CardNumber: int
{
    As,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

public class Card: System.IEquatable<Card>, System.IComparable<Card>, IComparer<Card>, IEqualityComparer<Card>
{
    public Card(CardType type, CardNumber number)
    {
        this.type = type;
        this.number = number;
    }
    public CardType type;
    public CardNumber number;

    public bool Equals(Card other)
    {
        return this.number == other.number && this.type == other.type;
    }

    public int CompareTo(Card other)
    {
        if (this.number == other.number)
            return 0;

        return this.number > other.number ? 1 : -1;
    }

    public override string ToString()
    {
        return $"{number} of {type}.";
    }

    public int Compare(Card x, Card y)
    {
        return x.CompareTo(y);
    }

    public bool Equals(Card x, Card y)
    {
        return x.Equals(y);
    }

    public int GetHashCode(Card obj)
    {
        return obj.number.GetHashCode() + obj.type.GetHashCode();
    }

    public bool IsHigherThan(Card obj)
    {
        if (this.number == CardNumber.As) return true;
        else if (obj.number == CardNumber.As) return false;

        return this.CompareTo(obj) == 1;
    }
}

public class CardSpace : MonoBehaviour
{
    private static string RESOURCES_FOLDER = "Third_Party/Cardpacks";
    private static string HIDE_CARD = "Backside/card-back05";

    public Card Card { get; private set; }
    [SerializeField]
    private SpriteRenderer cardRenderer;
    [SerializeField]
    private SpriteRenderer cardSelectedRenderer;
    public BoxCollider2D boxCollider2D;

    public bool IsSelected { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCard(string spriteName)
    {
        Sprite texture = Resources.Load<Sprite>(spriteName);

        cardRenderer.sprite = texture;
    }

    private void EnableSelection()
    {
        cardSelectedRenderer.gameObject.SetActive(IsSelected);
    }

    public void SetCard(Card card)
    {
        this.Card = card;

        string spriteName = $"{RESOURCES_FOLDER}/{card.type.ToString()}/{card.type.ToString().ToLower()}{(((int)card.number) + 1).ToString().PadLeft(2, '0')}";
        SetCard(spriteName);
    }

    public void HideCard()
    {
        string spriteName = $"{RESOURCES_FOLDER}/{HIDE_CARD}";
        SetCard(spriteName);
    }

    
    public void Select()
    {
        IsSelected = true;
        EnableSelection();
    }

    public void Deselect()
    {
        IsSelected = false;
        EnableSelection();
    }
}
