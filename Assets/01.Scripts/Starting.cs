using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour
{
    public GameObject cardUI;
    public GameObject EctUI;
    public CardManager cardM;

    public void StartGame()
    {
        cardUI.SetActive(true);
        EctUI.SetActive(true);
        cardM.gameObject.SetActive(true);
        cardM.StartGame();
        gameObject.SetActive(false);
    }
}
