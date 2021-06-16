using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public GameObject cardPrefab;
    public Transform cardInstantiatePos;
    public DropArea defaultDropArea;
    public GameObject DesObj;

    public Deck initialDeck;
    private Deck playerDeck;

    public List<CardHandler> cardsInHand;
   // public List<CardHandler> cardsUsed;

    public void Start()
    {
        playerDeck = initialDeck.Clone();
    }

    public void Draw()
    {
        InstantiateCardObject();
    }

    private void InstantiateCardObject()
    {
        GameObject cardObject;
        Card card = playerDeck.Draw();
        if (card != null)
        {
            cardObject = Instantiate(cardPrefab, this.transform);
            cardObject.GetComponent<CardHandler>().Initialize(card);
            cardsInHand.Add(cardObject.GetComponent<CardHandler>());
            cardObject.GetComponent<DropItem>().droppedArea = defaultDropArea;
            cardObject.GetComponent<DropItem>().descriptionObj = DesObj;
            cardObject.transform.SetParent(cardInstantiatePos);
        }
        else
        {
            Debug.LogWarning("덱에 더이상 카드가 존재하지 않습니다");
            return;
        }
    }

    private void UseCard()
    {

    }
}
