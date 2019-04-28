using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CowManager : MonoBehaviour
{
    public float Fatness {
        get{ return fatness; }
        set {
            fatness = value;
            if (fatnessBar != null && isPlayer)
            {
                fatnessBar.SetFatness(value);
            }
        }
    }
    public List<GameObject> CanEat { get; private set; } = new List<GameObject>();
    public bool IsEating { get; private set; } = false;
    public bool IsStunned { get; private set; } = false;
    public int FatnessLevel { get; private set; } = 0;

    [SerializeField] private SpriteRenderer cowBody;
    [SerializeField] private Sprite[] cowBodySprites;

    private AbstractCowController cowController;
    private Coroutine stunnedCoroutine;
    private MovementManager movement;
    private Animator animator;
    private bool isPlayer = false;
    private FatnessBar fatnessBar;
    private float fatness;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
        if(movement.IsCharging && collision.collider.GetComponent<Rock>() != null)
        {
            StartCoroutine(Stunned());
        }
    }

    void FixedUpdate()
    {
        var currentFatnessLevel = (int)Mathf.Clamp(fatness / 25, 0, cowBodySprites.Length - 0.1f);
        if (currentFatnessLevel != FatnessLevel)
        {
            FatnessLevel = currentFatnessLevel;
            movement.Speed = 150 - currentFatnessLevel * 30;
            cowBody.sprite = cowBodySprites[currentFatnessLevel];
        }

        if (CanEat.Count > 0 && cowController.Eat && !movement.IsCharging)
        {
            IsEating = true;
            movement.CanMove = false;
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
        var eatenGrass = CanEat[0];
        CanEat.RemoveAt(0);
        Destroy(eatenGrass);
        GameManager.Instance.LevelManager.OnGrassEaten(isPlayer);
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
