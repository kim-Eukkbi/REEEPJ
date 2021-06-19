using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public Text nowDamageText;
    public Text totalDamageText;

    private float totalDamage;

    public void SetDamage(float damage)
    {
        totalDamage += damage;
        nowDamageText.text = "현재:" + damage;
        totalDamageText.text = "누적:" + totalDamage;
    }
}
