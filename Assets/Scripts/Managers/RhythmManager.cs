using System;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    [Header("싱글톤")]
    static RhythmManager _instance;
    public static RhythmManager Instance => _instance;

    [Header("비트")]
    public Action colorFloorAction;                                 // 비트 발생 시 바닥들 색상 변경하는 것
    public float BeatInterval => _beatInterval;
    public double LastBeatTime => _lastBeatTime; 
    public double CurrentBeatTime => _nextBeatTime - _beatInterval;
    [SerializeField] float _bpm = 60f;                              // 분당 비트 수
    float _beatInterval;                                            // 비트 간격 (초 단위)
    float _nextBeatTime;                                            // 다음 비트 발생 시점
    double _lastBeatTime;                                           // 마지막 비트 발생 시점
    double _currentBeatTime = -1;                                   // 현재 비트 시점

    [Header("판정")]
    public bool IsJudging => _isJudging;
    public double JudgeWindowStart => _judgeWindowStart;
    public double JudgeWindowEnd => _judgeWindowEnd;
    bool _isJudging = false;                                // 판정 중인지 여부
    double _judgeWindowStart;                               // 판정 시작 지점
    double _judgeWindowEnd;                                 // 판정 종료 지점

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (AudioSettings.dspTime >= _nextBeatTime)
        {
            _lastBeatTime = _nextBeatTime; // ← 여기 주의! 실제 비트 시점은 nextBeatTime

            //lastBeatTime = nextBeatTime; // 다음 비트 전에 미리 저장!
            //Debug.Log($"[Rhythm] 비트 발생 시점 기록: {_lastBeatTime:F4}, 현재 시간: {AudioSettings.dspTime:F4}");
            HandleBeat();
            colorFloorAction?.Invoke();         // 바닥 색상 변경
            _nextBeatTime += _beatInterval;
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
        _beatInterval = 60f / _bpm;
        _nextBeatTime = (float)AudioSettings.dspTime + _beatInterval;
    }

    // 비트에 맞게 호출하는 함수
    void HandleBeat()
    {
        if (GameManager.Instance.CurrentEnemy == null)
        {
            GameManager.Instance.SpawnEnemy(); // 적이 없으면 소환
        }
        else
        {
            GameManager.Instance.CurrentEnemy.Move();

            double beatTime = _lastBeatTime;
            if (beatTime == _currentBeatTime) return; // 동일 박자 중복 처리 방지

            float beatInterval = _beatInterval; // 큰 박자 간격 (BPM 기반)
            float window = beatInterval * 0.3f; // 성공 구간 (±0.3초, BPM 60 기준 0.6초)
            _judgeWindowStart = beatTime - window;
            _judgeWindowEnd = beatTime + window;
            //Debug.Log($"[큰 박자] 비트: {beatTime:F4}, 간격: {beatInterval:F4}, 성공 구간: {judgeWindowStart:F4}~{judgeWindowEnd:F4}");

            _isJudging = true;
            GameManager.Instance.PlayerController.HasInputThisBeat = false; // 플레이어 입력 초기화
            _currentBeatTime = beatTime;

            // 입력이 없으면 박자 끝에서 Miss
            if (!GameManager.Instance.PlayerController.HasInputThisBeat && AudioSettings.dspTime >= _judgeWindowEnd)
            {
                //ShowJudge("Miss");
                Debug.Log("입력 없이 큰 박자 종료 - Miss");
                _isJudging = false;
            }
        }
    }
}