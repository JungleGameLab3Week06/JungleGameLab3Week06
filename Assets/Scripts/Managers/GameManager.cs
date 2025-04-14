using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static Define;

// 게임 전반적인 관리 담당
// 적 소환, 플레이어 및 동료 관리, 웨이브 관리 등
public class GameManager : MonoBehaviour
{
    [Header("싱글톤")]
    static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("적 및 웨이브")]
    int _currentWave = 0;                                                                 // 현재 웨이브
    int _waveMonsterCount = 0;                                                            // 웨이브 몬스터 수
    [SerializeField] List<(Enemy enemyPrefab, float weight)> _currentWaveSpawnInfoList;   // 현재 웨이브의 적 정보 리스트
    [SerializeField] Transform _enemySpawnPoint;                                          // 적 소환 지점
    int _noneMonsetCount = 0;                                                             // 공백 몬스터 수

    [Header("플레이어 및 동료")]
    public PlayerController PlayerController => _playerController;
    public Friend Friend => _friend;
    [SerializeField] PlayerController _playerController;
    [SerializeField] Friend _friend;

    Enemy _currentEnemy; // 추후에 리스트로 바꿔서 관리하기
    public Enemy CurrentEnemy => _currentEnemy;

    public List<Enemy> CurrentEnemyList => _currentEnemyList;
    List<Enemy> _currentEnemyList = new List<Enemy>();      // 현재 적 리스트 

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        _playerController = FindAnyObjectByType<PlayerController>();
        _friend = FindAnyObjectByType<Friend>();
        _enemySpawnPoint = FindAnyObjectByType<EnemySpawnPoint>().transform;
    }

    void Start()
    {
        _friend.PrepareElemental(); // 동료 마법 준비
        StartWave();
        //Debug.Log($"웨이브 {_currentWave} 시작! 가중치: Normal={_weightedEnemies.First(e => _normalEnemyPrefabList.Contains(e.prefab)).weight}, Special={_weightedEnemies.First(e => _specialEnemyPrefabList.Contains(e.prefab)).weight}, Confuse={_weightedEnemies.First(e => _confuseEnemyPrefabList.Contains(e.prefab)).weight}");
    }

    public void StartWave()
    {
        if (Manager.Data.WaveInfoDict.TryGetValue(_currentWave, out List<(Enemy enemy, float weight)> currentWaveInfoList))
        {
            _currentWaveSpawnInfoList = currentWaveInfoList;
            _waveMonsterCount = 30;
            _currentWave++;
            Manager.UI.updateWaveAction?.Invoke(_currentWave); // UI 업데이트

            RhythmManager.Instance.IncreaseBPM();
            if (_currentWave < 6)
            {
                if (_currentWave <= 3)
                {
                    _noneMonsetCount = 10;
                }
                else
                {
                    _noneMonsetCount = 5;
                }
            }
            else
            {
                _noneMonsetCount = 0;
            }
            //Debug.Log($"웨이브 {_currentWave} 시작! 가중치: Normal={currentWaveInfoList.First(e => _normalEnemyPrefabList.Contains(e.prefab)).weight}, Special={waveConfig.First(e => _specialEnemyPrefabList.Contains(e.prefab)).weight}, Confuse={waveConfig.First(e => _confuseEnemyPrefabList.Contains(e.prefab)).weight}");
        }
        else
        {
            Debug.LogError($"웨이브 {_currentWave} 설정이 없습니다!");
            return;
        }
    }

    // 적 소환
    public void SpawnEnemy(Vector3 spawnPoint)
    {
        if(!CheckSpawnCondition()) 
            return;
        if (_currentWave < 6)
        {
            if (_noneMonsetCount > 0)
            {
                if (Random.value > 0.3f)
                {
                    Enemy enemy = GetRandomEnemy();
                    _currentEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
                    _currentEnemyList.Add(_currentEnemy);
                    _waveMonsterCount--;
                    Debug.Log($"웨이브 {_currentWave} 몬스터 타입{enemy}");
                }
                else { _waveMonsterCount--; _noneMonsetCount--; Debug.LogError("웨이브브브브.None" + _waveMonsterCount); }
            }
            else
            {
                Enemy enemy = GetRandomEnemy();
                _currentEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
                _currentEnemyList.Add(_currentEnemy);
            }
        }
        else
        {
            Enemy enemy = GetRandomEnemy();
            _currentEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
            _currentEnemyList.Add(_currentEnemy);
        }
        _waveMonsterCount--;
        Debug.Log($"웨이브 {_currentWave} 몬스터 수: {_waveMonsterCount}");

    }

    Enemy GetRandomEnemy()
    {
        float totalWeight = _currentWaveSpawnInfoList.Sum(enemySpawnInfo => enemySpawnInfo.weight);
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var enemySpawnInfo in _currentWaveSpawnInfoList)
        {
            currentWeight += enemySpawnInfo.weight;
            if (randomValue <= currentWeight)
                return enemySpawnInfo.enemyPrefab;
        }
        return _currentWaveSpawnInfoList.Last().enemyPrefab;
    }

    bool CheckSpawnCondition()
    {

        if (_waveMonsterCount <= 0)
        {
            if(_currentEnemyList.Count == 0)
            {
                StartWave(); // 다음 웨이브 시작
                Debug.Log($"웨이브 {_currentWave} 시작!");
            }
            return false;
        }
        return true;
    }

    // 적 중 가장 앞에 있는 적 반환 (여러 개면 전부 반환)
    public List<Enemy> GetFrontEnemies()
    {
        float minX = float.MaxValue;
        for(int i=0; i<_currentEnemyList.Count; i++)
        {
            if (_currentEnemyList[i] == null)
                continue;
            if (_currentEnemyList[i].transform.position.x < minX)
                minX = _currentEnemyList[i].transform.position.x;
        }

        return _currentEnemyList.Where(enemy => Mathf.Approximately(enemy.transform.position.x, minX)).ToList();
    }

    // 소환 지점 검사
    public bool CheckSpawnPoint(out Vector3 spawnPoint)
    {
        spawnPoint = _enemySpawnPoint.position;
        if (_enemySpawnPoint == null || _currentEnemyList.Count == 0)
            return true;

        float spawnX = _enemySpawnPoint.position.x;
        List<Enemy> spawnPointEnemies = new List<Enemy>();
        foreach (Enemy enemy in _currentEnemyList)
        {
            if (Mathf.Approximately(enemy.transform.position.x, spawnX))
                spawnPointEnemies.Add(enemy);
        }
        if(spawnPointEnemies.Count == 0)
            return true;
        else if (spawnPointEnemies.Count == 1)
        {
            if(spawnPointEnemies[0].transform.position == _enemySpawnPoint.position)
                spawnPoint = _enemySpawnPoint.position + new Vector3(0, -4f, 0);
            return true;
        }
        return false;
    }
}