using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewDeck", menuName = "PJ/GGMCardGame/Deck")]
public class Deck : ScriptableObject
{
    public List<Card> deck;

    public void Init()
    {
        deck = new List<Card>();
    }

    public void Put(Card card)
    {
        Debug.Assert(card != null);
        deck.Add(card);
    }

    public void Put(Deck deck)
    {
        var cards = deck.DrawAll();
        this.deck.AddRange(cards);
    }

    public Card Draw()
    {
        if (deck.Count == 0)
            return null;

        Shuffle();
        Card card = deck.Last();
        deck.Remove(card);
        return card;
    }

    public List<Card> DrawAll()
    {
        var cards = new List<Card>(deck);
        deck.Clear();
        return cards;
    }

    public void Shuffle()
    {
        var rand = new System.Random();
        int n = deck.Count;
        while(n>0)
        {
            n--;
            int k = rand.Next(n + 1);
            var value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }

    public Deck Clone()
    {
        Deck newDeck = CreateInstance<Deck>();
        newDeck.Init();

        foreach(var card in deck)
        {
            newDeck.Put(card.clone());
        }

        return newDeck;
    }
}
