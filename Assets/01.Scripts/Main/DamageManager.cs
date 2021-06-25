using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

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

    public UnityEvent OnNextTrun;

    public Text nowDamageText;
    public Text totalDamageText;
    public Text hptext;
    public Text enemyHptext;
    public Text time;
    public GameObject Hpbar;
    public GameObject enemyHpbar;
    public GameObject gameOverObj;

    public float totalDamage;
    public float nowDamage;
    public float hp;
    public float enemyHp;
    public float maxhp;
    public float enemyMaxhp;
    public float remainTime;

    public void Start()
    {
        remainTime = 30;
        hp = 250;
        enemyHp = 250;
        maxhp = hp;
        enemyMaxhp = enemyHp;
        Hpbar.GetComponent<Slider>().value = hp / maxhp;
        hptext.text = hp.ToString() + "/" + maxhp.ToString();
        enemyHpbar.GetComponent<Slider>().value = enemyHp / enemyMaxhp;
        enemyHptext.text = enemyHp.ToString() + "/" + enemyMaxhp.ToString();
    }

    public void Update()
    {
        remainTime -= Time.deltaTime;
        time.text = string.Format("Time : {0:0.00}",remainTime);
        if(remainTime <= 0)
        {
            remainTime = 30;
            Test.isMyturn = !Test.isMyturn;
            OnNextTrun.Invoke();
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
        if(Test.isMyturn)
        {
            hp += heal;
            if (hp > maxhp)
                maxhp = hp;
            DrawHpBar();
        }
        else
        {
            enemyHp += heal;
            if (enemyHp > enemyMaxhp)
                enemyMaxhp = enemyHp;
            DrawHpBar();
        }
    }

    public void HitHp()
    {
        if(Test.isMyturn)
        {
            hp -= totalDamage;
            DrawHpBar();
            if (hp < 0)
            {
                StartCoroutine(GameOver());
            }
        }
        else
        {
            enemyHp -= totalDamage;
            DrawHpBar();
            if (enemyHp < 0)
            {
                StartCoroutine(GameOver());
            }
        }
    }

    public void DrawHpBar()
    {
        if(Test.isMyturn)
        {
            Hpbar.GetComponent<Slider>().DOValue(hp / maxhp, 2f).SetEase(Ease.OutQuart);
            hptext.DOText(hp + "/" + maxhp, .5f).SetEase(Ease.OutQuart);
        }
        else
        {
            enemyHpbar.GetComponent<Slider>().DOValue(enemyHp / enemyMaxhp, 2f).SetEase(Ease.OutQuart);
            enemyHptext.DOText(enemyHp + "/" + enemyMaxhp, .5f).SetEase(Ease.OutQuart);
        }
    }

    public void Damazing(float damage)
    {
        nowDamage = damage;
        totalDamage += damage;
        nowDamageText.DOText("현재:" + damage,.5f);
        totalDamageText.DOText("누적:" + totalDamage,.5f);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        gameOverObj.SetActive(true);
    }

    public void ResetTurn()
    {
        remainTime = 30;
        Test.isMyturn = !Test.isMyturn;
        OnNextTrun.Invoke();
    }
}
