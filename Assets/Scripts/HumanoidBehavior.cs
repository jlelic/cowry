using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBehavior : MonoBehaviour
{
    [SerializeField] private bool isRich = false;
    [SerializeField] AudioClip hitClip;
    DamageTakenHandler damageTakenHandler;

    public bool IsRich { get { return isRich; } }

    void Start()
    {
        damageTakenHandler = GetComponent<DamageTakenHandler>();
        damageTakenHandler.RegisterListener(OnDamageTaken);
    }
    
    void OnDamageTaken()
    {
        StartCoroutine(DamageTakenSlowMo());
        GameManager.Instance.CameraEffect(Color.white);
        Utils.PlayAudio(GetComponent<AudioSource>(), hitClip);
        Die();
    }

    void Die()
    {
        float delay = 2f;
        GetComponent<Animator>().SetBool("isFalling", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<MovementManager>().enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        iTween.ScaleTo(gameObject, Vector3.one * 2, delay);
        GameManager.Instance.OnSuitorKilled(isRich);
        StartCoroutine(DestroyAfterDelay(delay));
    }

    IEnumerator DestroyAfterDelay(float delay = 2f)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    IEnumerator DamageTakenSlowMo()
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(0.03f);
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.45f);
        if (isRich)
        {
            Time.timeScale = 0.5f;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
    }
}
