using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleAnime : MonoBehaviour
{
    public GameObject logo;
    public GameObject bgCloud;
    public GameObject cardGameText;
    public GameObject flash;
    public GameObject clickToStart;
    public GameObject infoText;
    public GameObject joinButton;
    public GameObject ClickPanel;
    public LobbyManager LobbyManager;

    private Sequence mainSeq;
    private Vector3 logoPos;
    private Vector3 cardGamePos;
    private Vector3 infoTextPos;
    private Vector3 joinButtonPos;

    private void Awake()
    {
        LobbyManager.gameObject.SetActive(false);
    }

    private void Start()
    {
        ClickPanel.SetActive(false);
        Image _flash = flash.GetComponent<Image>();


        logoPos = logo.transform.position;
        cardGamePos = cardGameText.transform.position;
        infoTextPos = infoText.transform.position;
        joinButtonPos = joinButton.transform.position;

        logo.transform.position -= new Vector3(0, 100, 0);
        cardGameText.transform.position -= new Vector3(0, 100, 0);
        infoText.transform.position -= new Vector3(0, 100, 0);
        joinButton.transform.position -= new Vector3(0, 100, 0);

        mainSeq = DOTween.Sequence();
        mainSeq.Append(logo.transform.DOMove(logoPos, 1.5f).SetEase(Ease.OutQuart));
        mainSeq.Insert(1f, cardGameText.transform.DOMove(cardGamePos, 1f).SetEase(Ease.OutQuart));
        mainSeq.Append(_flash.DOFade(1, 0.5f).SetEase(Ease.InOutElastic));
        mainSeq.Append(_flash.DOFade(0, 1f)).OnComplete(()=> 
        {
            bgCloud.AddComponent<clouldMove>();
            clickToStart.AddComponent<ClickToStart>();
            ClickPanel.SetActive(true);
        });
    }

    public void ClickScr()
    {
        Debug.Log("ÀÀ¾Ö");
        clickToStart.SetActive(false);
        Sequence clickSeq = DOTween.Sequence();
        clickSeq.Append(logo.transform.DOMove(new Vector3(logo.transform.position.x,
            logo.transform.position.y + 50,0), 5f).SetEase(Ease.OutQuart));
        clickSeq.Join(bgCloud.transform.DOMove(new Vector3(bgCloud.transform.position.x,
            bgCloud.transform.position.y +50,0), 5f).SetEase(Ease.OutQuart));
        clickSeq.Join(cardGameText.transform.DOMove(new Vector3(cardGameText.transform.position.x,
            cardGameText.transform.position.y + 50f,0), 5f).SetEase(Ease.OutQuart)).OnComplete(()=>
        {
            logo.SetActive(false);
            bgCloud.SetActive(false);
            cardGameText.SetActive(false);
        });

        clickSeq.Insert(.5f,infoText.transform.DOMove(infoTextPos, 1.5f).SetEase(Ease.OutQuart));
        clickSeq.Join(joinButton.transform.DOMove(joinButtonPos, 1.5f).SetEase(Ease.OutQuart)).OnComplete(()=>
        {
            LobbyManager.gameObject.SetActive(true);
            ClickPanel.SetActive(false);
            clickSeq.Kill();
            mainSeq.Kill();
        });
    }

}
