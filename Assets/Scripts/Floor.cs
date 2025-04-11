using System.Collections;
using UnityEngine;

// 바닥
public class Floor : MonoBehaviour
{
    SpriteRenderer _floorSpriteRenderer;
    [SerializeField] Color _beatColor;             // 비트 발생 시 색상
    [SerializeField] Color _baseColor;             // 기본 색상
    [SerializeField] float _fadeDuration = 0.5f;   // 색상 페이드 시간 (비트 간격의 일부)

    void Start()
    {
        _floorSpriteRenderer = GetComponent<SpriteRenderer>();
        _floorSpriteRenderer.color = _baseColor;                         // 초기 색상 설정
        RhythmManager.Instance.colorFloorAction += HandleBeat;                // 비트 발생 시 호출될 메서드 등록
    }

    // 비트 발생 시 색상 변경 후 페이드 시작
    void HandleBeat()
    {
        StopAllCoroutines();                    // 이전 페이드 중단
        StartCoroutine(FadeColorCoroutine());
    }

    IEnumerator FadeColorCoroutine()
    {
        _floorSpriteRenderer.color = _beatColor; // 비트 시점에 색상 변경
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _floorSpriteRenderer.color = Color.Lerp(_beatColor, _baseColor, elapsed / _fadeDuration);
            yield return null;
        }

        _floorSpriteRenderer.color = _baseColor; // 완전히 기본 색상으로 복귀
    }
}
