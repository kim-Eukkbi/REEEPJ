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

    public void Awake()
    {
//        Initialize(card);
    }

    public void Initialize(Card drawCard)
    {
        DecObj = GameObject.Find("DesObj");
        card = drawCard;
        illuset.sprite = drawCard.power.illuset;
        cardName.text = drawCard.power.cardName;
        cardDamage.text = "Power :" + drawCard.power.cardDamage.ToString();
        cardDescription = drawCard.power.cardDescription;
    }

    public void UseCard()
    {
        card.OnUse();
    }
}
