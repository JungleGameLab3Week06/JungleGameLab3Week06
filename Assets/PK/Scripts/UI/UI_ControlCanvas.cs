using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_ControlCanvas : MonoBehaviour
{
    [SerializeField] Image[] btnImage; // Content as Flame, Water, Ground, Lightning

    void Start()
    {
        InputManager.Instance.makePressBtnFX += MakePressBtnFX;
    }

    // Update is called once per frame
    public void MakePressBtnFX(int index)
    {
        CanvasGroup _canvasGroup = btnImage[index].GetComponent<CanvasGroup>();
        Image buttonImage = btnImage[index].GetComponent<Image>();
        RectTransform buttonRect = btnImage[index].GetComponent<RectTransform>();
        if (_canvasGroup){
            StartCoroutine(ShowPressBtnFX(_canvasGroup, buttonImage, buttonRect));
        }
    }
    
    private IEnumerator ShowPressBtnFX(CanvasGroup cg, Image buttonImage, RectTransform buttonRect)
    {
        float duration = 0.1f; // 빠른 반응성
        float elapsed = 0f;
        Vector3 originalScale = buttonRect.localScale;
        Color originalColor = buttonImage.color;
        Color pressedColor = originalColor * 0.85f; // 살짝 어두운 색상
        pressedColor.a = originalColor.a; // 알파값 유지

        // 눌림 효과: 스케일 축소, 색상 변화
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            buttonRect.localScale = Vector3.Lerp(originalScale, originalScale * 0.7f, t);
            buttonImage.color = Color.Lerp(originalColor, pressedColor, t);
            cg.alpha = Mathf.Lerp(1f, 0.5f, t);

            yield return null;
        }

        // 복원 효과
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            buttonRect.localScale = Vector3.Lerp(originalScale * 0.9f, originalScale, t);
            buttonImage.color = Color.Lerp(pressedColor, originalColor, t);
            cg.alpha = Mathf.Lerp(0.7f, 1f, t);

            yield return null;
        }

        // 최종 상태 보장
        buttonRect.localScale = originalScale;
        buttonImage.color = originalColor;
        cg.alpha = 1f;
    }
}
