using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private AbstractController controller;
    public float Speed = 6f;
    private Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<AbstractController>();
    }

    void Update()
    {
        if (controller.TargetVelocity.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (controller.TargetVelocity.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        rigidBody.velocity = Vector2.Lerp(controller.TargetVelocity, rigidBody.velocity, 0.5f);
    }
}
