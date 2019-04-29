using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakenHandler : MonoBehaviour
{
    private List<Action> listeners = new List<Action>();
    [SerializeField] private GameObject particleEffect;

    public void RegisterListener(Action action)
    {
        listeners.Add(action);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemyMovement = collision.collider.GetComponent<MovementManager>();
        if (enemyMovement!=null && enemyMovement.IsCharging)
        {
            OnDamageTaken(collision.contacts[0].point);
        }
    }

    private void OnDamageTaken(Vector3 contactPoint)
    {
        if(particleEffect != null)
        {
            var pePosition = new Vector3(contactPoint.x, contactPoint.y, -3);
            Instantiate(particleEffect, pePosition, Quaternion.identity);
        }
        foreach (var action in listeners)
        {
            action();
        }
    }
}
