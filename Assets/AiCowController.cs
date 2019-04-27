using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCowController : AbstractCowController
{
    public enum AiState
    {
        Initial = 0,
        Idle = 1,
        LookForSpot = 2,
        MovingToSpot = 3
    }

    [SerializeField] private float minIdleTime = 1;
    [SerializeField] private float maxIdleTime = 2;
    [SerializeField] private float minWanderDistance = 2;
    [SerializeField] private float maxWanderDistance = 4.5f;
    [SerializeField] private AiState State = AiState.Initial;

    [SerializeField] private float timeInCurrentState = 0;
    [SerializeField] private float timeForIdling = 0;
    [SerializeField] private Vector2 targetSpot;

    void FixedUpdate()
    {
        timeInCurrentState += Time.deltaTime;

        TargetVelocity = Vector2.zero;

        var position2d = new Vector2(transform.position.x, transform.position.y);
        switch (State)
        {
            case AiState.Initial:
                ChangeState(AiState.Idle);
                break;
            case AiState.Idle:
                if(timeInCurrentState > timeForIdling)
                {
                    ChangeState(AiState.LookForSpot);
                }
                break;
            case AiState.LookForSpot:
                var direction = (Vector2)(Quaternion.Euler(0, 0, Random.value * 360) * Vector2.right);
                var distance = Random.Range(minWanderDistance, maxWanderDistance);
                var hit = Physics2D.Raycast(position2d, direction, distance);
                Debug.DrawRay(position2d, direction*distance, Color.red, 3, false);
                if (hit.collider == null)
                {
                    targetSpot = position2d + direction * distance;
                    ChangeState(AiState.MovingToSpot);
                }
                Debug.Log(hit.collider);
                break;
            case AiState.MovingToSpot:
                distance = Vector2.Distance(targetSpot, position2d);
                if(distance < 0.8f)
                {
                    ChangeState(AiState.Idle);
                }
                else
                {
                    TargetVelocity = targetSpot - position2d;
                }
                break;
        }
    }

    void ChangeState(AiState state)
    {
        State = state;
        timeInCurrentState = 0;
        switch(State)
        {
            case AiState.Idle:
                timeForIdling = Random.Range(minIdleTime, maxIdleTime);
                break;
        }
    }
}
