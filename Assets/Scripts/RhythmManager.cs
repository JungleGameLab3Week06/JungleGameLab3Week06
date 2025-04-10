using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public AudioSource musicSource; // 배경 음악
    public float bpm = 60f; // 분당 비트 수
    public float beatInterval; // 비트 간격 (초 단위)
    public float nextBeatTime;

    public delegate void BeatEvent();
    public event BeatEvent OnBeat;
    public double CurrentBeatTime => nextBeatTime - beatInterval; // 마지막 비트 발생 시점
    public float BeatInterval => beatInterval;
    private double lastBeatTime;

    void Start()
    {
        beatInterval = 60f / bpm;   
        nextBeatTime = (float)AudioSettings.dspTime + beatInterval;
        musicSource.Play();
    }

    void Update()
    {
        if (AudioSettings.dspTime >= nextBeatTime)
        {
            lastBeatTime = nextBeatTime; // ← 여기 주의! 실제 비트 시점은 nextBeatTime

            //lastBeatTime = nextBeatTime; // 다음 비트 전에 미리 저장!
            Debug.Log($"[Rhythm] 비트 발생 시점 기록: {lastBeatTime:F4}, 현재 시간: {AudioSettings.dspTime:F4}");

            OnBeat?.Invoke();
            nextBeatTime += beatInterval;
        }
    }

    public double GetLastBeatTime() => lastBeatTime;

    public float GetBeatInterval() => beatInterval;
    public float GetNextBeatTime() => nextBeatTime;
}