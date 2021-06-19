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
    public Text Hptext;
    public GameObject Hpbar;

    public float totalDamage;
    public float nowDamage;
    public float hp;

    public void Start()
    {
        hp = 500;
        Hptext.text = hp.ToString();
    }


    public void SetDamage(float damage,bool isNomal)
    {
        if (isNomal)
        {
            nowDamage = damage;
            totalDamage += damage;
            nowDamageText.text = "현재:" + damage;
            totalDamageText.text = "누적:" + totalDamage;
        }
        else
        {
            HitHp();
            totalDamage = 0;
            nowDamage = damage;
            totalDamage += damage;
            nowDamageText.text = "현재:" + damage;
            totalDamageText.text = "누적:" + totalDamage;
        }
    }

    public void HitHp()
    {
        hp -= totalDamage;
        Hpbar.GetComponent<Slider>().value = hp / 500;
        Hptext.text = hp.ToString();
    }
}
