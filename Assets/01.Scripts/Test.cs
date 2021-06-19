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


    public List<GameObject> cardList;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        playerArea.onLifted += ObjectLiftedFromPlayer;
        playerArea.onDropped += ObjectDroppedPlayer;

        fieldArea.onLifted += ObjectLiftedFromfield;
        fieldArea.onDropped += ObjectDroppedfield;
        cardList = new List<GameObject>();
    }


    private void ObjectLiftedFromPlayer(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(hoverRectParent, true);
    }

    private void ObjectDroppedPlayer(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(playerRectParent, true);
    }

    private void ObjectLiftedFromfield(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(hoverRectParent, true);
    }

    private void ObjectDroppedfield(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(fieldRectParent, true);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.Translate(new Vector3(.03f, .03f, -.03f) * cardList.Count);
        cardList.Add(gameObject);
        gameObject.GetComponent<CardHandler>().UseCard();
    }

    private void SetDropArea(bool active)
    {
        playerArea.gameObject.SetActive(active);
    }
}
