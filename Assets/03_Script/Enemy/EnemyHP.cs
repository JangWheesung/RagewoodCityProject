using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : Living
{
    [SerializeField] private GameObject prfHpBar;

    private GameObject canvers;
    private Camera mainCam;

    const float height = 1f;

    protected override void Awake()
    {
        base.Awake();

        mainCam = Camera.main;
        canvers = GameObject.Find("Canvas");
        hpBar = Instantiate(prfHpBar, canvers.transform).GetComponent<RectTransform>();
        slider = hpBar.GetComponent<Slider>();

        slider.maxValue = hp;
        slider.value = hp;
    }

    void Update()
    {
        Die();
        HpBar();
    }

    void HpBar()
    {
        Vector3 hpBarVec = new Vector3(transform.position.x, transform.position.y + height, 0);
        Vector3 hpBarPos = mainCam.WorldToScreenPoint(hpBarVec);
        hpBar.position = hpBarPos;
    }

    void Die()
    {
        if (hp <= 0)
        {
            Destroy(hpBar);
            Destroy(gameObject);
        }
    }
}
