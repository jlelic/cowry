using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FatnessBar : MonoBehaviour
{
    [SerializeField] private Image barFill;
    private float fullLength;
    private float fatness = 0;
    private Coroutine glowingCoroutine;
    private RectTransform rectTransform;
    [SerializeField] private Color[] barColors;
    [SerializeField] private int glowLimit = 75;

    void Awake()
    {
        rectTransform = barFill.transform as RectTransform;
        fullLength = rectTransform.sizeDelta.x;
        if(barColors.Length == 0)
        {
            throw new System.Exception("FATNESS BAR COLORS UNDEFINED");
        }
    }

    public void SetFatness(float value)
    {
        value = Mathf.Clamp(value, 0, 100);
        rectTransform.sizeDelta = new Vector2(value/100*fullLength, rectTransform.sizeDelta.y);
        barFill.color = GetColorForValue(value);
        fatness = value;
    }

    private Color GetColorForValue(float value)
    {
        return barColors[(int)(barColors.Length * (value / 100))];
    }

    void Update()
    {
        if(fatness > glowLimit && glowingCoroutine == null)
        {
            glowingCoroutine = StartCoroutine(Glowing());
        }
        else if(fatness < glowLimit && glowingCoroutine != null)
        {
            StopCoroutine(glowingCoroutine);
            glowingCoroutine = null;
        }
    }

    IEnumerator Glowing()
    {
        var loopTime = 0.5f;
        var shakeCount = 2;
        while(true)
        {
            for (int i = 0; i < shakeCount; i++)
            {
                iTween.ShakePosition(barFill.gameObject, iTween.Hash(
                    "amount", new Vector3(2, 2, 5),
                    "time", loopTime / shakeCount,
                    "delay", loopTime / shakeCount * i
                ));
            }
            var originalColor = GetColorForValue(fatness);
            var lighterColor = new Color(
                originalColor.r + 0.2f,
                originalColor.g + 0.2f,
                originalColor.b + 0.2f,
                originalColor.a
                );
            Utils.tweenColor(barFill, lighterColor, loopTime / 2);
            Utils.tweenColor(barFill, originalColor, loopTime / 2, loopTime/2);
            yield return new WaitForSeconds(loopTime);
        }
    }
}
