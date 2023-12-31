using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using DG.Tweening;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float bombTime;

    [Header("bombStat")]
    [HideInInspector] public float bombRadius;
    [HideInInspector] public float bombDmg;
    [HideInInspector] public bool fire;
    [HideInInspector] public bool ice;
    [HideInInspector] public bool gas;
    private float empacTime = 5;

    [Header("Particle")]
    [SerializeField] private GameObject fireParticle;
    [SerializeField] private GameObject iceParticle;
    [SerializeField] private GameObject gasParticle;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private ParticleSystem particle;
    private AudioSource bombSound;

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin vCam;

    public float rotateValue;
    bool stopRotate;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        rb = GetComponent<Rigidbody2D>();

        particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        bombSound = GetComponent<AudioSource>();

        cam = FindObjectOfType<CinemachineVirtualCamera>();
        vCam = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        stopRotate = false;
        StartCoroutine(Bomb());
    }

    private void Update()
    {
        RotateGrenade();
    }

    void RotateGrenade()
    {
        if (!stopRotate)
        {
            rb.rotation += Time.deltaTime * rotateValue;
        }
    }

    void BombEmpact()
    {
        stopRotate = true;
        spriteRenderer.enabled = false;

        rb.rotation = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.localRotation = Quaternion.identity;

        particle.startSize = 0.7f + (0.1f * (bombRadius - 8));
        particle.Play();

        bombSound.Play();

        DOTween.KillAll();
        vCam.m_AmplitudeGain = 30;
        DOTween.To(() => vCam.m_AmplitudeGain, val => vCam.m_AmplitudeGain = val, 0f, 1f);
    }

    private void BombAttact()
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, bombRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in collider2D)
        {
            //만약 이녀석이 방패병이고 터지는 구간 앞에 방패가 있다면 방패병은 물론 방패병 뒤에 있는 놈들도 공격이 막혀야 함
            if (enemy.gameObject.activeSelf)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                Vector2 nuckbackDir = enemy.transform.position.x - transform.position.x > 0 ? Vector2.right : Vector2.left;
                enemy.GetComponent<Living>().OnDamage(bombDmg, (bombRadius - distance) / 2, nuckbackDir);

                if (fire)
                    enemy.GetComponent<Living>().OnFireDamage(bombDmg / 10, empacTime, fireParticle);
                if (ice)
                    enemy.GetComponent<Living>().OnIceDamage(empacTime, iceParticle);
                if (gas)
                {
                    PoolingManager.instance.Pop(gasParticle.name, transform.position);
                    enemy.GetComponent<Living>().OnGasDamage(empacTime);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, bombRadius);
    }

    private IEnumerator Bomb()
    {
        yield return new WaitForSeconds(bombTime);

        BombEmpact();
        BombAttact();

        yield return new WaitForSeconds(1);

        PoolingManager.instance.Push(gameObject);
    }
}
