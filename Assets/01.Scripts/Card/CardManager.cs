using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardManager : MonoBehaviour
{

    public GameObject cardPrefab;
    public Transform cardInstantiateRectPos;
    public Transform cardInstantiatePos;
    public DropArea defaultDropArea;
    public GameObject DesObj;
    public GameObject NowDamage;
    public GameObject TotalDamage;

    public Deck initialDeck;
    private Deck playerDeck;

    public List<CardHandler> cardsInHand;
   // public List<CardHandler> cardsUsed;

    private int firstCount =0;
    private bool isEndCardSpawn = false;

    public void Start()
    {
        playerDeck = initialDeck.Clone();
        firstCount = playerDeck.deck.Count;
        StartCoroutine(InstantiateCo());
    }

    public void Draw()
    {
        if (isEndCardSpawn)
        {
            GameObject drawCard = cardsInHand[firstCount - 1].gameObject;
            DrawSeq(drawCard);
            firstCount--;
        }
        else
        {
            Debug.LogWarning("카드 스폰이 아직 완료되지 않았습니다");
        }
    }

    private void DrawSeq(GameObject drawCard)
    {
        Sequence drawSeq = DOTween.Sequence();
        Vector3 drawCardPos = drawCard.transform.position;
        drawSeq.Append(drawCard.transform.DOMove(new Vector3(drawCardPos.x - 3, drawCardPos.y, drawCardPos.z), .5f));
        drawSeq.Insert(.1f,drawCard.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), .5f));
        drawSeq.Append(drawCard.transform.DOMove(cardInstantiateRectPos.position, .1f)).OnComplete(()=>
        {
            drawCard.transform.SetParent(cardInstantiateRectPos);
            drawCard.GetComponent<DropItem>().droppedArea = defaultDropArea;
        });
    }

    private void InstantiateCardObject(int index)
    {
        GameObject cardObject;
        Card card = playerDeck.Draw();
        if (card != null)
        {
            cardObject = Instantiate(cardPrefab,cardInstantiatePos.position,Quaternion.Euler(0,180,0), cardInstantiatePos);
            cardObject.transform.Translate(new Vector3(-.01f, .01f,.01f) * index);
            cardObject.GetComponent<CardHandler>().Initialize(card);
            cardObject.GetComponent<DropItem>().descriptionObj = DesObj;
            cardsInHand.Add(cardObject.GetComponent<CardHandler>());
        }
        else
        {
            Debug.LogWarning("덱에 더이상 카드가 존재하지 않습니다");
            return;
        }
    }


    private IEnumerator InstantiateCo()
    {
        for(int i =0; i< firstCount; i++)
        {
            InstantiateCardObject(i);
            yield return new WaitForSeconds(0.1f);
        }
        isEndCardSpawn = true;
    }
}
