﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSuitorController : AbstractCowController
{
    private Vector2 position2d;
    [SerializeField] private bool reachedNavPoint = false;
    [SerializeField] private bool navigatingToDoor = false;
    [SerializeField] private GameObject targetPoint;

    override protected void FixedUpdate()
    {
        position2d = Utils.V3toV2(transform.position);

        if(targetPoint != null)
        {
            var targetPositon2d = Utils.V3toV2(targetPoint.transform.position);
            if (Vector2.Distance(position2d, targetPositon2d) < 0.2f)
            {
                if(navigatingToDoor)
                {
                    // TODO GAME OVER
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
        var hit = Physics2D.Raycast(position2d, direction, distance);
        Debug.DrawRay(position2d, direction * distance, Color.blue, 1, false);
        return hit.collider == null;
    }
    
}