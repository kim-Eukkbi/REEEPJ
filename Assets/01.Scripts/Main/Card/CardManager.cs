using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject ponCardprefab;
    public GameObject enemyPonCardprefab;
    public Transform playerCardInstantiateRectPos;
    public Transform playercardObjectPos;
    public Transform enemyCardInstantiateRectPos;
    public Transform enemycardObjectPos;
    public Transform cardStackPos;
/*    public DropArea playerDropArea;
    public DropArea enemyDropArea;*/
    public GameObject DesObj;
    public GameObject NowDamage;
    public GameObject TotalDamage;

    public Deck initialDeck;
    private Deck playerDeck;

    public List<CardHandler> cardsOnSpwan;
    public List<CardHandler> cardsInHand;
    public List<CardHandler> cardsInEnemyHand;
    public List<CardHandler> cardsUsed;

    public List<GameObject> ponCardList = new List<GameObject>();
    public List<GameObject> enemyPonCardList = new List<GameObject>();

    private int firstCount = 0;
    private bool isEndCardSpawn = false;




    public void Start()
    {
        DamageManager.Instance.OnNextTrun.AddListener(() =>
        {
            if (Test.isMyturn)
                Draw();
        });
    }

    public void Draw()
    {
        if (cardsOnSpwan.Count < 2)
        {
            StartCoroutine(ReyCastReset(false));
            StartCoroutine(SuffleDeck(cardsUsed.Count));
            Invoke("Draw", 5f);
        }

        if (isEndCardSpawn)
        {

            if (Test.isMyturn)
            {
                StartCoroutine(ReyCastReset(false));
                if (cardsInHand.Count < 9)
                {
                    GameObject drawCard = cardsOnSpwan[firstCount - 1].gameObject;
                    ponCardList.Add(Instantiate(ponCardprefab, cardStackPos.position, Quaternion.identity, playerCardInstantiateRectPos));
                    DrawSeq(drawCard, true);
                    firstCount--;
                }
                else
                {
                    Debug.LogWarning("?????? ?? ???? ?????? ???? ??????????");
                }
            }
            else
            {
                StartCoroutine(ReyCastReset(false));
                if (cardsInEnemyHand.Count < 9)
                {
                    GameObject drawCard = cardsOnSpwan[firstCount - 1].gameObject;
                    enemyPonCardList.Add(Instantiate(enemyPonCardprefab, cardStackPos.position, Quaternion.identity, enemyCardInstantiateRectPos));
                    DrawSeq(drawCard, false);
                    firstCount--;
                }
            }

        }
        else
        {
            Debug.LogWarning("???? ?????? ???? ???????? ??????????");
        }
    }

    public IEnumerator OrderCard(bool isPlayerTurn,bool isNeedRayOn = false,float delay = 0)
    {
        if (isPlayerTurn)
        {
            if(Test.isMyturn)
            {
                StartCoroutine(ReyCastReset(false));
                for (int i = 0; i < ponCardList.Count; i++)
                {
                    if (isNeedRayOn)
                    {
                        cardsInHand[i].transform.DOMove(ponCardList[i].transform.position, .5f)
                     .OnComplete(() => StartCoroutine(ReyCastReset(true, delay)));
                    }
                    else
                        cardsInHand[i].transform.DOMove(ponCardList[i].transform.position, .5f);
                }
            }
            else
            {
                StartCoroutine(ReyCastReset(false));
                for (int i = 0; i < ponCardList.Count; i++)
                {
                    cardsInHand[i].transform.DOMove(ponCardList[i].transform.position, .5f);
                }
            }
        
        }
        else
        {
            for (int i = 0; i < enemyPonCardList.Count; i++)
            {
                cardsInEnemyHand[i].transform.DOMove(enemyPonCardList[i].transform.position, .5f);
            }
        }

        yield return null;
    }

    public int CheckCard(GameObject gameObject)
    {
        if(Test.isMyturn)
        {
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                if (gameObject.Equals(cardsInHand[i]))
                    return i;
            }
        }
        else
        {
            for (int i = 0; i < cardsInEnemyHand.Count; i++)
            {
                if (gameObject.Equals(cardsInEnemyHand[i]))
                    return i;
            }
        }
        return 0;
    }


    private void DrawSeq(GameObject drawCard, bool isPlayerTurn)
    {
        Sequence drawSeq = DOTween.Sequence();
        Vector3 drawCardPos = drawCard.transform.position;
        CardHandler drawCardHandler = drawCard.GetComponent<CardHandler>();
        cardsOnSpwan.Remove(drawCardHandler);
        if (isPlayerTurn)
        {
            cardsInHand.Add(drawCardHandler);
            drawSeq.Append(drawCard.transform.DOMove(new Vector3(drawCardPos.x - 3, drawCardPos.y, drawCardPos.z - 2), .5f));
            drawSeq.Insert(.1f, drawCard.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), .5f)).OnComplete(() =>
            {
                //drawCard.GetComponent<DropItem>().droppedArea = playerDropArea;
                drawCard.transform.SetParent(playercardObjectPos);
                StartCoroutine(OrderCard(true,true,0));
            });
        }
        else
        {
            cardsInEnemyHand.Add(drawCardHandler);
            drawSeq.Append(drawCard.transform.DOMove(new Vector3(drawCardPos.x - 5, drawCardPos.y, drawCardPos.z - 2), .5f));
            drawSeq.Insert(.1f, drawCard.transform.DOScale(.5f, .5f)).OnComplete(() =>
            {
                //drawCard.GetComponent<DropItem>().droppedArea = enemyDropArea;
                drawCard.transform.SetParent(enemycardObjectPos);
                StartCoroutine(OrderCard(false));
            });
        }

    }

    private void InstantiateCardObject(int index)
    {
        GameObject cardObject;
        Card card = playerDeck.Draw();
        Sequence instSeq = DOTween.Sequence();
        if (card != null)
        {
            cardObject = Instantiate(cardPrefab, cardStackPos.position + new Vector3(10, 0, 0), Quaternion.Euler(0, 180, 0), cardStackPos);
            instSeq.Append(cardObject.transform.DOMove(cardStackPos.position, .05f)).OnComplete(() =>
            {
                cardObject.transform.Translate(new Vector3(-.01f, .01f, .01f) * index);
                cardObject.GetComponent<CardHandler>().Initialize(card);
                cardObject.GetComponent<DropItem>().descriptionObj = DesObj;
                cardsOnSpwan.Add(cardObject.GetComponent<CardHandler>());
            });
        }
        else
        {
            Debug.LogWarning("???? ?????? ?????? ???????? ????????");
            return;
        }
    }


    private IEnumerator InstantiateCo()
    {
        for (int i = 0; i < firstCount; i++)
        {
            InstantiateCardObject(i);
            yield return new WaitForSeconds(.05f);
        }
        isEndCardSpawn = true;
    }

    public void UsedCard(GameObject UsedCard)
    {
        CardHandler usedCardHandler = UsedCard.GetComponent<CardHandler>();
        switch (usedCardHandler.card.power.cardName)
        {
            case "????":
                for(int i =0;i<2;i++)
                {
                    if(Test.isMyturn)
                    {
                        if(cardsInEnemyHand.Count <= 1)
                            break;

                        int x = UnityEngine.Random.Range(0, cardsInEnemyHand.Count);
                        GameObject rCard = cardsInEnemyHand[x].gameObject;
                        GameObject rPonCard = enemyPonCardList[x].gameObject;
                        cardsInEnemyHand.RemoveAt(x);
                        enemyPonCardList.RemoveAt(x);
                        Destroy(rCard);
                        Destroy(rPonCard);
                    }
                    else
                    {
                        if (cardsInHand.Count <= 1)
                            break;
                        int x = UnityEngine.Random.Range(0, cardsInHand.Count);
                        GameObject rCard = cardsInHand[x].gameObject;
                        GameObject rPonCard = ponCardList[x].gameObject;
                        cardsInHand.RemoveAt(x);
                        ponCardList.RemoveAt(x);
                        Destroy(rCard);
                        Destroy(rPonCard);
                    }
                }
                StartCoroutine(OrderCard(!Test.isMyturn));
                break;
            case "????????????????":
                int a = UnityEngine.Random.Range(0, 2);
                if (a.Equals(0))
                    usedCardHandler.card.damage = 0;
                else
                    usedCardHandler.card.damage = 100;
                break;
            case "????????":
                if(cardsOnSpwan.Count >= 3)
                {
                    Draw();
                    Draw();
                }
                break;
            case "??????":
                if (cardsOnSpwan.Count >= 3)
                {
                    Test.isMyturn = !Test.isMyturn;
                    Draw();
                    Test.isMyturn = !Test.isMyturn;
                    Draw();
                }
                break;
            case "?????? ????":
                DamageManager.Instance.totalDamage = 0;
                DamageManager.Instance.Damazing(0);
                break;
            case "????????":
                if (cardsOnSpwan.Count >= 3)
                {
                    Test.isMyturn = !Test.isMyturn;
                    Draw();
                    Draw();
                    Test.isMyturn = !Test.isMyturn;
                    Draw();
                    Draw();
                }
                break;
            default:
                break;
        }

        if (Test.isMyturn)
            cardsInHand.Remove(usedCardHandler);
        else
            cardsInEnemyHand.Remove(usedCardHandler);

        cardsUsed.Add(usedCardHandler);
    }

    public IEnumerator FirstDraw()
    {
        yield return new WaitForSeconds(firstCount * .05f + 1f);
        for (int i = 0; i < 5; i++)
        {
            Draw();
        }
    }

    public IEnumerator ReyCastReset(bool isMytrun,float Delay =0)
    {
        yield return new WaitForSeconds(Delay);
        foreach (var x in cardsInHand)
        {
            if(isMytrun)
                x.GetComponent<Image>().raycastTarget = true;
            else
                x.GetComponent<Image>().raycastTarget = false;
        }
    }

    public IEnumerator SuffleDeck(int index)
    {
        yield return new WaitForSeconds(.5f);
        for (int i =0;i< index; i++)
        {
            cardsUsed[i].transform.DOMove(cardsUsed[i].transform.position + new Vector3(-10, 0, 0), .15f);
            yield return new WaitForSeconds(.05f);
        }
        yield return null;

        foreach(var x in cardsUsed)
        {
            Destroy(x.gameObject);
        }
        cardsUsed.Clear();

        isEndCardSpawn = false;
        playerDeck = initialDeck.Clone();
        firstCount = playerDeck.deck.Count;
        StartCoroutine(InstantiateCo());

    }

    public void ResetGame()
    {
        foreach(var x in cardsOnSpwan)
            Destroy(x.gameObject);
        foreach (var x in cardsInHand)
            Destroy(x.gameObject);
        foreach (var x in cardsInEnemyHand)
            Destroy(x.gameObject);
        foreach (var x in cardsUsed)
            Destroy(x.gameObject);
        foreach (var x in ponCardList)
            Destroy(x.gameObject);
        foreach (var x in enemyPonCardList)
            Destroy(x.gameObject);
        cardsOnSpwan.Clear();
        cardsInHand.Clear();
        cardsInEnemyHand.Clear();
        cardsUsed.Clear();
        ponCardList.Clear();
        enemyPonCardList.Clear();
    }

    public void StartGame()
    {
        playerDeck = initialDeck.Clone();
        firstCount = playerDeck.deck.Count;
        StartCoroutine(InstantiateCo());
        StartCoroutine(FirstDraw());
    }

    public void GotoLastIndex(GameObject gameObject)
    {
        gameObject.transform.SetAsLastSibling();
    }
}
