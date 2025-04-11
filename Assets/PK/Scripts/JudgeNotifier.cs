using System.Collections;
using TMPro;
using UnityEngine;

public class JudgeNotifier : MonoBehaviour
{
    [Header("Instances and Progress")]
    public static JudgeNotifier Instance; // Instance which will be used for UI
    private Coroutine judgeCoroutine; // Coroutine which controls current work

    [Header("Components")]
    private TextMeshProUGUI _judgeText;
    private CanvasGroup _canvasGroup;
    private readonly float _displayDuration = 0.16f;

    [Header("Color of Judgements")]
    private Color colorPerfect;
    private Color colorGood;
    private Color colorMiss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        _judgeText = GetComponent<TextMeshProUGUI>();
        _canvasGroup = GetComponent<CanvasGroup>();

        // Sets color
        ColorUtility.TryParseHtmlString("#ffc815", out colorPerfect);
        ColorUtility.TryParseHtmlString("#1f80ff", out colorGood);
        ColorUtility.TryParseHtmlString("#a6a6a6", out colorMiss);
    }

    // Externally Accessible
    public void ShowJudge(string result)
    {
        if (judgeCoroutine != null) StopCoroutine(judgeCoroutine);
        judgeCoroutine = StartCoroutine(ShowJudgeCoroutine(result));
    }

    private IEnumerator ShowJudgeCoroutine(string result)
    {
        // Set Text
        float alphaValue = 1;
        _canvasGroup.alpha = alphaValue;
        _judgeText.text = result;

        // Set Color
        switch (result)
        {
            case "Perfect":
                _judgeText.color = colorPerfect;
                break;
            case "Good":
                _judgeText.color = colorGood;
                break;
            case "Miss":
                _judgeText.color = colorMiss;
                break;
        }

        // Fade out text
        while(alphaValue > 0)
        {
            alphaValue -= Time.deltaTime * _displayDuration;
            _canvasGroup.alpha = alphaValue;
            yield return null;
        }
        
        // Hide Text
        alphaValue = 0;
        _judgeText.text = "";
        _canvasGroup.alpha = alphaValue;
    }
}
