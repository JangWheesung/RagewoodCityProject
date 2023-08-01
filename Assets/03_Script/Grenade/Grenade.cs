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
    public float bombRadius;
    public float bombDmg;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private ParticleSystem particle;

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin vCam;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        particle = transform.GetChild(0).GetComponent<ParticleSystem>();

        cam = FindObjectOfType<CinemachineVirtualCamera>();
        vCam = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        StartCoroutine(Bomb());
    }

    void BombEmpact()
    {
        spriteRenderer.enabled = false;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.localRotation = Quaternion.identity;
        particle.Play();

        DOTween.KillAll();
        vCam.m_AmplitudeGain = 30;
        DOTween.To(() => vCam.m_AmplitudeGain, val => vCam.m_AmplitudeGain = val, 0f, 1f);
    }

    protected void BombAttact()
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, bombRadius, LayerMask.GetMask("Enemy"));
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

        Destroy(gameObject);
    }
}
