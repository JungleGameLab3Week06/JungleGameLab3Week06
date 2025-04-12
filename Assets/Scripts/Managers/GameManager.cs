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
    [SerializeField] PlayerController _playerController;
    [SerializeField] Friend _friend;

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

        if (enemyList.Count > 0)
            _friend.PrepareElemental(); // 동료 마법 준비
    }
}