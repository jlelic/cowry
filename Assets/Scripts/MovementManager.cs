using System.Collections;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public bool IsCharging { get; private set; }
    private AbstractController controller;
    public float Speed = 90f;
    private Animator animator;
    private Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<AbstractController>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (controller.TargetVelocity.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (controller.TargetVelocity.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if(controller.Charge)
        {
            StartCoroutine(Charging());
        }
        if(IsCharging)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * 13;
        }
        else
        {
            rigidBody.velocity = Vector2.Lerp(controller.TargetVelocity.normalized * Speed * Time.deltaTime, rigidBody.velocity, 0.5f);
        }
        animator.SetFloat("velocity", rigidBody.velocity.magnitude);
    }

    IEnumerator Charging()
    {
        IsCharging = true;
        yield return new WaitForSeconds(0.45f);
        IsCharging = false;
    }
}
