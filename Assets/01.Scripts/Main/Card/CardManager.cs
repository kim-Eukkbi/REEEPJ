using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class CardManager : MonoBehaviourPun
{
    public enum ServerState
    {
        Master,
        Other
    };
    public ServerState serverState;
    public GameObject cardPrefab;
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

    private int firstCount =0;
    private bool isEndCardSpawn = false;


    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            serverState = ServerState.Master;
        else
            serverState = ServerState.Other;

        if (serverState.Equals(ServerState.Master))
        {
            playerDeck = initialDeck.Clone();
            firstCount = playerDeck.deck.Count;
            StartCoroutine(InstantiateCo());
        }
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
                    DrawSeq(drawCard);
                    firstCount--;
                }
                else
                {
                    Debug.LogWarning("������ �� �ִ� ī���� ���� �Ѿ����ϴ�");
                }
            }
            else
            {
                Debug.LogWarning("ī�� ������ ���� �Ϸ���� �ʾҽ��ϴ�");
            }
        }
    }

    private void DrawSeq(GameObject drawCard)
    {
        Sequence drawSeq = DOTween.Sequence();
        Vector3 drawCardPos = drawCard.transform.position;
        CardHandler drawCardHandler = drawCard.GetComponent<CardHandler>();
        cardsOnSpwan.Remove(drawCardHandler);
        cardsInHand.Add(drawCardHandler);
        drawSeq.Append(drawCard.transform.DOMove(new Vector3(drawCardPos.x - 3, drawCardPos.y, drawCardPos.z - 2), .5f));
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
        Sequence instSeq = DOTween.Sequence();
        if (card != null)
        {
            cardObject = PhotonNetwork.Instantiate(cardPrefab.name,
                cardInstantiatePos.position + new Vector3(10, 0, 0), Quaternion.Euler(0, 180, 0));
            cardObject.transform.SetParent(cardInstantiatePos);
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
            Debug.LogWarning("���� ���̻� ī�尡 �������� �ʽ��ϴ�");
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