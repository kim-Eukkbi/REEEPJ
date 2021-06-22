using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class clouldMove : MonoBehaviour
{
    private int lr = -1;

    private void Start()
    {
        Sequence Main = DOTween.Sequence();
        StartCoroutine(Move(Main));
    }

    private IEnumerator Move(Sequence sequence)
    {
        while(true)
        {
            sequence.Append(transform.DOMove(new Vector3(transform.position.x + lr
                , transform.position.y, 0), 5f).SetEase(Ease.Linear));
            yield return new WaitForSeconds(5f);
            lr = -lr;
        }
    }
}
