using System.Collections.Generic;
using UnityEngine;
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

    [Header("플레이어 및 동료")]
    public PlayerController PlayerController => _playerController;
    public Friend Friend => _friend;
    PlayerController _playerController;
    Friend _friend;

    Elemental _friendElemental;
    Dictionary<(Elemental, Elemental), ElementalEffect> tagInteractions = new Dictionary<(Elemental, Elemental), ElementalEffect>
    {
        { (Elemental.Ground, Elemental.Flame), ElementalEffect.Ignition },
        // { ("화염", "기름"), "점화" }, // 반대 순서 추가
        // { ("냉기", "번개"), "감전" },
        { (Elemental.Lightning, Elemental.Water), ElementalEffect. ElectricShock } // 반대 순서 추가
    };

    Enemy _currentEnemy; // 추후에 리스트로 바꿔서 관리하기
    public Enemy CurrentEnemy => _currentEnemy;

    // (테스트용)
    public List<Enemy> enemyList = new List<Enemy>(); // 적 리스트 
    public bool isFireStrong = false;
    public bool isLightningStrong = false;

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

    // 적 소환
    public void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity);
        enemyList.Add(_currentEnemy);
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
            case ElementalEffect.Ignition:
                _currentEnemy.ApplyState((EnemyState)ElementalEffect.Ignition);
                _currentEnemy.TakeDamage(70);
                break;
            case ElementalEffect. ElectricShock:
                _currentEnemy.ApplyState((EnemyState)ElementalEffect. ElectricShock);
                _currentEnemy.TakeDamage(50);
                Debug.Log("감전감전50");
                break;
            default:
                break;
        }
    }
    #endregion
}