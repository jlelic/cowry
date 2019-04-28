using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSuitorController : AbstractCowController
{
    private Coroutine knockingCoroutine;
    private Vector2 position2d;
    [SerializeField] private bool navigatingToDoor = false;
    [SerializeField] private bool knocked = false;
    [SerializeField] private GameObject targetPoint;

    private void Awake()
    {
        GetComponent<DamageTakenHandler>().RegisterListener(OnDamageTaken);    
    }

    override protected void FixedUpdate()
    {
        position2d = Utils.V3toV2(transform.position);

        if(targetPoint != null)
        {
            var targetPositon2d = Utils.V3toV2(targetPoint.transform.position);
            if (Vector2.Distance(position2d, targetPositon2d) < 0.25f)
            {
                TargetVelocity = Vector2.zero;
                if(navigatingToDoor)
                {
                    if (!knocked)
                    {
                        knockingCoroutine = StartCoroutine(KnockOnTheDoor());
                    }
                }
                else
                {
                    navigatingToDoor = true;
                    targetPoint = null;
                }
            }
            else
            {
                TargetVelocity = targetPositon2d - position2d;
            }
        }
        else
        {
            targetPoint = FindClosestPoint(GameManager.Instance.DoorEntranceList);
            if(!navigatingToDoor && targetPoint == null)
            {
                targetPoint = FindClosestPoint(GameManager.Instance.NavPointList);
            }
        }
    }

    private void OnDamageTaken()
    {
        if(knockingCoroutine != null)
        {
            StopCoroutine(knockingCoroutine);
        }
    }

    private GameObject FindClosestPoint(HashSet<GameObject> list)
    {
        var closestDistance = 9999999999999999f;
        GameObject result = null;
        foreach(var point in list)
        {
            if(IsReachable(point.transform))
            {
                var distance = Vector2.Distance(position2d, Utils.V3toV2(point.transform.position));
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    result = point.gameObject;
                }
            }
        }
        return result;
    }

    private bool IsReachable(Transform target)
    {
        var targetPosition2d = new Vector2(target.position.x, target.position.y);
        var direction = targetPosition2d - position2d;
        var distance = Vector2.Distance(targetPosition2d, position2d);
        var hit = Physics2D.Raycast(position2d, direction, distance, (1 << 9)-1);
        Debug.DrawRay(position2d, direction * distance, Color.blue, 1, false);
        return hit.collider == null;
    }

    IEnumerator KnockOnTheDoor()
    {
        var doorEntrance = targetPoint.GetComponent<DoorEntrance>();
        doorEntrance.Knock();
        knocked = true;
        yield return new WaitForSeconds(3);
        if (GetComponent<HumanoidBehavior>().IsRich)
        {
            GameManager.Instance.LevelManager.OnRichSuitorEntered();
        }
        else
        {
            GameManager.Instance.LevelManager.OnPoorSuitorEntered();
        }
    }
    
}
