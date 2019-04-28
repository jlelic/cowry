using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static iTween;

public class UiManager : MonoBehaviour
{
    readonly Color Transparent = new Color(0, 0, 0, 0);

    [SerializeField] private Image overlayPanel;
    [SerializeField] private Text endLevelTitleText;
    [SerializeField] private Text endLevelReasonText;
    [SerializeField] private Text multiKillText;
    [SerializeField] private Text objectiveText;

    void Awake()
    {
        multiKillText.enabled = false;
        endLevelReasonText.enabled = false;
        endLevelTitleText.enabled = false;
        overlayPanel.color = Transparent;
        overlayPanel.enabled = false;
        objectiveText.text = "";
    }

    public void ShowGameOverScreen(string reason = "")
    {
        overlayPanel.gameObject.SetActive(true);
        overlayPanel.enabled = true;
        Utils.tweenColor(overlayPanel, Color.black, 4);

        endLevelReasonText.enabled = true;
        endLevelReasonText.color = Transparent;
        endLevelReasonText.text = reason;
        Utils.tweenColor(endLevelReasonText, Color.white, 2, 2);

        endLevelTitleText.enabled = true;
        endLevelTitleText.color = Transparent;
        endLevelTitleText.text = "GAME OVER";
        endLevelTitleText.color =  Color.red;
        MoveFrom(endLevelTitleText.gameObject, Hash(
             "y", 400,
             "easetype", EaseType.linear,
             "time", 3
        ));
    }

    public void ShowLevelCompletedScreen()
    {
        overlayPanel.gameObject.SetActive(true);
        overlayPanel.enabled = true;
        Utils.tweenColor(overlayPanel, Color.black, 2, 0, EaseType.easeInBounce);

        endLevelReasonText.enabled = false;

        endLevelTitleText.enabled = true;
        endLevelTitleText.color = Transparent;
        endLevelTitleText.text = "LEVEL COMPLETED";
        endLevelTitleText.color = new Color(150,255,150);
        MoveFrom(endLevelTitleText.gameObject, Hash(
             "y", -500,
             "x", 500,
             "easetype", EaseType.easeInOutBounce,
             "time", 2,
             "delay", 1
        ));
        MoveTo(endLevelTitleText.gameObject, Hash(
             "y", 500,
             "x", -500,
             "easetype", EaseType.easeInOutExpo,
             "time", 2,
             "delay", 5
        ));
        RotateAdd(endLevelTitleText.gameObject, Hash(
             "amount", new Vector3(0,0,270),
             "time", 2,
             "delay", 5
        ));
    }

    public void ShowMultiKillMessage(int bodyCount)
    {
        iTween.Stop(multiKillText.gameObject);
        var texts = new [] {
            "DOUBLE KILL",
            "TRIPLE KILL",
            "QUADRUPLE KILL",
            "PENTA KILL",
            "HOLY SHIT!!!",
        };
        var textIndex = bodyCount -2 > texts.Length - 1 ? texts.Length-1 : bodyCount - 2;
        var color = multiKillText.color;
        var solidColor = new Color(color.r, color.g, color.b);
        multiKillText.color = solidColor;
        multiKillText.enabled = true;
        multiKillText.text = texts[textIndex];
        ShakePosition(multiKillText.gameObject, new Vector3(bodyCount, bodyCount), 3);
        var transparentColor = new Color(color.r, color.g, color.b, 0);
        Utils.tweenColor(multiKillText, transparentColor, 1, 2);
    }

    public void ShowObjective(string objective)
    {
        objectiveText.text = objective;
    }

    public void ClearObjective()
    {
        objectiveText.text = "";
    }
}
