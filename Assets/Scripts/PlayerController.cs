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

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetDirection = Utils.V3toV2(mousePosition - transform.position);

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

        if (Input.GetAxisRaw("Charge") > 0 || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
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
