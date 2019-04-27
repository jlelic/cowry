using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 6f;
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var axisX = Input.GetAxis("Horizontal");
        var axisY = Input.GetAxis("Vertical");

        var targetVelocity = new Vector2(axisX * Speed, axisY * Speed);

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale =  new Vector3(-1,1,1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        rigidBody.velocity = Vector2.Lerp(targetVelocity, rigidBody.velocity, 0.5f);
        
    }
}
