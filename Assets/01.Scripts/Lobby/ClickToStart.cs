using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickToStart : MonoBehaviour
{
    private int lr = 1;

    private void Start()
    {
        Sequence Main = DOTween.Sequence();
        StartCoroutine(Move(Main));
    }

    private IEnumerator Move(Sequence sequence)
    {
        while (true)
        {
            sequence.Append(gameObject.GetComponent<Text>().DOFade(lr, 2f));
            yield return new WaitForSeconds(2f);
            if (lr.Equals(1))
                lr = 0;
            else
                lr = 1;
        }
    }
}
