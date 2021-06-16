using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "PJ/GGMCardGame/Card")]
public class Card : ScriptableObject

{
    public string id;
    public string tagString;
    public float damage;

    public bool usable;
    public bool dispoasable;

    public CardPower power;

    public void Init(string _id, string _tagString, float _damege,
        CardPower defualtCP,bool dispose = false,bool usable = true)
    {
        this.id = _id;
        this.tagString = _tagString;
        this.damage = _damege;
        this.dispoasable = dispose;
        this.usable = usable;
        power = defualtCP;
    }

    public Card clone(bool setDispose = false)
    {
        var card = CreateInstance<Card>();
        bool dispose = setDispose || this.dispoasable;
        card.Init(id, tagString,damage, power, dispose);
        return card;
    }

    public void OnUse()
    {
        Debug.Log("Use Card :" + power.cardName);
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
