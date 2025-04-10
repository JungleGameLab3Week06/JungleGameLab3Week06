using System.Collections;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public RhythmManager rhythmManager; // RhythmManager 참조
    public SpriteRenderer floor;        // 바닥 오브젝트의 SpriteRenderer
    public Color beatColor = Color.red; // 비트 발생 시 색상
    public Color baseColor = Color.white; // 기본 색상
    public float fadeDuration = 0.5f;   // 색상 페이드 시간 (비트 간격의 일부)
    private float beatInterval;
    void Awake()
    {
        if (rhythmManager == null)
            rhythmManager = FindAnyObjectByType<RhythmManager>();
    }
    void Start()
    {
        rhythmManager.OnBeat += HandleBeat; // OnBeat 이벤트 구독
        beatInterval = rhythmManager.GetBeatInterval();
        floor.color = baseColor; // 초기 색상 설정
    }
    void HandleBeat()
    {
        // 비트 발생 시 색상 변경 후 페이드 시작
        StopAllCoroutines(); // 이전 페이드 중단
        StartCoroutine(FadeColor());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FadeColor()
    {
        floor.color = beatColor; // 비트 시점에 색상 변경
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            floor.color = Color.Lerp(beatColor, baseColor, elapsed / fadeDuration);
            yield return null;
        }

        floor.color = baseColor; // 완전히 기본 색상으로 복귀
    }
}
