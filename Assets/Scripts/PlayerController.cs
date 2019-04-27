using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractCowController
{
    private bool pressedEat = false;
    private bool pressedCharge = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        var axisX = Input.GetAxis("Horizontal");
        var axisY = Input.GetAxis("Vertical");
        TargetVelocity = new Vector2(axisX, axisY);

        if (Input.GetAxisRaw("Eat") > 0)
        {
            Eat = !pressedEat;
            pressedEat = true;
        }
        else
        {
            Eat = false;
            pressedEat = false;
        }

        if (Input.GetAxisRaw("Charge") > 0)
        {
            Charge = !pressedCharge;
            pressedCharge = true;
        }
        else
        {
            Charge = false;
            pressedCharge = false;
        }
    }
}
