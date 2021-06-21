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
    public float maxhp;

    public void Start()
    {
        hp = 500;
        maxhp = hp;
        Hptext.text = hp.ToString();
    }


    public void DamagePlayer(float damage,bool isNomal)
    {
        if (isNomal)
        {
            Damazing(damage);
        }
        else
        {
            HitHp();
            totalDamage = 0;
            Damazing(damage);
        }
    }

    public void HealPlayer(float heal)
    {
        hp += heal;
        if (hp > maxhp)
            maxhp = hp;
        DrawHpBar();
    }

    public void HitHp()
    {
        hp -= totalDamage;
        DrawHpBar();
    }

    public void DrawHpBar()
    {
        Hpbar.GetComponent<Slider>().value = hp / maxhp;
        Hptext.text = hp.ToString();
    }

    public void Damazing(float damage)
    {
        nowDamage = damage;
        totalDamage += damage;
        nowDamageText.text = "����:" + damage;
        totalDamageText.text = "����:" + totalDamage;
    }
}
