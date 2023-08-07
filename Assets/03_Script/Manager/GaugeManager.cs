using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GaugeManager : MonoBehaviour
{
    public static GaugeManager instance;

    private PlayerThrow playerThrow;

    [Header("Roulette")]
    [SerializeField] private Image image;
    [SerializeField] private RectTransform roulettePanel;
    [SerializeField] private RectTransform rouletteImage;
    [SerializeField] private Button rouletteBtn;
    [SerializeField] private float rouletteTime;
    [Header("Value")]
    [SerializeField] private int[] maxGauge;
    [SerializeField] private float[] radiusValue;
    [SerializeField] private float[] dmgValue;
    [SerializeField] private int maxDrawcnt;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelText;

    public float spinSpeed;
    private int gaugeLevel = 0;
    private int bombLevel = 0;

    private void Awake()
    {
        instance = this;
        playerThrow = FindObjectOfType<PlayerThrow>();

        playerThrow.bombRadius = radiusValue[bombLevel];
        playerThrow.bombDmg = dmgValue[bombLevel];
    }

    private void Update()
    {
        rouletteImage.Rotate(0, 0, spinSpeed);
    }

    public void GaugeUp()
    {
        image.fillAmount += (1f / maxGauge[gaugeLevel]);
        if (image.fillAmount >= 1 && gaugeLevel < maxGauge.Length - 1)
        {
            image.fillAmount = 0;
            gaugeLevel++;
            levelText.text = gaugeLevel == maxGauge.Length - 1 ? "Lv.max" : $"Lv.{gaugeLevel}";

            rouletteImage.rotation = Quaternion.identity;
            rouletteBtn.onClick.AddListener(RouletteSpin);
            RouletteActive(true);
        }
    }

    private void RouletteActive(bool active)
    {
        float posY = active == true ? 500 : -550;
        PoliceSponManager.instance.wait = active;
        playerThrow.gameObject.SetActive(!active);
        roulettePanel.DOMoveY(posY, 0.5f).SetEase(Ease.OutCirc);
    }

    private void RouletteSpin()
    {
        rouletteBtn.onClick.RemoveListener(RouletteSpin);
        spinSpeed = 10f;

        StartCoroutine(StopSpin());
    }

    private void BuffUp()
    {
        playerThrow.throwCnt = BuffRange(0, 90) == true ? playerThrow.throwCnt + 1 : playerThrow.throwCnt;
        playerThrow.gas = BuffRange(90, 126) == true ? true : false;
        playerThrow.ice = BuffRange(126, 162) == true ? true : false;
        playerThrow.fire = BuffRange(162, 198) == true ? true : false;

        if (BuffRange(198, 360))
        {
            bombLevel++;
            playerThrow.bombRadius = radiusValue[bombLevel];
            playerThrow.bombDmg = dmgValue[bombLevel];
        }
    }

    bool BuffRange(float a, float b)
    {
        return rouletteImage.rotation.eulerAngles.z >= a && rouletteImage.rotation.eulerAngles.z < b;
    }

    float SetBuff()
    {
        int number = 0;
        List<int> benList = new List<int>();
        if (playerThrow.throwCnt >= maxDrawcnt)
        {
            benList.Add(0);
            benList.Add(1);
            benList.Add(2);
            benList.Add(3);
            benList.Add(4);
        }
        if (playerThrow.fire)
        {
            benList.Add(9);
            benList.Add(10);
        }
        if (playerThrow.ice)
        {
            benList.Add(7);
            benList.Add(8);
        }
        if (playerThrow.gas)
        {
            benList.Add(5);
            benList.Add(6); 
        }
        if (bombLevel >= radiusValue.Length)
        {
            benList.Add(11);
            benList.Add(12);
            benList.Add(13);
            benList.Add(14);
            benList.Add(15);
            benList.Add(16);
            benList.Add(17);
            benList.Add(18);
            benList.Add(19);
        }
        bool a = true;
        while (a)
        {
            a = false;
            number = Random.Range(0, 20);
            foreach (int ben in benList)
            {
                if (number == ben)
                    a = true;
            }
        }

        return -95.6f + (number * 18f);
    }
    //01234, 56, 78, 910, 11/12/13/14/15/16/17/18/19

    private IEnumerator StopSpin()
    {
        yield return new WaitForSeconds(rouletteTime);

        spinSpeed = 0;

        float firstValue = SetBuff();
        float rotValue = 40f;
        float lerpValue = 40f;
        float lerpAmount = 1.05f;
        for (int i = 0; i < 100; i++)
        {
            rouletteImage.rotation = Quaternion.Euler(0, 0, rotValue - firstValue);
            rotValue += (lerpValue / lerpAmount);
            lerpValue /= lerpAmount;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.5f);
        BuffUp();

        yield return new WaitForSeconds(1f);
        RouletteActive(false);
    }

    //0 ~ 90 : 개수 증가
    //90 ~ 126 : 가스
    //126 ~ 162 : 아이스
    //162 ~ 198 : 불
    //198 ~360 : 크기 업
}
