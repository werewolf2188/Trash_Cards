using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class UserPlayer : Player
{
    private bool mouseClicked = false;
    private bool dealerAsked = false;
    private bool sessionFinished = false;
    // TODO: This will be deleted since the final assertion will be with money
    private uint points = 0;
    [SerializeField]
    private UnityEngine.UI.Button button;

    void Awake()
    {
        GetCards();
    }

    void Start()
    {
        button.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (sessionFinished) return;

        LookForACard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseClicked = true;
        }
    }

    private void LookForACard()
    {
        if (mouseClicked)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (CardSpaces.Any(e => e.boxCollider2D == hit.collider))
                {
                    CardSpace cardSpace = CardSpaces.Find(e => e.boxCollider2D == hit.collider);
                    if (cardSpace.IsSelected)
                        cardSpace.Deselect();
                    else cardSpace.Select();
                    button.gameObject.SetActive(true);
                }
            }
            mouseClicked = false;
        }
    }

    public override void SetCards(IList<Card> cards)
    {
        if (dealerAsked)
        {
            ReplaceSelectedCards(cards);
            dealerAsked = false;
            button.gameObject.SetActive(false);

            ValidateMovement("User");
            return;
        }
        base.SetCards(cards);
        
    }

    public void AskDealerToHit()
    {
        dealerAsked = true;
        Dealer.MainDealer.DealCards(this, CardSpaces.Where(e => e.IsSelected).Count());
        Dealer.MainDealer.NotifyNPC();
    }

    public void FinishSession()
    {
        sessionFinished = true;
    }

    public override void Restart()
    {
        base.Restart();
        sessionFinished = false;
    }

    public override void Win()
    {
        points++;
    }

    public override void Lose()
    {

    }

    // Test
    public uint GetPoints()
    {
        return points;
    }
}
