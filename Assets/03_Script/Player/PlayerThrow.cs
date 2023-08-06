using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThrow : PlayerRoot
{
    [Header("Grenade")]
    [SerializeField] private GameObject grenade;
    [SerializeField] private float throwSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float reloadTime;
    private bool canThrow = true;

    [Header("Bar")]
    [SerializeField] private GameObject prfBar;
    private GameObject canvers;
    private Camera mainCam;
    private RectTransform grenadeBar;
    private Slider slider;

    [Header("BuffEmpact")]
    [HideInInspector] public float bombRadius;
    [HideInInspector] public float bombDmg;
    public int throwCnt = 1;
    [HideInInspector] public bool fire;
    [HideInInspector] public bool ice;
    [HideInInspector] public bool gas;

    const float height = 1.8f;

    private void Start()
    {
        GrenadebarSetting();
    }

    private void Update()
    {
        ThrowControl();
    }

    private void FixedUpdate()
    {
        DirectionControl();
        Grenadebar();
    }

    private void GrenadebarSetting()
    {
        mainCam = Camera.main;
        canvers = GameObject.Find("Canvas");

        grenadeBar = Instantiate(prfBar, canvers.transform.Find("PlayerSlider")).GetComponent<RectTransform>();
        slider = grenadeBar.GetComponent<Slider>();

        grenadeBar.gameObject.SetActive(false);
    }

    private void Grenadebar()
    {
        Vector3 grenadeBarVec = new Vector3(transform.position.x, transform.position.y + height, 0);
        Vector3 grenadeBarPos = mainCam.WorldToScreenPoint(grenadeBarVec);
        grenadeBar.position = grenadeBarPos;
    }

    private void DirectionControl()
    {
        if (Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;

            Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector2 point = Camera.main.ScreenToWorldPoint(mousePos);
            DrawParabola(DirectionSetting(point));
        }
    }
    private void ThrowControl()
    {
        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;

            if (canThrow)
            {
                Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
                Vector2 point = Camera.main.ScreenToWorldPoint(mousePos);
                StartCoroutine(ThrowGrenade(DirectionSetting(point)));

                StartCoroutine(Reload());
            }
        }
    }

    private Vector2 DirectionSetting(Vector3 point)
    {
        return (point - transform.position).normalized;
    }

    private void DrawParabola(Vector2 dir)
    {
        lineRenderer.SetPosition(0, transform.position);

        float time = 0;
        float timeStep = 0.02f; // 시뮬레이션 간격
        Vector2 currentPosition = transform.position;
        Vector2 currentVelocity = dir * throwSpeed - new Vector2(Input.GetAxis("Horizontal") * 19, 0);
        //Vector2 currentVelocityX = Input.GetAxis("Horizontal");

        int positionCount = 1;
        while (true)
        {
            Vector2 gravityForce = Vector2.down * gravity;
            currentVelocity += gravityForce * timeStep;
            currentPosition += currentVelocity * timeStep;
            time += timeStep;

            // 궤적 그리기
            lineRenderer.positionCount = ++positionCount;
            lineRenderer.SetPosition(positionCount - 1, currentPosition);

            // 땅에 닿았을 때까지 반복
            if (currentPosition.y <= -15)
            {
                break;
            }
        }
    }

    private IEnumerator ThrowGrenade(Vector2 dir)
    {
        for (int i = 0; i < throwCnt; i++)
        {
            GameObject newGrenade = PoolingManager.instance.Pop(grenade.name, transform.position);
            newGrenade.GetComponent<Rigidbody2D>().AddForce(dir * throwSpeed, ForceMode2D.Impulse);

            float throwDir = dir.x > 0 ? -400 : dir.x < 0 ? 400 : 0;
            newGrenade.GetComponent<Grenade>().rotateValue = throwDir;

            newGrenade.GetComponent<Grenade>().bombRadius = bombRadius;
            newGrenade.GetComponent<Grenade>().bombDmg = bombDmg;
            newGrenade.GetComponent<Grenade>().fire = fire == true ? true : false;
            newGrenade.GetComponent<Grenade>().ice = ice == true ? true : false;
            newGrenade.GetComponent<Grenade>().gas = gas == true ? true : false;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Reload()
    {
        grenadeBar.gameObject.SetActive(true);
        canThrow = false;

        float time = 0;
        slider.value = reloadTime;
        while (time < 1)
        {
            yield return new WaitForSeconds(0.05f);
            time += (1 / reloadTime);
            slider.value = time;
        }

        grenadeBar.gameObject.SetActive(false);
        canThrow = true;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        grenadeBar.gameObject.SetActive(false);
        canThrow = true;
    }
}
