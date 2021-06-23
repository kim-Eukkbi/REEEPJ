using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class CardManager : MonoBehaviourPun
{
    public GameObject cardPrefab;
    public GameObject ponCardprefab;
    public Transform cardInstantiateRectPos;
    public Transform cardInstantiatePos;
    public DropArea defaultDropArea;
    public GameObject DesObj;
    public GameObject NowDamage;
    public GameObject TotalDamage;

    public Deck initialDeck;
    private Deck playerDeck;

    public List<CardHandler> cardsOnSpwan;
    public List<CardHandler> cardsInHand;
    public List<CardHandler> cardsUsed;

    public List<GameObject> ponCardList = new List<GameObject>();

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
        if (Test.isMyturn)
        {
            if (isEndCardSpawn)
            {
                if (cardsInHand.Count < 9)
                {
                    GameObject drawCard = cardsOnSpwan[firstCount - 1].gameObject;
                    ponCardList.Add(Instantiate(ponCardprefab, cardInstantiatePos.position, Quaternion.identity, cardInstantiateRectPos));
                    DrawSeq(drawCard);
                    firstCount--;
                }
                else
                {
                    Debug.LogWarning("소지할 수 있는 카드의 양을 넘었습니다");
                }
            }
            else
            {
                Debug.LogWarning("카드 스폰이 아직 완료되지 않았습니다");
            }
        }
    }

    public void OrderCard()
    {
        for(int i =0;i<ponCardList.Count;i++)
        {
            cardsInHand[i].transform.DOMove(ponCardList[i].transform.position, 1f);
        }
    }

    public int CheckCard(GameObject gameObject)
    {
        for(int i =0;i<cardsInHand.Count;i++)
        {
            if (gameObject.Equals(cardsInHand[i]))
                return i;
        }
        return 0;
    }


    private void DrawSeq(GameObject drawCard)
    {
        Sequence drawSeq = DOTween.Sequence();
        Vector3 drawCardPos = drawCard.transform.position;
        CardHandler drawCardHandler = drawCard.GetComponent<CardHandler>();
        cardsOnSpwan.Remove(drawCardHandler);
        cardsInHand.Add(drawCardHandler);
        drawSeq.Append(drawCard.transform.DOMove(new Vector3(drawCardPos.x - 3, drawCardPos.y, drawCardPos.z - 2), .5f));
        drawSeq.Insert(.1f, drawCard.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), .5f)).OnComplete(() => 
        {
            drawCard.GetComponent<DropItem>().droppedArea = defaultDropArea;
            OrderCard();
        }); 
    }

    private void InstantiateCardObject(int index)
    {
        GameObject cardObject;
        Card card = playerDeck.Draw();
        Sequence instSeq = DOTween.Sequence();
        if (card != null)
        {
            cardObject = Instantiate(cardPrefab,cardInstantiatePos.position + new Vector3(10, 0, 0), Quaternion.Euler(0, 180, 0),cardInstantiatePos);
            instSeq.Append(cardObject.transform.DOMove(cardInstantiatePos.position, .05f)).OnComplete(() =>
            {
                 cardObject.transform.Translate(new Vector3(-.01f, .01f, .01f) * index);
                 cardObject.GetComponent<CardHandler>().Initialize(card);
                 cardObject.GetComponent<DropItem>().descriptionObj = DesObj;
                 cardsOnSpwan.Add(cardObject.GetComponent<CardHandler>());
            });
        }
        else
        {
            Debug.LogWarning("덱에 더이상 카드가 존재하지 않습니다");
            return;
        }
    }


    private IEnumerator InstantiateCo()
    {
        for (int i =0; i< firstCount; i++)
        {
            InstantiateCardObject(i);
            yield return new WaitForSeconds(.05f);
        }
        isEndCardSpawn = true;
    }

    public void UsedCard(GameObject UsedCard)
    {
        CardHandler usedCardHandler = UsedCard.GetComponent<CardHandler>();
        cardsInHand.Remove(usedCardHandler);
        cardsUsed.Add(usedCardHandler);
    }
}
