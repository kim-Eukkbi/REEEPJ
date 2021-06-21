using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCon : MonoBehaviour
{
    public Transform card;
    public bool isCardMovepos = false;


    void Update()
    {
        if (transform.GetChild(0) == null)
        {
            return;
        }
        else if(this.transform.GetChild(0) != null && !isCardMovepos)
        {
            card = this.transform.GetChild(0);
            card.gameObject.transform.position += Vector3.back;
            isCardMovepos = true;
        }
    }
}
