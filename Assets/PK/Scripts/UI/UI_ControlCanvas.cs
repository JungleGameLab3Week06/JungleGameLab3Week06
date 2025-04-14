using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_ControlCanvas : MonoBehaviour
{
    [SerializeField] Image[] btnImage; // Content as Flame, Water, Ground, Lightning

    Coroutine _showPressBtnFXCoroutine;

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

        if (_showPressBtnFXCoroutine == null)
        {
            _showPressBtnFXCoroutine = StartCoroutine(ShowPressBtnFX(_canvasGroup, buttonImage, buttonRect));
        }
    }
    
    private IEnumerator ShowPressBtnFX(CanvasGroup cg, Image buttonImage, RectTransform buttonRect)
    {
        float duration = 0.1f; // 빠른 반응성
        float elapsed = 0f;
        Vector3 originalScale = buttonRect.localScale;
        Quaternion originalRotation = buttonRect.localRotation;
        Color originalColor = buttonImage.color;
        Color pressedColor = Color.Lerp(originalColor, Color.white, 0.6f); // 하얗게 보정 (30% 흰색 혼합)
        pressedColor.a = originalColor.a; // 알파값 유지

        // 눌림 효과: 스케일 축소, 색상 변화
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            buttonRect.localScale = Vector3.Lerp(originalScale, originalScale * 0.7f, t);
            float shake = Mathf.Sin(t * Mathf.PI * 5f) * 3f; // 5번 왕복, 최대 3도
            buttonRect.localRotation = Quaternion.Euler(0, 0, shake);
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
        buttonRect.localRotation = originalRotation;

        buttonImage.color = originalColor;
        cg.alpha = 1f;

        _showPressBtnFXCoroutine = null;
    }
}
