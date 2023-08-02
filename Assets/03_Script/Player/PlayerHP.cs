using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHP : Living
{
    [SerializeField] private GameObject prfHpBar;
    [Header("HpStat")]
    [SerializeField] private float maxHp;
    [SerializeField] private float heeling;
    [SerializeField] private float heelTime;

    private GameObject canvers;
    private Camera mainCam;

    protected override void Awake()
    {
        base.Awake();
        maxHp = hp;

        StartCoroutine(Heel());
    }

    protected override void HpbarSetting()
    {
        canvers = GameObject.Find("Canvas");

        hpBar = Instantiate(prfHpBar, canvers.transform).GetComponent<RectTransform>();
        slider = hpBar.GetComponent<Slider>();

        slider.maxValue = hp;
        slider.value = hp;
    }

    void Update()
    {
        Die();
        Hpbar();
    }

    private IEnumerator Heel()
    {
        while (true)
        {
            yield return new WaitForSeconds(heelTime);

            if (hp < maxHp)
            {
                hp += heeling;
                slider.value = hp;
            }
        }
    }

    void Die()
    {

    }
}
