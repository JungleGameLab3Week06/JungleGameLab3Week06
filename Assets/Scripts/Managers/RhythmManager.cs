using System;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    [Header("싱글톤")]
    static RhythmManager _instance;
    public static RhythmManager Instance => _instance;

    [Header("비트")]
    public Action beatAction;
    public float BeatInterval => _beatInterval;
    public double LastBeatTime => _lastBeatTime; // 마지막 비트 발생 시점
    public double CurrentBeatTime => _nextBeatTime - _beatInterval; // 마지막 비트 발생 시점
    [SerializeField] float _bpm = 60f;          // 분당 비트 수
    float _beatInterval;                        // 비트 간격 (초 단위)
    float _nextBeatTime;
    double _lastBeatTime;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _beatInterval = 60f / _bpm;   
        _nextBeatTime = (float)AudioSettings.dspTime + _beatInterval;
    }

    void Update()
    {
        if (AudioSettings.dspTime >= _nextBeatTime)
        {
            _lastBeatTime = _nextBeatTime; // ← 여기 주의! 실제 비트 시점은 nextBeatTime

            //lastBeatTime = nextBeatTime; // 다음 비트 전에 미리 저장!
            //Debug.Log($"[Rhythm] 비트 발생 시점 기록: {_lastBeatTime:F4}, 현재 시간: {AudioSettings.dspTime:F4}");
            beatAction?.Invoke();
            _nextBeatTime += _beatInterval;
        }
    }
}