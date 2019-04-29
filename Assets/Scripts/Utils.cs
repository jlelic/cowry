using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static iTween;
using System;

public class Utils
{
    public static Vector2 V3toV2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static void tweenColor(Graphic graphic, Color color, float time, float delay = 0, EaseType easeType = EaseType.linear, bool ignoreTimeScale= false)
    {
        ValueTo(graphic.gameObject, Hash(
         "from", graphic.color,
         "to", color,
         "delay", delay,
         "time", time,
         "easetype", easeType,
         "ignoretimescale", ignoreTimeScale,
         "onupdate", (Action<Color>)(newColor =>
         {
             graphic.color = newColor;
         })
        ));
    }

    public static string ToTimeString(float seconds)
    {
        int m = Mathf.FloorToInt(seconds / 60F);
        int s = Mathf.FloorToInt(seconds - m * 60);
        return string.Format("{0:0}:{1:00}", m, s);
    }

    public static void PlayAudio(AudioSource source, AudioClip clip, bool randomPitch = false)
    {
        source.pitch = randomPitch ? UnityEngine.Random.Range(0.75f, 1.5f) : 1;
        source.clip = clip;
        source.Play();
    }
}
