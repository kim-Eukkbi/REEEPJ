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
    public GameObject frontCard;
    public Material redMat;
    public Material GreenMat;

    private Image thisCard;

    private void Start()
    {
        thisCard = gameObject.GetComponent<Image>();
    }


    public void Initialize(Card drawCard)
    {
        DecObj = GameObject.Find("DesObj");
        card = drawCard;
        illuset.sprite = card.power.illuset;
        cardName.text = card.power.cardName;
        if(card.isDamageCard)
            cardDamage.text = "Power :" + card.damage.ToString();
        else if(card.isHealCard)
            cardDamage.text = "Heal :" + card.heal.ToString();
        cardDescription = card.power.cardDescription;
        if (card.isDamageCard)
            frontCard.GetComponent<MeshRenderer>().material = redMat;
        if (card.isHealCard)
            frontCard.GetComponent<MeshRenderer>().material = GreenMat;
    }

    public void UseCard()
    {
        if (card.isDamageCard)
        {
            if (DamageManager.Instance.nowDamage <= card.damage)
                DamageManager.Instance.DamagePlayer(card.damage, true);
            else
            {
                DamageManager.Instance.DamagePlayer(card.damage, false);
            }
        }
        else if(card.isHealCard)
        {
            DamageManager.Instance.HealPlayer(card.heal);
        }
    }
}
