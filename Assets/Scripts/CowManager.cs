using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowManager : MonoBehaviour
{
    public float Fatness { get; private set; } = 1;
    public List<GameObject> CanEat { get; private set; } = new List<GameObject>();
    public bool IsEating { get; private set; } = false;

    [SerializeField] private GameObject cowBody;


    private AbstractCowController cowController;
    private Animator animator;
    private Rigidbody2D r2d;
    private bool isPlayer = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();
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
        // cowBody.transform.localScale = new Vector2(Mathf.Sign(transform.localScale.x) * Fatness, Fatness);


        if (CanEat.Count > 0 && cowController.Eat)
        {
            IsEating = true;
        }
        animator.SetBool("isEating", IsEating);
        animator.SetFloat("velocity", r2d.velocity.magnitude);
    }

    void OnEatFinish()
    {
        IsEating = false;
        if (CanEat.Count == 0)
        {
            return;
        }
        var eatenGrass = CanEat[0];
        CanEat.RemoveAt(0);
        Destroy(eatenGrass);
        Fatness += 0.2f;
    }
}
