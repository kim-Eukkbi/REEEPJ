    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    public Card card;
   
    public GameObject DecObj;
    public Image illuset;
    public Text cardName;
    public Text cardDamage;
    public string cardDescription;
    public GameObject NowDamage;
    public GameObject totalDamage;


    public void Awake()
    {
//        Initialize(card);
    }

    public void Initialize(Card drawCard)
    {
        DecObj = GameObject.Find("DesObj");
        card = drawCard;
        illuset.sprite = card.power.illuset;
        cardName.text = card.power.cardName;
        cardDamage.text = "Power :" + card.power.cardDamage.ToString();
        cardDescription = card.power.cardDescription;
    }

    public void UseCard()
    {
        DamageManager.Instance.SetDamage(card.damage);
    }
}
