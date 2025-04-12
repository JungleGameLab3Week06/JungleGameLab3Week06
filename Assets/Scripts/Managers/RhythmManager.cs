using System;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    [Header("싱글톤")]
    static RhythmManager _instance;
    public static RhythmManager Instance => _instance;

    [Header("비트")]
    AudioSource _beatSource;                                       // 비트 소스
    AudioClip _beatClip;                                             // 비트 클립
    public float BeatInterval => _beatInterval;
    public double LastBeatTime => _lastBeatTime; 
    public double CurrentBeatTime => _nextBeatTime - _beatInterval;
    float _prevBpm = 60f;
    [SerializeField] float _bpm = 60f;                              // 분당 비트 수
    float _beatInterval;                                            // 비트 간격 (초 단위)
    float _nextBeatTime;                                            // 다음 비트 발생 시점
    double _lastBeatTime;                                           // 마지막 비트 발생 시점
    double _currentBeatTime = -1;                                   // 현재 비트 시점
    public Action colorFloorAction;                                 // 비트 발생 시 바닥들 색상 변경하는 것

    [Header("판정")]
    public bool IsJudging => _isJudging;
    public double JudgeWindowStart => _judgeWindowStart;
    public double JudgeWindowEnd => _judgeWindowEnd;
    bool _isJudging = false;                                // 판정 중인지 여부
    double _judgeWindowStart;                               // 판정 시작 지점
    double _judgeWindowEnd;                                 // 판정 종료 지점

    int cnt = 0;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (_prevBpm != _bpm)
        {
            SetBpm();
        }

        if (AudioSettings.dspTime >= _nextBeatTime)
        {
            //Debug.Log($"{cnt} {_nextBeatTime}");
            //Debug.LogError($"테스트 {cnt++}");
            //Debug.Log(AudioSettings.dspTime);

            _lastBeatTime = _nextBeatTime; // ← 여기 주의! 실제 비트 시점은 nextBeatTime
                                           // Debug.Log($"[Rhythm] 비트 발생 시점 기록: {_lastBeatTime:F4}, 현재 시간: {AudioSettings.dspTime:F4}");
            RegularBeat();
            _nextBeatTime += _beatInterval;
        }
        else
        {
            //Debug.Log($"{cnt} {_nextBeatTime}");
            //cnt++;
        }
    }

    public void Init()
    {
        // 싱글톤 초기화
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        // 비트 설정
        _beatSource = GetComponent<AudioSource>();
        AudioClip metronomeClip = Manager.Resource.Load<AudioClip>("Sounds/Beat/Metronome");
        _beatClip = metronomeClip;
        _beatSource.clip = _beatClip;

        SetBpm();
    }

    // BPM 설정
    public void SetBpm()
    {
        _beatInterval = 60f / _bpm;
        _nextBeatTime = (float)AudioSettings.dspTime + _beatInterval;
        _prevBpm = _bpm;
    }

    // 정박자
    void RegularBeat()
    {
        _beatSource.PlayOneShot(_beatClip); // 비트 소리 재생

        if (GameManager.Instance.CurrentEnemy == null)
        {
            GameManager.Instance.SpawnEnemy(); // 적이 없으면 소환
            Debug.LogWarning("적 소환");
        }
        else
        {
            double beatTime = _lastBeatTime;
            if (beatTime == _currentBeatTime)       // 동일 박자 중복 처리 방지
                return;

            _isJudging = true;

            colorFloorAction?.Invoke();                 // 바닥 색상 변경
            GameManager.Instance.CurrentEnemy.Move();   // 적 이동

            float beatInterval = _beatInterval;     // 큰 박자 간격 (BPM 기반)
            float window = beatInterval * 0.5f;     // 성공 구간 (±0.5초, BPM 60 기준 1초)
            _judgeWindowStart = beatTime - window;
            _judgeWindowEnd = beatTime + window;

            //Debug.Log($"[큰 박자] 비트: {beatTime:F4}, 간격: {beatInterval:F4}, 성공 구간: {judgeWindowStart:F4}~{judgeWindowEnd:F4}");

            GameManager.Instance.PlayerController.HasInputThisBeat = false; // 플레이어 입력 초기화
            _currentBeatTime = beatTime;

            // 입력이 없으면 박자 끝에서 Miss
            if (!GameManager.Instance.PlayerController.HasInputThisBeat && AudioSettings.dspTime >= _judgeWindowEnd)
            {
                Debug.Log("입력 없이 큰 박자 종료 - Miss");
                _isJudging = false;
            }
        }
    }

    // 타이밍 판정
    public bool CheckTimingJudgement(double deltaTime)
    {
        float beatInterval = _beatInterval;
        float perfectWindow = beatInterval * 0.5f; // ±0.5초 (BPM 60 기준 1초)
        return (deltaTime <= perfectWindow) ? true : false;
    }
}