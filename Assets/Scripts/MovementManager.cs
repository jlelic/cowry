﻿using System.Collections;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    public bool CanCharge { get; private set; } = true;
    public bool IsCharging { get; private set; } = false;
    public bool CanMove = true;
    private AbstractController controller;
    private AudioSource audioSource;
    private TrailRenderer trailRenderer;
    public float Speed = 90f;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private Vector2 chargingDirection;
    [SerializeField] private float chargeDuration = 0.45f;
    [SerializeField] private float chargeCooldown = 0.2f;
    [SerializeField] private ParticleSystem[] chargeParticles;
    [SerializeField] private AudioClip chargeClip;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<AbstractController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void FixedUpdate()
    {
        var scale = transform.localScale;
        if (rigidBody.velocity.x > 0.08f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }
        if (rigidBody.velocity.x < 0.08f)
        {
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        if(controller.Charge && CanCharge && CanMove)
        {
            StartCoroutine(Charging());
        }
        var targetVelocity = controller.TargetVelocity.normalized;
        if(!CanMove)
        {
            targetVelocity = Vector2.zero;
        }
        if (IsCharging)
        {
            rigidBody.velocity = chargingDirection.normalized * 13;
        }
        else
        {
            rigidBody.velocity = Vector2.Lerp(targetVelocity * Speed * Time.deltaTime, rigidBody.velocity, 0.5f);
        }
        animator.SetFloat("velocity", rigidBody.velocity.magnitude);
    }

    IEnumerator Charging()
    {
        Utils.PlayAudio(audioSource, chargeClip, true);
        trailRenderer.enabled = true;
        chargingDirection = controller.TargetDirection;
        var strechBase = 7/8f;
        var strechScale = 15/56f;
        var velocityNorm = rigidBody.velocity.normalized;
        var stretchX = strechBase + Mathf.Abs(velocityNorm.x) *strechScale;
        var stretchY = strechBase + Mathf.Abs(velocityNorm.y) * strechScale;
        var oldScale = transform.localScale;
        iTween.ScaleBy(gameObject, iTween.Hash(
             "amount", new Vector3(stretchX/stretchY, stretchY / stretchX, 1),
             "time", 0.08f,
             "delay", 0
        ));
        iTween.ScaleBy(gameObject, iTween.Hash(
             "amount", new Vector3(stretchY/ stretchX, stretchX / stretchY, 1),
             "time", 0.15f,
             "delay", 0.3f
        ));
        foreach(var particleSystem in chargeParticles)
        {
            particleSystem.Play();
        }
        IsCharging = true;
        CanCharge = false;
        yield return new WaitForSeconds(chargeDuration);
        transform.localScale = oldScale;
        foreach (var particleSystem in chargeParticles)
        {
            particleSystem.Stop();
        }
        IsCharging = false;
        trailRenderer.enabled = false;
        yield return new WaitForSeconds(chargeCooldown);
        CanCharge = true;
    }
}
