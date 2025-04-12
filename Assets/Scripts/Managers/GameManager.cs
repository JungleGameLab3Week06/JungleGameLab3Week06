using System.Collections.Generic;
using UnityEngine;
using static Define;
using System.Linq;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("적")]
    [SerializeField] List<Enemy> _normalEnemyPrefab;       // 적 Prefab
    [SerializeField] List<Enemy> _specialEnemyPrefab;
    [SerializeField] List<Enemy> _confuseEnemyPrefab;
    List<(Enemy prefab, float weight)> _weightedEnemies;
    [SerializeField] Transform _spawnPoint;    // 적 소환 지점

    // 웨이브 관리
    int _currentWave = 0;
    int _waveMonsterCount = 0;
    readonly Dictionary<int, List<(Enemy prefab, float weight)>> _waveConfigurations = new Dictionary<int, List<(Enemy prefab, float weight)>>();

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

    void Start()
    {
        _normalEnemyPrefab = Resources.LoadAll<Enemy>("Prefabs/Enemies/None").ToList();
        _specialEnemyPrefab = Resources.LoadAll<Enemy>("Prefabs/Enemies/Special").ToList();
        _confuseEnemyPrefab = Resources.LoadAll<Enemy>("Prefabs/Enemies/Confuse").ToList();

        InitializeWaveConfigurations();

        StartWave(1);
        Debug.Log($"웨이브 {_currentWave} 시작! 가중치: Normal={_weightedEnemies.First(e => _normalEnemyPrefab.Contains(e.prefab)).weight}, Special={_weightedEnemies.First(e => _specialEnemyPrefab.Contains(e.prefab)).weight}, Confuse={_weightedEnemies.First(e => _confuseEnemyPrefab.Contains(e.prefab)).weight}");
    }

    void InitializeWaveConfigurations()
    {
        // 웨이브 1: Normal 100%, Special 0%, Confuse 0%
        _waveConfigurations[1] = new List<(Enemy, float)>();
        float normalWeight1 = _normalEnemyPrefab.Count > 0 ? 1f / _normalEnemyPrefab.Count : 0f;
        float specialWeight1 = 0f;
        float confuseWeight1 = 0f;
        _normalEnemyPrefab.ForEach(e => _waveConfigurations[1].Add((e, normalWeight1)));
        _specialEnemyPrefab.ForEach(e => _waveConfigurations[1].Add((e, specialWeight1)));
        _confuseEnemyPrefab.ForEach(e => _waveConfigurations[1].Add((e, confuseWeight1)));
        Debug.Log($"웨이브 1 설정: {_waveConfigurations[1].Count} 항목");

        // 웨이브 2: Normal 70%, Special 0%, Confuse 30%
        _waveConfigurations[2] = new List<(Enemy, float)>();
        float normalWeight2 = _normalEnemyPrefab.Count > 0 ? 0.7f / _normalEnemyPrefab.Count : 0f;
        float specialWeight2 = 0f;
        float confuseWeight2 = _confuseEnemyPrefab.Count > 0 ? 0.3f / _confuseEnemyPrefab.Count : 0f;
        _normalEnemyPrefab.ForEach(e => _waveConfigurations[2].Add((e, normalWeight2)));
        _specialEnemyPrefab.ForEach(e => _waveConfigurations[2].Add((e, specialWeight2)));
        _confuseEnemyPrefab.ForEach(e => _waveConfigurations[2].Add((e, confuseWeight2)));
        Debug.Log($"웨이브 2 설정: {_waveConfigurations[2].Count} 항목");

        // 웨이브 3: Normal 30%, Special 40%, Confuse 30%
        _waveConfigurations[3] = new List<(Enemy, float)>();
        float normalWeight3 = _normalEnemyPrefab.Count > 0 ? 0.3f / _normalEnemyPrefab.Count : 0f;
        float specialWeight3 = _specialEnemyPrefab.Count > 0 ? 0.4f / _specialEnemyPrefab.Count : 0f;
        float confuseWeight3 = _confuseEnemyPrefab.Count > 0 ? 0.3f / _confuseEnemyPrefab.Count : 0f;
        _normalEnemyPrefab.ForEach(e => _waveConfigurations[3].Add((e, normalWeight3)));
        _specialEnemyPrefab.ForEach(e => _waveConfigurations[3].Add((e, specialWeight3)));
        _confuseEnemyPrefab.ForEach(e => _waveConfigurations[3].Add((e, confuseWeight3)));
        Debug.Log($"웨이브 3 설정: {_waveConfigurations[3].Count} 항목");
    }

    public void StartWave(int waveNumber)
    {
        _currentWave = waveNumber;
        if (_waveConfigurations.TryGetValue(waveNumber, out List<(Enemy prefab, float weight)> waveConfig))
        {
            _weightedEnemies = waveConfig;
            _waveMonsterCount = waveConfig.Count;
            Debug.Log($"웨이브 {_currentWave} 시작! 가중치: Normal={waveConfig.First(e => _normalEnemyPrefab.Contains(e.prefab)).weight}, Special={waveConfig.First(e => _specialEnemyPrefab.Contains(e.prefab)).weight}, Confuse={waveConfig.First(e => _confuseEnemyPrefab.Contains(e.prefab)).weight}");
        }
        else
        {
            Debug.LogError($"웨이브 {waveNumber} 설정이 없습니다!");
            return;
        }
    }

    public void SpawnEnemy()
    {
        if(!CheckSpawnCondition()) return;

        Enemy enemy = GetRandomEnemy();
        _currentEnemy = Instantiate(enemy, _spawnPoint.position, Quaternion.identity);
        enemyList.Add(_currentEnemy);
        _waveMonsterCount--;

        if (enemyList.Count > 0)
            _friend.PrepareElemental(); // 동료 마법 준비
    }

    Enemy GetRandomEnemy()
    {
        float totalWeight = _weightedEnemies.Sum(e => e.weight);
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var enemy in _weightedEnemies)
        {
            currentWeight += enemy.weight;
            if (randomValue <= currentWeight)
                return enemy.prefab;
        }
        return _weightedEnemies.Last().prefab;
    }

    bool CheckSpawnCondition()
    {
        if (_weightedEnemies == null || _weightedEnemies.Count == 0)
        {
            Debug.LogWarning("WeightedEnemies가 초기화되지 않았거나 비어 있습니다!");
            return false;
        }

        if (_waveMonsterCount <= 0)
        {
            if(enemyList.Count == 0)
            {
                StartWave(_currentWave + 1); // 다음 웨이브 시작
            }
            return false;
        }
        return true;
    }

    public List<Enemy> GetFrontEnemies()
    {
        if (enemyList.Count == 0) return new List<Enemy>();

        float minX = enemyList.Min(e => e.transform.position.x);
        return enemyList.Where(e => Mathf.Approximately(e.transform.position.x, minX)).ToList();
    }

    public bool CheckSpawnPoint()
    {
        if (_spawnPoint == null || enemyList.Count == 0)
            return true;

        float spawnX = _spawnPoint.position.x;
        int count = enemyList.Count(e => Mathf.Approximately(e.transform.position.x, spawnX));
        return count < 2;
    }
}
