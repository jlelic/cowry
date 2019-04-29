using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] ambienceClips;

    private AudioSource audioSource;
    private float timeToSound = 5;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        timeToSound -= Time.deltaTime;
        if(timeToSound <= 0)
        {
            Utils.PlayAudio(audioSource, ambienceClips[(int)(Random.value * ambienceClips.Length)], true);
            timeToSound = Random.RandomRange(5, 12);
        }
    }
}
