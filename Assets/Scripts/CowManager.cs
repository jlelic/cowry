using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private GameObject cowBody;


    private AbstractCowController cowController;
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
        Fatness = 70;
        if (cowController is PlayerController)
        {
            isPlayer = true;
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

    void Update()
    {
        //cowBody.transform.localScale = new Vector2(Mathf.Sign(transform.localScale.x) * Fatness, Fatness);


        if (CanEat.Count > 0 && cowController.Eat && !movement.IsCharging)
        {
            IsEating = true;
            movement.CanMove = false;
        }
        animator.SetBool("isEating", IsEating);
    }

    void OnEatFinish()
    {
        IsEating = false;
        movement.CanMove = true;
        if (CanEat.Count == 0)
        {
            return;
        }
        var eatenGrass = CanEat[0];
        CanEat.RemoveAt(0);
        Destroy(eatenGrass);
        if(isPlayer)
        {
            GameManager.Instance.LevelManager.OnGrassEaten();
        }
        Fatness = Fatness + 15;
    }
}
