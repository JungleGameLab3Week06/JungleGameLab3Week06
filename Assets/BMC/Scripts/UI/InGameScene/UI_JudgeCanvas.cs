using System.Collections;
using TMPro;
using UnityEngine;

// 판정 결과 캔버스
public class UI_JudgeCanvas : MonoBehaviour
{
    Coroutine judgeCoroutine;              // Coroutine which controls current work

    [Header("Components")]
    TextMeshProUGUI _judgeText;
    float _disappearWeight = 1f;

    [Header("Color of Judgements")]
    Color colorPerfect;
    Color colorGood;
    Color colorMiss;

    void Start()
    {
        _judgeText = GetComponentInChildren<TextMeshProUGUI>();

        // Sets color
        ColorUtility.TryParseHtmlString("#ffc815", out colorPerfect);
        ColorUtility.TryParseHtmlString("#1f80ff", out colorGood);
        ColorUtility.TryParseHtmlString("#a6a6a6", out colorMiss);

        Manager.UI.showJudgeTextAction += ShowJudge; // Subscribe to the event
    }

    // 판정 결과 보여주기
    public void ShowJudge(string result)
    {
        if (judgeCoroutine != null)
            StopCoroutine(judgeCoroutine);
        judgeCoroutine = StartCoroutine(ShowJudgeCoroutine(result));
    }

    // 판정 결과 보여주기 코루틴
    private IEnumerator ShowJudgeCoroutine(string result)
    {
        float alphaValue = 1f;

        // Set Text
        _judgeText.text = result;
        _judgeText.color = new Color(_judgeText.color.r, _judgeText.color.g, _judgeText.color.b, alphaValue);

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
            alphaValue -= Time.deltaTime * _disappearWeight;
            _judgeText.color = new Color(_judgeText.color.r, _judgeText.color.g, _judgeText.color.b, alphaValue);
            yield return null;
        }
        
        // Hide Text
        alphaValue = 0;
        _judgeText.text = "";
        _judgeText.color = new Color(_judgeText.color.r, _judgeText.color.g, _judgeText.color.b, alphaValue);
    }
}
