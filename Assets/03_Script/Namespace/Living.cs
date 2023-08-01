using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour
{
    [SerializeField] private float hp;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDamage(float dmg)
    {
        hp -= dmg;

        StartCoroutine(DamageColor(0.1f));
    }

    public void OnDamage(float dmg, float nuckbackLength, Vector3 nuckbackDir)
    {
        hp -= dmg;
        transform.position += nuckbackDir * nuckbackLength;

        StartCoroutine(DamageColor(0.1f));
    }

    public IEnumerator OnFireBomb(float dmg, float time)
    {
        yield return new WaitForSeconds(time);
    }

    public IEnumerator OnIceBomb(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public IEnumerator OnGasBomb(float time)
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
