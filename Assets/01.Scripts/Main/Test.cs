using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public DropArea playerArea;
    public DropArea fieldArea;

    public RectTransform playerRectParent;
    public RectTransform fieldRectParent;
    public RectTransform hoverRectParent;

    [SerializeField]
    public static bool isMyturn = true;


    public CardManager cardManager;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        playerArea.onLifted += ObjectLiftedFromPlayer;
        playerArea.onDropped += ObjectDroppedPlayer;

        fieldArea.onLifted += ObjectLiftedFromfield;
        fieldArea.onDropped += ObjectDroppedfield;
    }



    private void ObjectLiftedFromPlayer(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(hoverRectParent, true);
    }

    private void ObjectDroppedPlayer(DropArea area, GameObject gameObject)
    {
        cardManager.OrderCard(true);
        gameObject.transform.SetParent(playerRectParent, true);
    }

    private void ObjectLiftedFromfield(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(hoverRectParent, true);
    }

    private void ObjectDroppedfield(DropArea area, GameObject gameObject)
    {
        if(isMyturn)
        {
            int index;
            cardManager.UsedCard(gameObject);
            gameObject.transform.SetParent(fieldRectParent, true);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.Translate(new Vector3(.01f, .01f, -0.1f) * cardManager.cardsUsed.Count);
            gameObject.GetComponent<CardHandler>().UseCard();
            gameObject.GetComponent<Image>().raycastTarget = false;
            index = cardManager.CheckCard(gameObject);
            GameObject obj = cardManager.ponCardList[index];
            cardManager.ponCardList.RemoveAt(index);
            Destroy(obj);
            StartCoroutine(OrderCardCo());
        }
        isMyturn = !isMyturn;
        DamageManager.Instance.remainTime = 30;
        DamageManager.Instance.OnNextTrun.Invoke();
    }

    private IEnumerator OrderCardCo()
    {
        yield return null;
        cardManager.OrderCard(true);
    }

    /*private void SetDropArea(bool active)
    {
        playerArea.gameObject.SetActive(active);
    }*/

}
