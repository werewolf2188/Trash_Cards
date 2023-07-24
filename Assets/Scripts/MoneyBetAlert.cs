using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MoneyBetAlert : MonoBehaviour
{
    private float amount;
    private float maximum;
    private const float increment = 10;
    [SerializeField]
    private UnityEngine.UI.Image mainPanel;
    [SerializeField]
    private TMP_InputField amountInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    void OnAccept()
    {
        if (amount == 0) return;

        GetComponent<RectTransform>().LeanAlphaText(0, 0.8f).setOnComplete(() => {
            this.gameObject.SetActive(false);
            Dealer.MainDealer.SetBet(this.amount);
        });
    }

    [SerializeField]
    void OnCancel()
    {
        SceneManager.LoadScene((int)TrashCardsScenes.Menu);
    }

    [SerializeField]
    void OnPlus()
    {
        amount += increment;
        if (amount > maximum)
            amount = maximum;
        amountInput.text = $"${amount}";
    }

    [SerializeField]
    void OnMinus()
    {
        amount -= increment;
        if (amount < increment)
            amount = increment;
        amountInput.text = $"${amount}";
    }

    public void Open(float maximum)
    {
        amountInput.text = $"${increment}";
        amount = increment;
        this.maximum = maximum;
        this.gameObject.SetActive(true);
    }

    public bool IsOpen
    {
        get
        {
            return this.gameObject.activeInHierarchy;
        }
    }

    public float Amount
    {
        get
        {
            return amount;
        }
    }
}
