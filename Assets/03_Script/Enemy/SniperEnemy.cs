using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : EnemyFSM
{
    protected override IEnumerator AttackDelay()
    {
        fireSound.Play();
        while (state == State.Attack)
        {
            playerHP.OnDamage(attackPower);

            lineRenderer.enabled = true;
            yield return new WaitForSeconds(0.02f);
            lineRenderer.enabled = false;
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
