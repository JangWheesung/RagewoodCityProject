using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public abstract class Living : MonoBehaviour
{
    public float hp;

    private Camera mainCam;
    protected GameObject canvers;
    private SpriteRenderer spriteRenderer;
    protected RectTransform hpBar;
    protected Slider slider;

    Color stateColor = Color.white;
    const float height = 1f;

    protected virtual void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;

        mainCam = Camera.main;
        canvers = GameObject.Find("Canvas");
        HpbarSetting();
    }

    protected abstract void HpbarSetting();

    protected void Hpbar()
    {
        Vector3 hpBarVec = new Vector3(transform.position.x, transform.position.y + height, 0);
        Vector3 hpBarPos = mainCam.WorldToScreenPoint(hpBarVec);
        hpBar.position = hpBarPos;
    }

    public void OnDamage(float dmg)
    {
        hp -= dmg;
        slider.value -= dmg;

        if(gameObject.activeSelf)
            StartCoroutine(DamageColor(0.1f));
    }

    public void OnDamage(float dmg, float nuckbackLength, Vector3 nuckbackDir)
    {
        hp -= dmg;
        slider.value -= dmg;
        transform.position += nuckbackDir * nuckbackLength;

        StartCoroutine(DamageColor(0.1f));
    }

    public void OnFireDamage(float dmg, float time, GameObject particle)
        => StartCoroutine(OnFireBomb(dmg, time, particle));

    public void OnIceDamage(float time, GameObject particle)
        => StartCoroutine(OnIceBomb(time, particle));

    public void OnGasDamage(float time)
        => StartCoroutine(OnGasBomb(time));

    private IEnumerator OnFireBomb(float dmg, float time, GameObject particle)
    {
        PoolingManager.instance.Pop(particle.name, transform.position, transform);

        Color orange = new Color(1, 0.4f, 0, 1);
        stateColor = orange;
        for (int i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(1);
            OnDamage(dmg);
        }

        if(stateColor == orange)
            stateColor = Color.white;
    }

    private IEnumerator OnIceBomb(float time, GameObject particle)
    {
        PoolingManager.instance.Pop(particle.name, transform.position, transform);

        stateColor = Color.cyan;

        float slowSpeed = GetComponent<EnemyFSM>().moveSpeed / 2;
        GetComponent<EnemyFSM>().moveSpeed = slowSpeed;

        yield return new WaitForSeconds(time);

        GetComponent<EnemyFSM>().moveSpeed = slowSpeed * 2;

        if (stateColor == Color.cyan)
        {
            stateColor = Color.white;
            spriteRenderer.color = stateColor;
        }
    }

    private IEnumerator OnGasBomb(float time)
    {
        Color purple = new Color(0.8f, 0, 1, 1);
        stateColor = purple;
        GetComponent<EnemyFSM>().isFaint = true;

        yield return new WaitForSeconds(time);

        if (stateColor == purple)
        {
            stateColor = Color.white;
            spriteRenderer.color = stateColor;
        }
    }

    IEnumerator DamageColor(float time)
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(time);
            spriteRenderer.color = stateColor;
            yield return new WaitForSeconds(time);
        }
    }
}
