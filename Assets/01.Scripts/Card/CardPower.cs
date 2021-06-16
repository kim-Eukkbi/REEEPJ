using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewCardPower",menuName = "PJ/GGMCardGame/CardPower")]
public class CardPower : ScriptableObject
{
    public Sprite illuset;
    public string cardName;
    public string cardDescription;
    public float cardDamage;

    public string seqOnUse;
    public string seqOnDraw;
    public string seqOnDrop;
    public string seqTurnEnd;

    public void Init(Sprite _illust,string _name,string _description, float _cardDamage,string _seqOnUse,
        string _seqOnDraw,string _seqOnDrop,string _seqTurnEnd)
    {
        this.illuset = _illust;
        this.cardName = _name;
        this.cardDescription = _description;
        this.cardDamage = _cardDamage;
        this.seqOnUse = _seqOnUse;
        this.seqOnDraw = _seqOnDraw;
        this.seqOnDrop = _seqOnDrop;
        this.seqTurnEnd = _seqTurnEnd;
    }
}
