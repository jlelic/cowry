using System.Collections;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    public bool IsCharging { get; private set; }
    public bool CanMove = true;
    private AbstractController controller;
    public float Speed = 90f;
    private Animator animator;
    private Rigidbody2D rigidBody;
    [SerializeField] private ParticleSystem[] chargeParticles;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        controller = GetComponent<AbstractController>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        var scale = transform.localScale;
        if (controller.TargetVelocity.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }
        if (controller.TargetVelocity.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        if(controller.Charge)
        {
            StartCoroutine(Charging());
        }
        var targetVelocity = controller.TargetVelocity.normalized;
        if(!CanMove)
        {
            targetVelocity = Vector2.zero;
        }
        if (IsCharging)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * 13;
        }
        else
        {
            rigidBody.velocity = Vector2.Lerp(targetVelocity * Speed * Time.deltaTime, rigidBody.velocity, 0.5f);
        }
        animator.SetFloat("velocity", rigidBody.velocity.magnitude);
    }

    IEnumerator Charging()
    {
        var chargingTime = 0.45f;
        var strechBase = 7/8f;
        var strechScale = 15/56f;
        var velocityNorm = rigidBody.velocity.normalized;
        var stretchX = strechBase + Mathf.Abs(velocityNorm.x) *strechScale;
        var stretchY = strechBase + Mathf.Abs(velocityNorm.y) * strechScale;
        var oldScale = transform.localScale;
        iTween.ScaleBy(gameObject, iTween.Hash(
             "amount", new Vector3(stretchX/stretchY, stretchY / stretchX, 1),
             "time", 0.08f,
             "delay", 0
        ));
        iTween.ScaleBy(gameObject, iTween.Hash(
             "amount", new Vector3(stretchY/ stretchX, stretchX / stretchY, 1),
             "time", 0.15f,
             "delay", 0.3f
        ));
        foreach(var particleSystem in chargeParticles)
        {
            particleSystem.Play();
        }
        IsCharging = true;
        yield return new WaitForSeconds(chargingTime);
        transform.localScale = oldScale;
        foreach (var particleSystem in chargeParticles)
        {
            particleSystem.Stop();
        }
        IsCharging = false;
    }
}
