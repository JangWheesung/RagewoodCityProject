using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour
{
    public void OnHit()
    {

    }

    public void OnDamage(float dmg, Vector2 bombPos, Vector2 thisPos)
    {

    }

    public IEnumerator OnFireDamage(float dmg, float time)
    {
        yield return new WaitForSeconds(time);
    }

    public IEnumerator OnIceDamage(float dmg, float time)
    {
        yield return new WaitForSeconds(time);
    }
}
