using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public CardManager cardManager;
    public GameObject fieldPos;
    public DropArea fieldDropArea;

    public bool isfirstTrun = true;

    public void Start()
    {
        DamageManager.Instance.OnNextTrun.AddListener(AI);
    }

    public void AI()
    {
        if (!Test.isMyturn)
        {
            if (isfirstTrun)
            {
                StartCoroutine(cardManager.FirstDraw());
                isfirstTrun = false;
                StartCoroutine(Thinking(5));
            }
            else
            {
                cardManager.Draw();
                StartCoroutine(Thinking(3));
            }

            
        }
    }




    public IEnumerator Thinking(float n)
    {
        yield return new WaitForSeconds(n);

        IEnumerable<CardHandler> CardDamage = cardManager.cardsInEnemyHand.OrderBy(x => x.card.damage);
        //IEnumerable<CardHandler> CardHeal = cardManager.cardsInEnemyHand.OrderBy(x => x.card.heal);

        for (int i = 0; i < cardManager.cardsInEnemyHand.Count; i++)
        {
            if (CardDamage.ToList()[i].card.damage > DamageManager.Instance.nowDamage)
            {
                AIUseCard(CardDamage.ToList()[i].gameObject);
                yield break;
            }
            else if (cardManager.cardsInEnemyHand[i].card.isHealCard == true)   
            {
                AIUseCard(cardManager.cardsInEnemyHand[i].gameObject);
                yield break;
            }
        }

        AIUseCard(CardDamage.Last().gameObject);
    }

    public void AIUseCard(GameObject gameObject)
    {
        Sequence UseSeq = DOTween.Sequence();
        UseSeq.Append(gameObject.transform.DOMove(this.transform.position, .5f));
        UseSeq.Insert(.1f, gameObject.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), .5f));
        UseSeq.Join(gameObject.transform.DOScale(1, .5f));
        UseSeq.Append(gameObject.transform.DOMove(cardManager.cardsUsed.Count == 0 ? 
            fieldPos.transform.position + new Vector3(0, 0, -30) :
            cardManager.cardsUsed[cardManager.cardsUsed.Count - 1].transform.position + new Vector3(0, 0, -30), .5f)).OnComplete(() =>
        {
            cardManager.UsedCard(gameObject);
            gameObject.transform.SetParent(fieldPos.transform, true);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.Translate(new Vector3(.01f, .01f, -0.1f) * cardManager.cardsUsed.Count);
            gameObject.GetComponent<CardHandler>().UseCard();
            gameObject.GetComponent<DropItem>().droppedArea = fieldDropArea;
            gameObject.GetComponent<Image>().raycastTarget = false;
            int index = cardManager.CheckCard(gameObject);
            GameObject obj = cardManager.enemyPonCardList[index];
            cardManager.enemyPonCardList.RemoveAt(index);
            Destroy(obj);
            StartCoroutine(OrderCo());
            DamageManager.Instance.ResetTurn();
        });

    }

    public IEnumerator OrderCo()
    {
        yield return null;
        cardManager.OrderCard(false);
    }
}
