using System;
using UnityEngine;
using System.Collections.Generic;

public struct BeatJudgementWindow
{
    public double SuccessStart1;
    public double FailStart;
    public double SuccessStart2;
    public double End;
}

public class RhythmManager : MonoBehaviour
{
    [Header("싱글톤")]
    static RhythmManager _instance;
    public static RhythmManager Instance => _instance;

    [Header("비트")]
    AudioSource _beatSource;                                        // 비트 소스
    AudioClip _beatClip;                                            // 비트 클립

    double _prevBpm;
    [SerializeField] double _bpm = 60f;                             // 분당 비트 수

    public double BeatInterval => _beatInterval;
    public double LastBeatTime => _lastBeatTime; 
    double _beatInterval;                                           // 비트 간격 (초 단위)
    double _lastBeatTime;                                           // 마지막 비트 발생 시점
    double _nextBeatTime = -1;                                      // 다음 비트 발생 시점
    BeatJudgementWindow _beatWindow;                                // 비트 윈도우

    public Action colorFloorAction;                                 // 비트 발생 시 바닥들 색상 변경하는 것

    [Header("판정")]
    public bool IsJudging => _isJudging;
    [SerializeField] bool _isJudging = false;                       // 판정 중인지 여부
    [SerializeField] bool _isJudgeChangedToTrue = false;            // 판정이 바뀐 비트인지 여부
    [SerializeField] bool _isJudgeChangedToFalse = false;           // 판정이 바뀐 비트인지 여부
    double _successRatio = 0.25f;

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
            _lastBeatTime = _nextBeatTime; // ← 여기 주의! 실제 비트 시점은 nextBeatTime
            // 판정 윈도우 설정
            _beatWindow = CreateBeatWindow(_lastBeatTime);
            _nextBeatTime += _beatInterval;
            RegularBeat();
        }
        else if (!_isJudgeChangedToFalse && _isJudging && AudioSettings.dspTime >= _beatWindow.FailStart)
        {
            _isJudging = false; // 판정 종료
            _isJudgeChangedToFalse = true; // 판정이 바뀐 비트
            _isJudgeChangedToTrue = false; // 판정이 바뀐 비트
            GameManager.Instance.PlayerController.HasInputThisBeat = false; // 입력 초기화
            Debug.Log("판정 종료");
        }
        else if (!_isJudgeChangedToTrue && !_isJudging && AudioSettings.dspTime >= _beatWindow.SuccessStart2)
        {
            _isJudging = true; // 판정 시작
            _isJudgeChangedToTrue = true; // 판정이 바뀐 비트
            Debug.Log("판정 시작");
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
        _nextBeatTime = (double)AudioSettings.dspTime + _beatInterval;
        _prevBpm = _bpm;
    }

    // 정박자
    void RegularBeat()
    {
        _beatSource.PlayOneShot(_beatClip);         // 비트 소리 재생
        colorFloorAction?.Invoke();                 // 바닥 색상 변경

        /* 추후에 Action으로 빼기 */
        List<Enemy> enemies = GameManager.Instance._currentEnemyList; // 적 리스트
        foreach (Enemy enemy in enemies)
        {
            enemy.Move();
        }

        if (GameManager.Instance.CheckSpawnPoint()) // 소환 포인트 체크
        {
            GameManager.Instance.SpawnEnemy(); // 적 소환
            Debug.LogWarning("적 소환");
        }
        else
        {
            Debug.LogWarning("소환 포인트 없음");
        }
        _isJudgeChangedToFalse = false; // 판정이 바뀐 비트 초기화
    }

    BeatJudgementWindow CreateBeatWindow(double beatTime)
    {
        double interval = _beatInterval;

        // 성공: 25%, 실패: 50%, 성공: 25%
        double successLen = interval * _successRatio;
        double failLen = interval * (1 - _successRatio * 2);

        return new BeatJudgementWindow()
        {
            SuccessStart1 = beatTime,
            FailStart = beatTime + successLen,
            SuccessStart2 = beatTime + successLen + failLen,
            End = beatTime + interval
        };
    }
}