using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PoliceSpawnManager : MonoBehaviour
{
    public static PoliceSpawnManager instance;

    [Header("Obj")]
    [SerializeField] private Transform[] sponTrs;
    [SerializeField] private Sprite[] enemySprites;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Police")]
    [SerializeField] private GameObject[] police;
    [SerializeField] private float[] attackMultiplier;
    [SerializeField] private float[] hpMultiplier;

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
        int random = Random.Range(0, 101);
        int policeValue = random < 76 ? 0 : random < 90 ? 1 : 2;
        PoliceSetting(police[policeValue], attackMultiplier[policeValue], hpMultiplier[policeValue]);

        //0 ~ 75 75(일반)
        //76 ~ 89 13(저격)
        //90 ~ 100 10(방패)
    }

    private void PoliceSetting(GameObject police, float attackMultiplier, float hpMultiplier)
    {
        Transform sponTrs = this.sponTrs[Random.Range(0, this.sponTrs.Length)];
        GameObject newPolice = PoolingManager.instance.Pop(police.name, sponTrs.position);
        newPolice.GetComponent<SpriteRenderer>().sprite = enemySprites[WantedLevel()];
        newPolice.GetComponent<EnemyFSM>().attackPower = attackValue[WantedLevel()] * attackMultiplier;
        newPolice.GetComponent<EnemyHP>().hp = hpValue[WantedLevel()] * hpMultiplier;
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
