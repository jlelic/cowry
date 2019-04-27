using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakenHandler : MonoBehaviour
{
    private List<Action> listeners = new List<Action>();

    public void RegisterListener(Action action)
    {
        listeners.Add(action);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemyMovement = collision.collider.GetComponent<MovementManager>();
        if (enemyMovement!=null && enemyMovement.IsCharging)
        {
            InvokeAllListeners();
        }
    }

    private void InvokeAllListeners()
    {
        foreach(var action in listeners)
        {
            action();
        }
    }
}
