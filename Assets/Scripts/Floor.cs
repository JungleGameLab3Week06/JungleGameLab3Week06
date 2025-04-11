using System.Collections;
using UnityEditor;
using UnityEngine;

public class Floor : MonoBehaviour
{
    SpriteRenderer _floorSpriteRenderer;
    [SerializeField] Color _beatColor;       // 비트 발생 시 색상
    [SerializeField] Color _baseColor;       // 기본 색상
    [SerializeField] float _fadeDuration = 0.5f;   // 색상 페이드 시간 (비트 간격의 일부)
    [SerializeField] bool _colorChange = true; // 색상 변경 여부
    void Start()
    {
        _floorSpriteRenderer = GetComponent<SpriteRenderer>();
        _floorSpriteRenderer.color = _baseColor;                         // 초기 색상 설정
        RhythmManager.Instance.beatAction += HandleBeat;                // 비트 발생 시 호출될 메서드 등록
    }

    // 비트 발생 시 색상 변경 후 페이드 시작
    void HandleBeat()
    {
        StopAllCoroutines();                    // 이전 페이드 중단
        StartCoroutine(FadeColorCoroutine());
    }

    IEnumerator FadeColorCoroutine()
    {
        if (_colorChange)
        {
            _floorSpriteRenderer.color = _beatColor; // 비트 시점에 색상 변경
            float elapsed = 0f;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                _floorSpriteRenderer.color = Color.Lerp(_beatColor, _baseColor, elapsed / _fadeDuration);
                yield return null;
            }
            _colorChange = false;
            _floorSpriteRenderer.color = _baseColor; // 완전히 기본 색상으로 복귀
        }
        else if(!_colorChange)
        {
            _floorSpriteRenderer.color = _baseColor; // 비트 시점에 색상 변경
            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                _floorSpriteRenderer.color = Color.Lerp(_baseColor, _beatColor, elapsed / _fadeDuration);
                yield return null;
            }
            _colorChange = true;
            _floorSpriteRenderer.color = _beatColor; // 완전히 기본 색상으로 복귀
        }
    }
}
