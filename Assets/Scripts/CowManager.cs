using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowManager : MonoBehaviour
{
    public float Fatness { get; private set; } = 1;
    public bool CanEat { get; private set; } = false;
    public bool IsEating { get; private set; } = false;

    [SerializeField] private GameObject cowBody;


    private AbstractCowController cowController;
    private Animator animator;
    private bool isPlayer = false;
    private GameObject targetGrass;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cowController = GetComponent<AbstractCowController>();
        if (cowController is PlayerController)
        {
            isPlayer = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GrassBehavior>() != null)
        {
            CanEat = true;
            targetGrass = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GrassBehavior>() != null)
        {
            CanEat = false;
            targetGrass = null;
        }
    }

    void Update()
    {
        cowBody.transform.localScale = new Vector2(Mathf.Sign(transform.localScale.x) * Fatness, Fatness);


        if (CanEat && cowController.Eat)
        {
            IsEating = true;
        }
        animator.SetBool("isEating", IsEating);
    }

    void OnEatFinish()
    {
        Destroy(targetGrass);
        IsEating = false;
        Fatness += 0.2f;
    }
}
