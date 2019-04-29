using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject player;
    bool customTarget = false;
    Vector3 customTargetPosition;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public void SetTarget(Vector3 target)
    {
        customTargetPosition = target;
        customTarget = true;
    }

    public void ClearTarget()
    {
        customTarget = false;
    }

    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var playerPosition = player.transform.position;
        var targetViewPosition = playerPosition + (mousePosition - playerPosition).normalized * 2f;
        if(customTarget)
        {
            targetViewPosition = customTargetPosition;
        }
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(
                targetViewPosition.x,
                targetViewPosition.y,
                transform.position.z
            ),
            0.07f
        );
    }
}
