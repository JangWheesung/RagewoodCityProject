using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : Living
{
    [SerializeField] private GameObject prfHpBar;
    [SerializeField] private GameObject dieEmpact;

    void Update()
    {
        Die();
        Hpbar();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        HpbarSetting();
    }

    protected override void HpbarSetting()
    {
        hpBar = Instantiate(prfHpBar, canvers.transform.Find("Hpbars")).GetComponent<RectTransform>();
        slider = hpBar.GetComponent<Slider>();

        slider.maxValue = hp;
        slider.value = hp;
    }

    private void Die()
    {
        if (hp <= 0)
        {
            hp = 1;
            hpBar.gameObject.SetActive(false);
            PoliceSponManager.instance.diePolices++;
            PoliceSponManager.instance.PlusScore();
            GaugeManager.instance.GaugeUp();
            PoolingManager.instance.Pop(dieEmpact.name, transform.position);
            PoolingManager.instance.Push(gameObject);
        }
    }
}
