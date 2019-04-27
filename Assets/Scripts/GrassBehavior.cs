using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBehavior : MonoBehaviour
{
    [SerializeField] private Color highlightColor = Color.red;
    private Color baseColor = Color.black;
    private SpriteRenderer renderer;

    void Awake()
    {
        // grow
        transform.localScale = new Vector3(0, 0, 0);
        iTween.MoveBy(gameObject, new Vector3(0, -0.5f, 0), 0.0001f);
        iTween.MoveBy(gameObject, new Vector3(0, +0.5f, 0), 3f);
        iTween.ScaleTo(gameObject, new Vector3(1, 1, 1), 3f);

        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (baseColor == Color.black)
            {
                baseColor = renderer.color;
            }
            renderer.color = highlightColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            renderer.color = baseColor;
        }
    }

    void Update()
    {

    }
}
