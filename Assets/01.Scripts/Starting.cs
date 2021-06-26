using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour
{
    public GameObject cardUI;
    public GameObject EctUI;
    public CardManager cardM;
    public EnemyManager enemyM;

    public void StartGame()
    {
        cardUI.SetActive(true);
        EctUI.SetActive(true);
        cardM.gameObject.SetActive(true);
        cardM.StartGame();
        Test.isMyturn = true;
        enemyM.isfirstTrun = true;
        enemyM.StartEnemyManager();
        DamageManager.Instance.ResetGame();
        gameObject.SetActive(false);
    }
}
