using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    float customTargetZoomInRatio = 2f;
    float chargeZoomInRatio = 1.5f;
    GameObject player;
    MovementManager playerMovement;
    float originalAppu;
    bool customTarget = false;
    PixelPerfectCamera pixelPerfect;
    Vector3 customTargetPosition;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        playerMovement = player.GetComponent<MovementManager>();
        pixelPerfect = GetComponent<PixelPerfectCamera>();
        originalAppu = pixelPerfect.assetsPPU;
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
            0.06f
        );

        var appu = originalAppu;
        if(customTarget)
        {
            appu = originalAppu * customTargetZoomInRatio;
        }
        else if (playerMovement.IsCharging)
        {
            appu = originalAppu * chargeZoomInRatio;
        }
        pixelPerfect.assetsPPU = (int)Mathf.Lerp(pixelPerfect.assetsPPU, appu, 0.15f);
    }
}
