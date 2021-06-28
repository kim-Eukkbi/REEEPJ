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
    public Text turnIndecator;
    public Text Win;
    public GameObject healOrDealObj;
    public GameObject Hpbar;
    public GameObject enemyHpbar;
    public GameObject gameOverObj;
    public GameObject hitEffect;
    public GameObject healEffect;
    public GameObject turnIndecatorObj;
    public Slider timerBar;


    public float totalDamage;
    public float nowDamage;
    public float hp;
    public float enemyHp;
    public float maxhp;
    public float enemyMaxhp;
    public float remainTime;

    public void Start()
    {
        ResetGame();
    }

    public void Update()
    {
        remainTime -= Time.deltaTime;
        time.text = string.Format("Time : {0:0.00}",remainTime);
        timerBar.value = remainTime / 30;
        if (remainTime <= 0)
        {
            DamagePlayer(0, false);
            ResetTurn();
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
            DrawHpBar(false, heal);
        }
        else
        {
            enemyHp += heal;
            if (enemyHp > enemyMaxhp)
                enemyMaxhp = enemyHp;
            DrawHpBar(false, heal);
        }
    }

    public void HitHp()
    {
        if(Test.isMyturn)
        {
            hp -= totalDamage;
            DrawHpBar(true, totalDamage);
            if (hp < 0)
            {
                StartCoroutine(GameOver(true));
            }
        }
        else
        {
            enemyHp -= totalDamage;
            DrawHpBar(true, totalDamage);
            if (enemyHp < 0)
            {
                StartCoroutine(GameOver(false));
            }
        }
    }

    public void DrawHpBar(bool isDamage,float index)
    {
        if(Test.isMyturn)
        {
            HealOrDamageEff(Hpbar, isDamage, index);
            Hpbar.GetComponent<Slider>().DOValue(hp / maxhp, 2f).SetEase(Ease.OutQuart);
            hptext.DOText(hp + "/" + maxhp, .5f).SetEase(Ease.OutQuart);
        }
        else
        {
            HealOrDamageEff(enemyHpbar, isDamage, index);
            enemyHpbar.GetComponent<Slider>().DOValue(enemyHp / enemyMaxhp, 2f).SetEase(Ease.OutQuart);
            enemyHptext.DOText(enemyHp + "/" + enemyMaxhp, .5f).SetEase(Ease.OutQuart);
        }

        Sequence HitSeq = DOTween.Sequence();
        if(isDamage)
        {
            HitSeq.Append(Camera.main.DOShakePosition(.5f, .5f, 20, 90, false));
            HitSeq.Join(hitEffect.GetComponent<Image>().DOFade(.5f, .25f));
            HitSeq.Append(hitEffect.GetComponent<Image>().DOFade(0, .25f));
        }
        else
        {
            HitSeq.Append(healEffect.GetComponent<Image>().DOFade(.5f, .25f));
            HitSeq.Append(healEffect.GetComponent<Image>().DOFade(0, .25f));
        }

    }

    public void Damazing(float damage)
    {
        nowDamage = damage;
        totalDamage += damage;
        nowDamageText.DOText("현재:" + damage,.5f);
        totalDamageText.DOText("누적:" + totalDamage,.5f);
    }

    public IEnumerator GameOver(bool isplayerDead)
    {
        yield return new WaitForSeconds(.5f);
        if(isplayerDead)
        {
            gameOverObj.SetActive(true);
            Win.text = "LOSE";
        }
        else
        {
            gameOverObj.SetActive(true);
            Win.text = "WIN";
        }
    }

    public void ResetTurn()
    {
        //timerBar.DOValue(1, .5f);
        remainTime = 30;
        Test.isMyturn = !Test.isMyturn;
        if (Test.isMyturn)
        {
            turnIndecatorObj.transform.DORotateQuaternion(Quaternion.Euler(360, 0, 0), 1f);
            turnIndecator.DOText("MyTurn", 1f);
        }
        else
        {
            turnIndecatorObj.transform.transform.DORotateQuaternion(Quaternion.Euler(180, 0, 0), 1f);
            turnIndecator.DOText("AiTurn", 1f);
        }
        OnNextTrun.Invoke();
    }

    public void ResetGame()
    {
        remainTime = 30;
        //timerBar.DOValue(1, .5f);
        Test.isMyturn = true;
        hp = 400;
        enemyHp = 400;
        maxhp = hp;
        enemyMaxhp = enemyHp;
        Hpbar.GetComponent<Slider>().value = hp / maxhp;
        hptext.text = hp.ToString() + "/" + maxhp.ToString();
        enemyHpbar.GetComponent<Slider>().value = enemyHp / enemyMaxhp;
        enemyHptext.text = enemyHp.ToString() + "/" + enemyMaxhp.ToString();
        totalDamage = 0;
        Damazing(0);
    }

    public void TurnEnd()
    {
        print("턴엔드");
        ResetTurn();
    }

    public void HealOrDamageEff(GameObject gameObject,bool isDamage,float index)
    {
        Sequence EffectSeq = DOTween.Sequence();
        GameObject effect;
        Text effText;
        effect = Instantiate(healOrDealObj, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        effText = effect.GetComponent<Text>();
        effText.DOFade(1, .01f);
        if (isDamage)
        {
            effText.text = "-" + index.ToString();
            effText.color = Color.red;
        }
        else
        {
            effText.text = "+" + index.ToString();
            effText.color = Color.green;
        }
        EffectSeq.Append(effect.transform.DOMove(gameObject.transform.position, 2f));
        EffectSeq.Insert(.5f,effect.GetComponent<Text>().DOFade(0, 1f)).OnComplete(()=> Destroy(effect));
    }
}
