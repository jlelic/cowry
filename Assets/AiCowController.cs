using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCowController : AbstractCowController
{
    public enum AiState
    {
        Initial = 0,
        Idle = 1,
        LookForGrass = 2,
        LookForSpot = 3,
        MovingToSpot = 4,
    }

    [SerializeField] private float minIdleTime = 1;
    [SerializeField] private float maxIdleTime = 2;
    [SerializeField] private float minWanderDistance = 2;
    [SerializeField] private float maxWanderDistance = 4.5f;
    [SerializeField] private AiState State = AiState.Initial;

    [SerializeField] private float timeInCurrentState = 0;
    [SerializeField] private float timeForIdling = 0;
    [SerializeField] private Vector2 targetSpot;

    private CowManager manager;

    private void Awake()
    {
        manager = GetComponent<CowManager>();
    }

    void FixedUpdate()
    {
        timeInCurrentState += Time.deltaTime;

        TargetVelocity = Vector2.zero;

        if(manager.IsEating)
        {
            return;
        }
        if(timeInCurrentState > 10)
        {
            ChangeState(AiState.Idle);
            return;
        }
        if(manager.CanEat.Count == 0)
        {
            Eat = false;    
        }
        else 
        {
            Eat = true;
            ChangeState(AiState.Idle);
            return;
        }

        var position2d = new Vector2(transform.position.x, transform.position.y);
        switch (State)
        {
            case AiState.Initial:
                ChangeState(AiState.Idle);
                break;
            case AiState.Idle:
                if(timeInCurrentState > timeForIdling)
                {
                    ChangeState(AiState.LookForGrass);
                }
                break;
            case AiState.LookForGrass:
                var foundGrass = false;
                foreach (var grass in GameManager.Instance.GrassList)
                {
                    var grassPosition2d = new Vector2(grass.transform.position.x, grass.transform.position.y);
                    var grassDirection = grassPosition2d - position2d;
                    var grassDistance = Vector2.Distance(grassPosition2d, position2d);
                    var grassHit = Physics2D.Raycast(position2d, grassDirection, grassDistance);
                    Debug.DrawRay(position2d, grassDirection * grassDistance, Color.green, 1, false);
                    if (grassHit.collider==null)
                    {
                        targetSpot = grassPosition2d;
                        ChangeState(AiState.MovingToSpot);
                        foundGrass = true;
                        break;
                    }
                    else
                    {
                        Debug.Log(grassHit.collider);
                    }
                }
                if(!foundGrass)
                {
                    ChangeState(AiState.LookForSpot);
                }
                break;
            case AiState.LookForSpot:
                var direction = (Vector2)(Quaternion.Euler(0, 0, Random.value * 360) * Vector2.right);
                var distance = Random.Range(minWanderDistance, maxWanderDistance);
                var hit = Physics2D.Raycast(position2d, direction, distance);
                Debug.DrawRay(position2d, direction*distance, Color.red, 1, false);
                if (hit.collider == null)
                {
                    targetSpot = position2d + direction * distance;
                    ChangeState(AiState.MovingToSpot);
                }
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
        Debug.Log("CHANGE STATE TO " + state);
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
