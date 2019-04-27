using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractCowController
{
    private bool pressedEat = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        var axisX = Input.GetAxis("Horizontal");
        var axisY = Input.GetAxis("Vertical");
        TargetVelocity = new Vector2(axisX, axisY);

        if(Input.GetAxisRaw("Eat") > 0)
        {
            Eat = !pressedEat;
            pressedEat = true;
        }
        else
        {
            Eat = false;
            pressedEat = false;
        }
    }
}
