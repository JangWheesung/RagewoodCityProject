using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Living : MonoBehaviour
{
    public float hp;

    private Camera mainCam;
    protected GameObject canvers;
    private SpriteRenderer spriteRenderer;
    protected RectTransform hpBar;
    protected Slider slider;

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

        StartCoroutine(DamageColor(0.1f));
    }

    public void OnDamage(float dmg, float nuckbackLength, Vector3 nuckbackDir)
    {
        hp -= dmg;
        slider.value -= dmg;
        transform.position += nuckbackDir * nuckbackLength;

        StartCoroutine(DamageColor(0.1f));
    }

    public virtual IEnumerator OnFireBomb(float dmg, float time)
    {
        yield return new WaitForSeconds(time);
    }

    public virtual IEnumerator OnIceBomb(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public virtual IEnumerator OnGasBomb(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator DamageColor(float time)
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(time);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(time);
        }
    }
}
