using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.NavPointList.Add(gameObject);   
    }

    private void OnDestroy()
    {
        GameManager.Instance.NavPointList.Remove(gameObject);
    }
}
