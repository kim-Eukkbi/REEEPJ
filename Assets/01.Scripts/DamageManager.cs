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
    public Text hptext;
    public Text time;
    public GameObject Hpbar;
    public GameObject gameOverObj;

    public float totalDamage;
    public float nowDamage;
    public float hp;
    public float maxhp;
    public float remainTime;

    public void Start()
    {
        remainTime = 50;
        hp = 500;
        maxhp = hp;
        hptext.text = hp.ToString();
    }

    public void Update()
    {
        remainTime -= Time.deltaTime;
        time.text = string.Format("Time : {0:0.00}",remainTime);
        if(remainTime <= 0)
        {
            remainTime = 50;
            Test.isMyturn = !Test.isMyturn;
        }
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
        if(hp <0)
        {
            GameOver();
        }
    }

    public void DrawHpBar()
    {
        Hpbar.GetComponent<Slider>().value = hp / maxhp;
        hptext.text = hp.ToString();
    }

    public void Damazing(float damage)
    {
        nowDamage = damage;
        totalDamage += damage;
        nowDamageText.text = "현재:" + damage;
        totalDamageText.text = "누적:" + totalDamage;
    }

    public void GameOver()
    {
        gameOverObj.SetActive(true);
    }
}
