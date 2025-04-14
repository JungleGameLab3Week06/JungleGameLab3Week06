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
    Color colorMiss;

    void Start()
    {
        _judgeText = GetComponentInChildren<TextMeshProUGUI>();

        // Sets color
        ColorUtility.TryParseHtmlString("#ffc815", out colorPerfect);
        ColorUtility.TryParseHtmlString("#a6a6a6", out colorMiss);

        Manager.UI.showJudgeTextAction += ShowJudge; // Subscribe to the event
    }

    // 판정 결과 보여주기
    public void ShowJudge(bool isPerfect)
    {
        if (judgeCoroutine != null)
            StopCoroutine(judgeCoroutine);
        judgeCoroutine = StartCoroutine(ShowJudgeCoroutine(isPerfect));
    }

    // 판정 결과 보여주기 코루틴
    private IEnumerator ShowJudgeCoroutine(bool isPerfect)
    {
        // Set Text
        _judgeText.text = (isPerfect) ? "Success" : "Miss";
        _judgeText.color = (isPerfect) ? colorPerfect : colorMiss;

        // Fade out text
        float alphaValue = 1f;
        _judgeText.color = new Color(_judgeText.color.r, _judgeText.color.g, _judgeText.color.b, alphaValue);
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
