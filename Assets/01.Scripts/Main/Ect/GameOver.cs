using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject cardUI;
    public GameObject ectUI;
    public GameObject startUI;
    public GameObject gameOverUI;
    public GameObject gameOverImage;
    public GameObject ClickToTitleText;
    public GameObject ReturnTitle;
    public GameObject WIN;
    public CardManager CardManager;

    private Vector3 ReturnPos;
    private Vector3 WinPos;
    private void OnEnable()
    {
        gameOverUI.SetActive(true);
        WinPos = WIN.transform.position;
        ReturnPos = ClickToTitleText.transform.position;

        ClickToTitleText.transform.position -= new Vector3(0, 10, 0);
        WIN.transform.position -= new Vector3(0, 10, 0);

        Sequence MainSeq = DOTween.Sequence();
        cardUI.SetActive(false);
        ectUI.SetActive(false);
        MainSeq.Append(Camera.main.transform.DOShakePosition(1.5f,.5f,20,90,false,false));
        MainSeq.Append(gameOverImage.GetComponent<Image>().DOFade(1,2f));
        MainSeq.Append(WIN.transform.DOMove(WinPos, .5f));
        MainSeq.Join(ClickToTitleText.transform.DOMove(ReturnPos, .5f));
    }

    public void ClickToTitle()
    {
        print("·Îµå");
        startUI.SetActive(true);
        gameOverUI.SetActive(false);
        CardManager.ResetGame();
        DamageManager.Instance.ResetGame();
    }
}
