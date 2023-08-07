using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceSponManager : MonoBehaviour
{
    public static PoliceSponManager instance;

    [Header("Obj")]
    [SerializeField] private Transform[] sponTrs;
    [SerializeField] private GameObject police;

    [Header("Level")]
    [SerializeField] private int maxLevel;
    [SerializeField] private int victimAmount;
    [HideInInspector] public int diePolices;

    [Header("Value")]
    [SerializeField] private float[] attackValue;
    [SerializeField] private float[] hpValue;
    [SerializeField] private float[] sponTime;

    [HideInInspector] public bool wait;

    private void Awake()
    {
        instance = this;
        StartCoroutine(Sponing());
    }

    private void SponPolice()
    {
        Transform sponTrs = this.sponTrs[Random.Range(0, this.sponTrs.Length)];
        GameObject newPolice = PoolingManager.instance.Pop(police.name, sponTrs.position);
        newPolice.GetComponent<EnemyFSM>().attackPower = attackValue[wantedLevel()];
        newPolice.GetComponent<EnemyHP>().hp = hpValue[wantedLevel()];
    }

    private int wantedLevel()
    {
        return Mathf.Clamp(diePolices / victimAmount, 0, maxLevel);
    }

    private IEnumerator Sponing()
    {
        while (true)
        {
            yield return new WaitForSeconds(sponTime[wantedLevel()]);
            if(!wait)
                SponPolice();
        }
    }
}
