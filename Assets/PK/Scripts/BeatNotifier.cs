using System.Collections;
using UnityEngine;

public class BeatNotifier : MonoBehaviour
{
    [Header("Instances and Progress")]
    public static BeatNotifier Instance; // Instance which will be used for UI
    private Coroutine beatCoroutine; // Coroutine which controls current work

    CanvasGroup beatNotifierCanvas;
    readonly float _displayDuration = 0.1f;

    void Start()
    {
        Instance = this;

        beatNotifierCanvas = GetComponent<CanvasGroup>();
    }

    // Externally Accessible
    public void ShowBeat()
    {
        if (beatCoroutine != null) StopCoroutine(beatCoroutine);
        beatCoroutine = StartCoroutine(ShowBeatCo());
    }


    private IEnumerator ShowBeatCo()
    {
        // Show
        float alphaValue = 1;
        beatNotifierCanvas.alpha = alphaValue;
         
        // Hide
        while(alphaValue > 0)
        {
            alphaValue -= Time.deltaTime * _displayDuration;
            beatNotifierCanvas.alpha = alphaValue;
            yield return null;
        }

        // Conceal Completely
        alphaValue = 0;
        beatNotifierCanvas.alpha = alphaValue;
    }
}
