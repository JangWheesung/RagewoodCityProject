using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHP : Living
{
    [SerializeField] private GameObject prfHpBar;
    [Header("HpStat")]
    private float maxHp;
    [SerializeField] private float heeling;
    [SerializeField] private float heelTime;

    private void Awake()
    {
        maxHp = hp;

        base.OnEnable();
        HpbarSetting();
        StartCoroutine(Heel());
    }

    protected override void HpbarSetting()
    {
        hpBar = Instantiate(prfHpBar, canvers.transform.Find("PlayerSlider")).GetComponent<RectTransform>();
        slider = hpBar.GetComponent<Slider>();

        slider.maxValue = hp;
        slider.value = hp;
    }

    void Update()
    {
        Die();
    }

    private void FixedUpdate()
    {
        Hpbar();
    }

    public void Heeling(int value)
    {
        if (hp + value > maxHp)
            hp = maxHp;
        else
            hp += value;
        slider.value = hp;
    }

    private void Die()
    {
        if (hp <= 0 && hp >= -999)
        {
            hp = -1999;
            GameoverManager.instance.Gameover();
            Time.timeScale = 0.1f;
        }
    }

    private IEnumerator Heel()
    {
        while (true)
        {
            yield return new WaitForSeconds(heelTime);

            if (hp < maxHp)
            {
                if (hp + heeling > 30f)
                    hp = maxHp;
                else
                    hp += heeling;

                slider.value = hp;
            }
        }
    }
}
