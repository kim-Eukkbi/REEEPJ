using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ServerManager : MonoBehaviourPun
{
    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;
    public bool iIsActivetrue = false;
    public GameObject GameManager;
    public GameObject UICanvas;

    private void Awake()
    {
        GameManager.SetActive(false);
        UICanvas.SetActive(false);
    }

    private void Update()
    {
        if(!iIsActivetrue)
        {
            if (!IsMasterClientLocal || PhotonNetwork.PlayerList.Length < 2)
            {
                return;
            }
            else
            {
                GameManager.SetActive(true);
                UICanvas.SetActive(true);
                iIsActivetrue = true;
            }
        }
      
    }
}
