using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var playerPosition = player.transform.position;
        var targetViewPosition = playerPosition + (mousePosition - playerPosition).normalized * 2f;
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(
                targetViewPosition.x,
                targetViewPosition.y,
                transform.position.z
            ),
            0.2f
        );
    }
}
