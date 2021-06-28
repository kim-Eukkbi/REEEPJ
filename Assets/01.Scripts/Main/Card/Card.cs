using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "PJ/GGMCardGame/Card")]
public class Card : ScriptableObject

{
    public string id;
    public string tagString;

    public bool isDamageCard;
    public bool isHealCard;
    public bool isBenefitCard;

    public CardPower power;

    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float heal;

    public void Init(string _id, string _tagString,
        CardPower defualtCP,bool _damageCard,bool _healCard)
    {
        power = defualtCP;
        this.id = _id;
        this.tagString = _tagString;
        this.damage = power.cardDamage;
        this.heal = power.cardHeal;
        this.isDamageCard = _damageCard;
        this.isHealCard = _healCard;
        //this.isBenefitCard = _isBenefitCard;
    }

    public Card clone()
    {
        var card = CreateInstance<Card>();
        switch (power.cardName)
        {
            case "»≠º∫ ∞•≤Ù¥œ±Ó~":
                power.cardDamage = Random.Range(1, 81);
                power.cardHeal = Random.Range(1, 81);
                if (Random.Range(0, 2).Equals(0))
                {
                    isDamageCard = true;
                    isHealCard = false;
                }
                else
                {
                    isDamageCard = false;
                    isHealCard = true;
                }
                break;
            case "¿œ∞›ø°¡÷¥‘∞Á¿∏∑Œ":
                break;
        }
        card.Init(id, tagString, power, isDamageCard, isHealCard);
        return card;
    }

    public float OnUse()
    {
        if(isDamageCard)
        {
            return damage;
        }
        else if(isHealCard)
        {
            return heal;
        }
        return 0;
    }

    public void OnDraw()
    {
        Debug.Log("Use Draw :" + power.cardName);
    }

    public void OnDrop()
    {
        Debug.Log("Use Drop :" + power.cardName);
    }

    public void OnTurnEnd()
    {
        Debug.Log("TurnEnd :" + power.cardName);
    }
}
