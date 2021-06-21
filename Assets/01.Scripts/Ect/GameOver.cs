using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOver : MonoBehaviour
{
    public GameObject cardUI;
    public GameObject ectUI;
    public GameObject gameOverImage;
    private void Start()
    {
        Sequence MainSeq = DOTween.Sequence();
        cardUI.SetActive(false);
        ectUI.SetActive(false);
        MainSeq.Append(Camera.main.transform.DOShakePosition(1.5f,.5f,20,90,false,false));
        MainSeq.Append(gameOverImage.GetComponent<Image>().DOFade(1,2f));
    }
}
