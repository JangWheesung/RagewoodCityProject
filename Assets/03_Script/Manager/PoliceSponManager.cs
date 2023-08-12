using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PoliceSponManager : MonoBehaviour
{
    public static PoliceSponManager instance;

    [Header("Obj")]
    [SerializeField] private Transform[] sponTrs;
    [SerializeField] private GameObject police;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Sprite[] enemySprites;

    [Header("Level")]
    [SerializeField] private int maxLevel;
    [SerializeField] private int victimAmount;
    [HideInInspector] public int diePolices;

    [Header("Value")]
    [SerializeField] private float[] attackValue;
    [SerializeField] private float[] hpValue;
    [SerializeField] private float[] sponTime;

    [HideInInspector] public bool wait;
    private int score;

    private void Awake()
    {
        instance = this;
        StartCoroutine(Sponing());
    }

    private void SponPolice()
    {
        Transform sponTrs = this.sponTrs[Random.Range(0, this.sponTrs.Length)];
        GameObject newPolice = PoolingManager.instance.Pop(police.name, sponTrs.position);
        newPolice.GetComponent<SpriteRenderer>().sprite = enemySprites[WantedLevel()];
        newPolice.GetComponent<EnemyFSM>().attackPower = attackValue[WantedLevel()];
        newPolice.GetComponent<EnemyHP>().hp = hpValue[WantedLevel()];
    }

    private int WantedLevel()
    {
        return Mathf.Clamp(diePolices / victimAmount, 0, maxLevel);
    }

    public void PlusScore()
    {
        score += (WantedLevel() + 1) * 10;
        scoreText.text = $"score : {score}";

        if (score > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }

    private IEnumerator Sponing()
    {
        while (true)
        {
            yield return new WaitForSeconds(sponTime[WantedLevel()]);
            if(!wait)
                SponPolice();
        }
    }
}
