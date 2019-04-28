using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractController : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    public bool Charge { get; protected set; } = false;
    public Vector2 TargetVelocity { get; protected set; } = Vector2.zero;
    public Vector2 TargetDirection { get; protected set; } = Vector2.zero;

    virtual protected void FixedUpdate()
    {
        Charge = false;
    }
}
