using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public CardManager cardManager;

    bool isfirstTrun = true;

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
            }
            else
            {
                cardManager.Draw();
            }

            StartCoroutine(Thinking());
        }
    }


    public IEnumerator Thinking()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < cardManager.cardsInEnemyHand.Count; i++)
        {
            if (cardManager.cardsInEnemyHand[i].card.damage > DamageManager.Instance.nowDamage)
            {
                cardManager.UsedCard(cardManager.cardsInEnemyHand[i].gameObject);
                DamageManager.Instance.ResetTurn();
            }
            else if (cardManager.cardsInEnemyHand[i].card.heal > DamageManager.Instance.nowDamage)
            {
                cardManager.UsedCard(cardManager.cardsInEnemyHand[i].gameObject);
                DamageManager.Instance.ResetTurn();
            }
            else
            {
                cardManager.UsedCard(cardManager.cardsInEnemyHand
                    .OrderByDescending(x => x.card.damage).First().gameObject);
                DamageManager.Instance.ResetTurn();
            }
        }
    }
}
