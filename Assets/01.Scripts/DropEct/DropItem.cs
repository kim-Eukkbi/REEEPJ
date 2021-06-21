using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItem : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
    ,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public delegate void DropItemMoveStartEvent(DropItem item);
    public event DropItemMoveStartEvent onMoveStart;

    public delegate void DropItemMoveEndEvent(DropItem item);
    public event DropItemMoveEndEvent onMoveEnd;

    public delegate void DropItemNoEvent(DropItem item);
    public event DropItemNoEvent onNothing;

    private RectTransform rectTransform;
    private RectTransform clampRectTransform;

    private Vector3 orignalWorldPos;
    private Vector3 orignalRectWorldPos;

    private Vector3 minWorldPos;
    private Vector3 maxWorldPos;

    public DropArea droppedArea;
    public DropArea prevDropArea;

    public GameObject descriptionObj;

    private Vector3 DescPos;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        clampRectTransform = rectTransform.root.GetComponent<RectTransform>();
    }

    public void SetDroppedArea(DropArea dropArea)
    {
        this.droppedArea = dropArea;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        orignalRectWorldPos = rectTransform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            clampRectTransform, eventData.position,
            eventData.pressEventCamera, out orignalWorldPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onMoveStart != null)
        {
            onMoveStart(this);
        }
        DropArea.SetDropArea(true);

        if(droppedArea != null)
        {
            droppedArea.TriggerOnLift(this);
        }

        prevDropArea = droppedArea;
        droppedArea = null;

        Rect clamp = new Rect(Vector2.zero, clampRectTransform.rect.size);
        Vector3  minPos = clamp.min - rectTransform.rect.min;
        Vector3  maxPos = clamp.max - rectTransform.rect.max;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            clampRectTransform, minPos,
            eventData.pressEventCamera, out minWorldPos);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            clampRectTransform, maxPos,
            eventData.pressEventCamera, out maxWorldPos);
        Debug.Log(minWorldPos + "/" + maxWorldPos);

        descriptionObj.GetComponentInChildren<Text>().text = gameObject.GetComponent<CardHandler>().cardDescription;
        descriptionObj.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPointerPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            clampRectTransform, eventData.position,
            eventData.pressEventCamera, out worldPointerPos))
        {
            Vector3 offsetToOriginal = worldPointerPos - orignalWorldPos;
            rectTransform.position = orignalRectWorldPos + offsetToOriginal;
        }

        RectTransformUtility.ScreenPointToWorldPointInRectangle(clampRectTransform, eventData.position
        , eventData.pressEventCamera, out DescPos);
        descriptionObj.transform.position = DescPos + new Vector3(2.3f, 1.3f, -3f);

        Vector3 worldPos = rectTransform.position;
        worldPos.x = Mathf.Clamp(rectTransform.position.x, minWorldPos.x, maxWorldPos.x);
        worldPos.y = Mathf.Clamp(rectTransform.position.y, minWorldPos.y, maxWorldPos.y);
        rectTransform.position = worldPos;


    }

    public void OnEndDrag(PointerEventData eventData)
    {

        DropArea.SetDropArea(false);
        if(onMoveEnd != null)
        {
            onMoveEnd(this);
        }

        bool noEvent = true;
        foreach(var go in eventData.hovered)
        {
            Debug.Log("on End Drag :" + go.name);
            var dropArea = go.GetComponent<DropArea>();
            if(dropArea != null)
            {
                noEvent = false;
                break;
            }
        }

        if(noEvent)
        {
            if (onNothing != null)
                onNothing(this);
        }

        descriptionObj.SetActive(false);
    }
}
