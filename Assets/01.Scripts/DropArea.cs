using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private static List<DropArea> dropAreas;

    public delegate void ObjectLiftEvent(DropArea area, GameObject gameObject);
    public event ObjectLiftEvent onLifted;

    public delegate void ObjectDropEvent(DropArea area, GameObject gameObject);
    public event ObjectDropEvent onDropped;

    public delegate void ObjectHoverEnterEvent(DropArea area, GameObject gameObject);
    public event ObjectHoverEnterEvent onHoverEnter;

    public delegate void ObjectHoverExitEvent(DropArea area, GameObject gameObject);
    public event ObjectHoverExitEvent onHoverExit;

    public void Awake()
    {
        dropAreas = dropAreas ?? new List<DropArea>();
        dropAreas.Add(this);
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        onLifted += ObjectLiftedTest;
        onDropped += ObjectDroppedTest;
        onHoverEnter += ObjectHoverEnterTest;
        onHoverExit += ObjectHoverExitTest;
    }

    public void OnDisable()
    {
        onLifted -= ObjectLiftedTest;
        onDropped -= ObjectDroppedTest;
        onHoverEnter -= ObjectHoverEnterTest;
        onHoverExit -= ObjectHoverExitTest;
    }

    public void ObjectHoverExitTest(DropArea area, GameObject gameObject)
    {
        Debug.Log(this.gameObject.name + " Object Hover Exit: " + gameObject.name);
    }

    public void ObjectHoverEnterTest(DropArea area, GameObject gameObject)
    {
        Debug.Log(this.gameObject.name + "Object Hover Enter : " + gameObject.name);
    }

    public void ObjectDroppedTest(DropArea area, GameObject gameObject)
    {
        Debug.Log(this.gameObject.name + "Object Dropped : " + gameObject.name);
    }

    public void ObjectLiftedTest(DropArea area, GameObject gameObject)
    {
        Debug.Log(this.gameObject.name + "Object Lift : " + gameObject.name);

    }

    public void TriggerOnLift(DropItem item)
    {
        onLifted(this, item.gameObject);
    }

    public void TriggerOnDrop(DropItem item)
    {
        item.SetDroppedArea(this);
        onDropped(this, item.gameObject);
    }

    public void TriggerOnHoverEnter(GameObject gameObject)
    {
        onHoverEnter(this, gameObject);
    }

    public void TriggerOnHoverExit(GameObject gameObject)
    {
        onHoverExit(this, gameObject);
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        var gameObject = eventData.pointerDrag;
        if (gameObject == null)
            return;
        TriggerOnHoverExit(gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject gameObject = eventData.selectedObject;
        if (gameObject == null)
            return;

        var draggable = gameObject.GetComponent<DropItem>();
        if (draggable == null)
            return;

        Debug.Log("item Dropped : " + draggable.gameObject.name);
        TriggerOnDrop(draggable);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var gameObject = eventData.pointerDrag;
        if (gameObject == null)
            return;
        TriggerOnHoverEnter(gameObject);
    }

    public static void SetDropArea(bool enable)
    {
        foreach(var area in dropAreas)
        {
            area.gameObject.SetActive(enable);
        }
    }
}
