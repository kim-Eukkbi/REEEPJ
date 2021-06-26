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
    public GameObject gameOverUI;
    public GameObject gameOverImage;
    public GameObject ClickToTitleText;
    public GameObject ReturnTitle;

    private Vector3 ReturnPos;
    private void Start()
    {
        gameOverUI.SetActive(true);
        ReturnPos = ClickToTitleText.transform.position;
        ClickToTitleText.transform.position -= new Vector3(0, 10, 0);
        Sequence MainSeq = DOTween.Sequence();
        cardUI.SetActive(false);
        ectUI.SetActive(false);
        MainSeq.Append(Camera.main.transform.DOShakePosition(1.5f,.5f,20,90,false,false));
        MainSeq.Append(gameOverImage.GetComponent<Image>().DOFade(1,2f));
        MainSeq.Append(ClickToTitleText.transform.DOMove(ReturnPos, 1f));
    }

    public void ClickToTitle()
    {
        print("·Îµå");
        SceneManager.LoadScene("Lobby");
        this.gameObject.SetActive(false);
    }
}
