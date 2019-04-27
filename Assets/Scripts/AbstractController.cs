using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractController : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    public Vector2 TargetVelocity { get; protected set; } = Vector2.zero;
}
