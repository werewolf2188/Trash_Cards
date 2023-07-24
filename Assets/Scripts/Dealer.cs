using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dealer : MonoBehaviour
{
    private static string USER_TAG = "Player";
    private static string ENEMY_TAG = "NPC";

    private static Dealer _mainDealer;
    public static Dealer MainDealer { get { return _mainDealer; } }

    public static int NUMBER_OF_CARDS = 5;

    private const int minimumOfCardsAvailableForNextSession = 20;
    private const float initialMoney = 500;

    private List<Card> cards;

    private UserPlayer player;
    private NPCPlayer npc;

    private float moneyBet;

    [SerializeField]
    private TMPro.TextMeshProUGUI resultText;
    [SerializeField]
    private TMPro.TextMeshProUGUI betIsText;
    [SerializeField]
    private UnityEngine.UI.Button dealButton;
    [SerializeField]
    private MoneyBetAlert betMoney;

    private void Awake()
    {
        if (_mainDealer == null)
            _mainDealer = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cards = new List<Card>();
        CreateCards();

        cards.Shuffle();

        // Look for User Player
        player = GameObject.FindGameObjectWithTag(USER_TAG).GetComponent<UserPlayer>();
        player.SetInitialMoneyAmount(initialMoney);
        // Look for NPC Player
        npc = GameObject.FindGameObjectWithTag(ENEMY_TAG).GetComponent<NPCPlayer>();
        npc.SetInitialMoneyAmount(initialMoney);

        // Give cards
        DealCards(player, NUMBER_OF_CARDS);
        DealCards(npc, NUMBER_OF_CARDS);

        Restart();
        betMoney.Open(initialMoney);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateCards()
    {
        cards.Clear();
        foreach (var cardType in System.Enum.GetValues(typeof(CardType)))
        {
            if (((CardType)cardType) == CardType.Unknown) break;
            foreach (var cardNumber in System.Enum.GetValues(typeof(CardNumber)))
            {
                cards.Add(new Card((CardType)cardType, (CardNumber)cardNumber));
            }
        }

        Debug.Assert(cards.Count == 52, "Count is incorrect");
    }

    public void DealCards(Player player, int numberOfCards)
    {
        player.SetCards(cards.Take(numberOfCards).ToList());
        cards.RemoveRange(0, numberOfCards);
    }

    public void NotifyNPC()
    {
        npc.ProcessCards();
    }

    public void ValidateMovements()
    {
        player.FinishSession();
        dealButton.gameObject.SetActive(true);
        if (player.CurrentMovement.IsHigherThan(npc.CurrentMovement))
        {
            resultText.text = "You win!!!";
            npc.Lose();
            player.Win();
        }
        else if (npc.CurrentMovement.IsHigherThan(player.CurrentMovement))
        {
            resultText.text = "You Lose...";
            player.Lose();
            npc.Win();
        }
        else
        {
            resultText.text = "Draw";
        }
        betIsText.gameObject.SetActive(false);
        moneyBet = 0;
        npc.CompareWith(player);
        if (player.GetMoney() == 0 || npc.GetMoney() == 0)
        {
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator GameOver()
    {
        dealButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(10);
        resultText.text = "Game Over!";
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene((int)TrashCardsScenes.Menu);
    }

    private void Restart()
    {
        resultText.text = "";
        dealButton.gameObject.SetActive(false);
    }

    public void Deal()
    {
        Restart();
        player.Restart();
        npc.Restart();

        if (cards.Count < minimumOfCardsAvailableForNextSession)
        {
            CreateCards();
            cards.Shuffle();
            Debug.Log("Dealer is shuffling new cards");
        }

        DealCards(player, NUMBER_OF_CARDS);
        DealCards(npc, NUMBER_OF_CARDS);
        betMoney.Open(Mathf.Min(player.GetMoney(), npc.GetMoney()));
    }

    public bool PlayersAreBetting
    {
        get
        {
            return betMoney.IsOpen;
        }
    }

    public void SetBet(float money)
    {
        moneyBet = money;
        betIsText.text = $"Bet is: ${moneyBet}";
        betIsText.gameObject.SetActive(true);
    }

    public float Bet
    {
        get
        {
            return moneyBet;
        }
    }
}
