using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCowController : AbstractController
{
    public bool Eat { get; protected set; } = false;
    public Vector2 LastVelocity { get; private set; } = Vector2.zero;


    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        Eat = false;
        LastVelocity = rigidBody.velocity;
    }
}
