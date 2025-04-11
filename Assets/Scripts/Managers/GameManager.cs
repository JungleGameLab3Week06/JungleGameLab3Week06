using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Define;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("적")]
    [SerializeField] Enemy _enemyPrefab;       // 적 Prefab
    [SerializeField] Transform _spawnPoint;    // 적 소환 지점
    
    [Header("판정")]
    public bool IsJudging => _isJudging;                 // 현재 판정 중인지 여부
    public double JudgeWindowStart => _judgeWindowStart; // 판정 시작 지점
    public double JudgeWindowEnd => _judgeWindowEnd;     // 판정 종료 지점
    bool _isJudging = false;                             // 판정 중인지 여부
    double _judgeWindowStart;                            // 판정 시작 지점
    double _judgeWindowEnd;                              // 판정 종료 지점
    [SerializeField] TextMeshProUGUI _judgeText;         // 판정 텍스트
    float judgeDisplayTime = 0.5f;                       // 판정 텍스트 표시 시간
    Coroutine judgeCoroutine;
    double _currentBeatTime = -1;   // 현재 처리 중인 비트

    [Header("플레이어 및 동료")]
    PlayerController _playerController;
    public PlayerController PlayerController => _playerController;
    Friend _friend;
    public Friend Friend => _friend;

    Elemental _friendElemental;
    Dictionary<(Elemental, Elemental), ElementalEffect> tagInteractions = new Dictionary<(Elemental, Elemental), ElementalEffect>
    {
        { (Elemental.Oil, Elemental.Flame), ElementalEffect.Ignite },
        // { ("화염", "기름"), "점화" }, // 반대 순서 추가
        // { ("냉기", "번개"), "감전" },
        { (Elemental.Lightning, Elemental.Frost), ElementalEffect.Shock } // 반대 순서 추가
    };

    Enemy _currentEnemy; // 추후에 리스트로 바꿔서 관리하기
    public Enemy CurrentEnemy => _currentEnemy;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _playerController = FindAnyObjectByType<PlayerController>();
        _friend = FindAnyObjectByType<Friend>();
    }

    void Start()
    {
        // 첫 적은 바로 스폰하지 않고 비트에서 처리
        RhythmManager.Instance.beatAction += HandleBeat;
    }

    // RhythomManager가 비트에 맞게 호출하는 함수
    void HandleBeat()
    {
        if (_currentEnemy == null)
        {
            SpawnEnemy();
        }
        else
        {
            _currentEnemy.Move();

            double beatTime = RhythmManager.Instance.LastBeatTime;
            if (beatTime == _currentBeatTime) return; // 동일 박자 중복 처리 방지

            float beatInterval = RhythmManager.Instance.BeatInterval; // 큰 박자 간격 (BPM 기반)
            float window = beatInterval * 0.3f; // 성공 구간 (±0.3초, BPM 60 기준 0.6초)
            _judgeWindowStart = beatTime - window;
            _judgeWindowEnd = beatTime + window;
            //Debug.Log($"[큰 박자] 비트: {beatTime:F4}, 간격: {beatInterval:F4}, 성공 구간: {judgeWindowStart:F4}~{judgeWindowEnd:F4}");

            _isJudging = true;
            _playerController.HasInputThisBeat = false; // 플레이어 입력 초기화
            _currentBeatTime = beatTime;

            // 입력이 없으면 박자 끝에서 Miss
            if (!_playerController.HasInputThisBeat && AudioSettings.dspTime >= _judgeWindowEnd)
            {
                ShowJudge("Miss");
                Debug.Log("입력 없이 큰 박자 종료 - Miss");
                _isJudging = false;
            }
        }
    }

    // 적 소환
    void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity);

        _friendElemental = (Elemental)Random.Range(0, System.Enum.GetValues(typeof(Elemental)).Length); // 나중에 뺄 예정
        //allyTag = tags[Random.Range(0, tags.Length)];
        //_currentEnemy.SetPreviewTag(_friendElemental, _weakSprites); // 이번 태그만 표시
        Debug.Log($"적 등장! 동료 예고: {_friendElemental}");
    }

    #region 마법 조합
    // 마법 조합 결과 반환
    public ElementalEffect GetInteraction(Elemental tag1, Elemental tag2)
    {
        if (tagInteractions.TryGetValue((tag1, tag2), out ElementalEffect interaction))
            return interaction;
        if (tagInteractions.TryGetValue((tag2, tag1), out interaction))
            return interaction;
        return ElementalEffect.None;
    }

    // 마법 조합 효과 적용
    public void ApplyInteraction(ElementalEffect interaction)
    {
        switch (interaction)
        {
            case ElementalEffect.Ignite:
                _currentEnemy.ApplyState((EnemyState)ElementalEffect.Ignite);
                _currentEnemy.TakeDamage(70);
                break;
            case ElementalEffect.Shock:
                _currentEnemy.ApplyState((EnemyState)ElementalEffect.Shock);
                _currentEnemy.TakeDamage(50);
                Debug.Log("감전감전50");
                break;
            default:
                break;
        }
    }
    #endregion

    #region 판정
    // 판정
    public string GetTimingJudgement(double timeDiff)
    {
        float beatInterval = RhythmManager.Instance.BeatInterval;
        float perfectWindow = beatInterval * 0.15f; // ±0.15초 (BPM 60 기준 0.3초)
        float goodWindow = beatInterval * 0.3f;     // ±0.3초 (BPM 60 기준 0.6초)

        if (timeDiff <= perfectWindow) return "Perfect";
        if (timeDiff <= goodWindow) return "Good";
        return "Miss";
    }

    // 판정 표시
    public void ShowJudge(string result)
    {
        if (judgeCoroutine != null)
            StopCoroutine(judgeCoroutine);
        judgeCoroutine = StartCoroutine(ShowJudgeCoroutine(result));
    }

    // 판정 표시 코루틴
    IEnumerator ShowJudgeCoroutine(string result)
    {
        _judgeText.text = result;
        _judgeText.gameObject.SetActive(true);

        // 색상 또는 효과 설정 (선택)
        switch (result)
        {
            case "Perfect":
                _judgeText.color = Color.yellow;
                break;
            case "Good":
                _judgeText.color = Color.green;
                break;
            case "Miss":
                _judgeText.color = Color.red;
                break;
        }

        yield return new WaitForSeconds(judgeDisplayTime);
        _judgeText.gameObject.SetActive(false);
    }
    #endregion
}