using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : Living
{
    [SerializeField] private GameObject prfHpBar;
    [SerializeField] private GameObject dieEmpact;

    private GameObject canvers;
    private Camera mainCam;

    private void OnEnable()
    {
        base.Awake();
    }

    void Update()
    {
        Die();
        Hpbar();
    }

    protected override void HpbarSetting()
    {
        canvers = GameObject.Find("Canvas");

        hpBar = Instantiate(prfHpBar, canvers.transform.Find("Hpbars")).GetComponent<RectTransform>();
        slider = hpBar.GetComponent<Slider>();

        slider.maxValue = hp;
        slider.value = hp;
    }

    void Die()
    {
        if (hp <= 0)
        {
            hp = 1;
            hpBar.gameObject.SetActive(false);
            PoolingManager.instance.Pop(dieEmpact.name, transform.position);
            PoolingManager.instance.Push(gameObject);
        }
    }
}
