using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleAnime : MonoBehaviour
{
    public GameObject logo;
    public GameObject bgCloud;
    public GameObject reCreate;
    public GameObject flash;
    public GameObject clickToStart;



    private void Start()
    {
        Image _flash = flash.GetComponent<Image>();
        Vector3 logoPos = logo.transform.position;
        Vector3 reCreatePos = reCreate.transform.position;
        logo.transform.position -= new Vector3(0, 100, 0);
        reCreate.transform.position -= new Vector3(0, 100, 0);
        Sequence mainSeq = DOTween.Sequence();
        Sequence cloudSeq = DOTween.Sequence();
        //cloudSeq.SetDelay(2.5f);
        mainSeq.Append(logo.transform.DOMove(logoPos, 1.5f).SetEase(Ease.OutQuart));
        mainSeq.Insert(1f, reCreate.transform.DOMove(reCreatePos, 1f).SetEase(Ease.OutQuart));
        mainSeq.Append(_flash.DOFade(1, 0.5f).SetEase(Ease.InOutElastic));
        mainSeq.Append(_flash.DOFade(0, 1f));

        AppendMove(-1, cloudSeq);
        AppendMove(1, cloudSeq);
        AppendMove(-1, cloudSeq);
    }

    public void AppendMove(int index, Sequence sequence)
    {
        sequence.Append(bgCloud.transform.DOMove(
        new Vector3(bgCloud.transform.position.x + 1 * index,
        bgCloud.transform.position.y, 0), 7f).SetEase(Ease.Linear));
    }
}
