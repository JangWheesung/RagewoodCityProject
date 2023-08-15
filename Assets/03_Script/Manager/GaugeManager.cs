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
    private PlayerHP playerHP;

    [Header("Icon")]
    [SerializeField] private TextMeshProUGUI bombLevelText;
    [SerializeField] private TextMeshProUGUI bombCntText;
    [SerializeField] private GameObject fireIcon;
    [SerializeField] private GameObject iceIcon;
    [SerializeField] private GameObject gasIcon;
    [Header("Roulette")]
    [SerializeField] private Image image;
    [SerializeField] private RectTransform roulettePanel;
    [SerializeField] private RectTransform rouletteImage;
    [SerializeField] private Button rouletteBtn;
    [SerializeField] private float rouletteTime;
    [SerializeField] private AudioSource rouletteSound;
    [Header("Value")]
    [SerializeField] private int[] maxGauge;
    [SerializeField] private float[] radiusValue;
    [SerializeField] private float[] dmgValue;
    [SerializeField] private int maxDrawcnt;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelText;

    private List<int> benList = new List<int>();
    public float spinSpeed;
    private int gaugeLevel = 0;
    private int bombLevel = 0;
    private bool isSpin;

    private void Awake()
    {
        instance = this;

        playerThrow = FindObjectOfType<PlayerThrow>();
        playerHP = FindObjectOfType<PlayerHP>();

        playerThrow.bombRadius = radiusValue[bombLevel];
        playerThrow.bombDmg = dmgValue[bombLevel];

        //불,얼음,가스 2번 나오는 거 방지
        benList.Add(1);
        benList.Add(3);
        benList.Add(5);
    }

    private void Update()
    {
        rouletteImage.Rotate(0, 0, spinSpeed);
    }

    public void GaugeUp()
    {
        image.fillAmount += (1f / maxGauge[gaugeLevel]);
        if (image.fillAmount >= 1f && gaugeLevel < maxGauge.Length - 1)
        {
            image.fillAmount = 0;
            gaugeLevel++;
            levelText.text = gaugeLevel == maxGauge.Length - 1 ? "Lv.max" : $"Lv.{gaugeLevel}";

            playerHP.Heeling(20);

            rouletteImage.rotation = Quaternion.identity;
            RouletteActive(true);
        }
    }

    private void RouletteActive(bool active)
    {
        float posY = active == true ? 550 : -550;
        PoliceSpawnManager.instance.wait = active;
        playerThrow.gameObject.SetActive(!active);
        if (active)
        {
            StartCoroutine(RouletteUp());
        }
        else
        {
            roulettePanel.DOMoveY(-550, 0.5f).SetEase(Ease.OutCirc);
        }
    }

    public void RouletteSpin()
    {
        if (isSpin) return;
        else isSpin = true;

        spinSpeed = 10f;
        rouletteSound.Play();

        StartCoroutine(StopSpin());
    }

    private void BuffUp()
    {
        playerThrow.throwCnt = BuffRange(0, 90) == true ? playerThrow.throwCnt + 1 : playerThrow.throwCnt;
        playerThrow.gas = BuffRange(90, 126) == true ? true : playerThrow.gas;
        playerThrow.ice = BuffRange(126, 162) == true ? true : playerThrow.ice;
        playerThrow.fire = BuffRange(162, 198) == true ? true : playerThrow.fire;

        if (BuffRange(198, 360))
        {
            bombLevel++;
            playerThrow.bombRadius = radiusValue[bombLevel];
            playerThrow.bombDmg = dmgValue[bombLevel];
        }

        StatIcon();
    }

    void StatIcon()
    {
        bombLevelText.text = $"Lv.{bombLevel}";
        bombCntText.text = $"x {playerThrow.throwCnt}";
        fireIcon.SetActive(playerThrow.fire);
        iceIcon.SetActive(playerThrow.ice);
        gasIcon.SetActive(playerThrow.gas);
    }

    bool BuffRange(float a, float b)
    {
        return rouletteImage.rotation.eulerAngles.z >= a && rouletteImage.rotation.eulerAngles.z < b;
    }

    float SetBuff()
    {
        int number = 0;
        bool re = true;
        while (re)
        {
            re = false;
            number = Random.Range(0, 20);
            foreach (int ben in benList)
            {
                if (number == ben)
                    re = true;
            }
        }
        benList.Add(number);
        return -95.6f + (number * 18f);
    }

    private IEnumerator RouletteUp()
    {
        float value = 21;
        for (int i = 0; i < 100; i++)
        {
            roulettePanel.transform.position += new Vector3(0, value, 0);
            value -= 0.21f;
            yield return new WaitForSeconds(0.01f);
        }
    }

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

        yield return new WaitForSeconds(0.5f);
        isSpin = false;
    }

    //0 ~ 90 : 개수 증가
    //90 ~ 126 : 가스
    //126 ~ 162 : 아이스
    //162 ~ 198 : 불
    //198 ~360 : 크기 업
}
