using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCowController : AbstractController
{
    public bool Eat { get; protected set; } = false;

    protected void Update()
    {
        Eat = false;
    }
}
