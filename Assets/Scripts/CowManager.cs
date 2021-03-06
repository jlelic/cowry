﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CowManager : MonoBehaviour
{

    public List<GameObject> CanEat { get; private set; } = new List<GameObject>();
    public bool IsEating { get; private set; } = false;
    public bool IsStunned { get; private set; } = false;
    public int FatnessLevel { get; private set; } = 0;

    [SerializeField] private SpriteRenderer cowBody;
    [SerializeField] private Sprite[] cowBodySprites;
    [SerializeField] private AudioClip playerStunClip;
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private AudioClip impactWoodClip;
    [SerializeField] private AudioClip squeakClip;
    [SerializeField] private AudioClip eatingClip;
    [SerializeField] private GameObject impactParticleEffect;
    [SerializeField] private GameObject starParticleEffect;
    [SerializeField] private float fatness;

    private AbstractCowController cowController;
    private AudioSource audioSource;
    private Coroutine stunnedCoroutine;
    private MovementManager movement;
    private Animator animator;
    private bool isPlayer = false;
    private FatnessBar fatnessBar;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<MovementManager>();
        fatnessBar = FindObjectOfType<FatnessBar>();
        cowController = GetComponent<AbstractCowController>();
        GetComponent<DamageTakenHandler>().RegisterListener(OnHit);
    }

    private void Start()
    {
        if (cowController is PlayerController)
        {
            isPlayer = true;
            Fatness = FindObjectOfType<LevelManager>().InitialFatness;
        } else
        {
            Fatness = fatness;
        }
    }

    public float Fatness
    {
        get { return fatness; }
        set
        {
            fatness = value;
            var currentFatnessLevel = (int)Mathf.Clamp(fatness / 25, 0, cowBodySprites.Length - 0.1f);
            if (isPlayer)
            {
                GameManager.Instance.LevelManager.OnFatnessChanged(value);
            }
            if (currentFatnessLevel != FatnessLevel)
            {
                if (isPlayer)
                {
                    GameManager.Instance.LevelManager.OnFatnessLevelChanged(currentFatnessLevel);
                }
                switch(currentFatnessLevel)
                {
                    case 0:
                    case 1:
                        animator.speed = 1;
                        break;
                    case 2:
                        animator.speed = 0.85f;
                        break;
                    case 3:
                        animator.speed = 0.75f;
                        break;
                }
                FatnessLevel = currentFatnessLevel;
                movement.Speed = 150 - currentFatnessLevel * 30;
                cowBody.sprite = cowBodySprites[currentFatnessLevel];
            }
            if (fatnessBar != null && isPlayer)
            {
                fatnessBar.SetFatness(value);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GrassBehavior>() != null)
        {
            CanEat.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GrassBehavior>() != null)
        {
            CanEat.Remove(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!movement.IsCharging)
        {
            return;
        }

        var contactPoint = collision.contacts[0].point;
        var pePosition = new Vector3(contactPoint.x, contactPoint.y, -3);

        if (collision.collider.GetComponent<Rock>() != null)
        {
            if (FatnessLevel == 3)
            {
                if (isPlayer)
                {
                    Utils.PlayAudio(audioSource, squeakClip, false);
                }
            }
            else
            {
                if (isPlayer)
                {
                    GameManager.Instance.CameraEffect(Color.red);
                    Utils.PlayAudio(audioSource, playerStunClip, false);
                }
                Instantiate(impactParticleEffect, pePosition, Quaternion.identity);
                StartCoroutine(Stunned());
            }
        }
        if (collision.collider.GetComponent<MaterialWood>() != null || collision.collider.GetComponent<TilemapCollider2D>() != null)
        {
            if (isPlayer)
            {
                Utils.PlayAudio(audioSource, impactWoodClip, true);
            }
            Instantiate(impactParticleEffect, pePosition, Quaternion.identity);
        }
    }

    void FixedUpdate()
    {
        if (CanEat.Count > 0 && cowController.Eat && !movement.IsCharging)
        {
            IsEating = true;
            movement.CanMove = false;
            if (isPlayer)
            {
                Utils.PlayAudio(audioSource, eatingClip, false);
            }
        }
        animator.SetBool("isEating", IsEating);
        animator.SetBool("isStunned", IsStunned);
    }

    void OnEatFinish()
    {
        if (IsStunned)
        {
            IsEating = false;
            return;
        }
        movement.CanMove = true;
        if (!IsEating)
        {
            return;
        }
        IsEating = false;
        if (CanEat.Count == 0)
        {
            return;
        }
        var eatenGrass = CanEat[0].GetComponent<GrassBehavior>();
        CanEat.RemoveAt(0);
        eatenGrass.OnEaten();
        GameManager.Instance.LevelManager.OnGrassEaten(isPlayer);
        if (isPlayer)
        {
            Instantiate(starParticleEffect, transform.position, Quaternion.identity);
        }
        Fatness = Fatness + GameManager.Instance.LevelManager.GrassFatIncrease;
    }

    public void OnHit()
    {
        if(stunnedCoroutine!=null)
        {
            StopCoroutine(stunnedCoroutine);
        }
        stunnedCoroutine = StartCoroutine(Stunned());
        if(!isPlayer)
        {
            GameManager.Instance.LevelManager.OnCowStunned();
        }
        Utils.PlayAudio(audioSource, impactClip, true);
    }

    IEnumerator Stunned()
    {
        IsStunned = true;
        IsEating = false;
        movement.CanMove = false;
        yield return new WaitForSeconds(2f);
        movement.CanMove = true;
        IsStunned = false;
        stunnedCoroutine = null;
    }
}
