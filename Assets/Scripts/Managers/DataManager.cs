using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

// 데이터 관리
// 스킬, 적, 웨이브 등 정보
public class DataManager
{
    // 적 Prefab 딕셔너리
    Dictionary<EnemyType, List<Enemy>> _enemyPrefabDict = new Dictionary<EnemyType, List<Enemy>>();

    [Header("Wave")]
    int _maxWave = 3;
    public Dictionary<int, List<(Enemy enemy, float weight)>> WaveInfoDict => _waveInfoDict;
    Dictionary<int, List<(Enemy enemy, float weight)>> _waveInfoDict = new Dictionary<int, List<(Enemy enemy, float weight)>>(); // key: 적 Prefab, value: 등장 가중치

    float[][] _waveWeights =
    {
        // [0]: Normal, [1]: Special, [2]: Confuse
        new float[] { 1f, 0f, 0f },         // 웨이브 1: Normal 100%, Special 0%, Confuse 0%
        new float[] { 0.7f, 0f, 0.3f },     // 웨이브 2: Normal 70%, Special 0%, Confuse 30%
        new float[] { 0.3f, 0.4f, 0.3f }    // 웨이브 3: Normal 30%, Special 40%, Confuse 30%
    };

    public void Init()
    {
        // 적 Prefab 로드 및 웨이브 정보 설정
        _enemyPrefabDict.Add(EnemyType.Normal, Resources.LoadAll<Enemy>("Prefabs/Enemies/Normal").ToList());
        _enemyPrefabDict.Add(EnemyType.Special, Resources.LoadAll<Enemy>("Prefabs/Enemies/Special").ToList());
        _enemyPrefabDict.Add(EnemyType.Confuse, Resources.LoadAll<Enemy>("Prefabs/Enemies/Confuse").ToList());
        SetWaveInfo();

        //Debug.Log($"몬스터 프리팹 로드 완료 {_enemyPrefabDict[EnemyType.Normal].Count} {_enemyPrefabDict[EnemyType.Special].Count} {_enemyPrefabDict[EnemyType.Confuse].Count}");
    }

    public void SetWaveInfo()
    {
        for(int i=0; i< _maxWave; i++)
        {
            _waveInfoDict[i] = new List<(Enemy, float)>();
            float normalWeight = _enemyPrefabDict[EnemyType.Normal].Count > 0 ? _waveWeights[i][0] / _enemyPrefabDict[EnemyType.Normal].Count : 0f;
            float specialWeight = _enemyPrefabDict[EnemyType.Special].Count > 0 ? _waveWeights[i][1] / _enemyPrefabDict[EnemyType.Special].Count : 0f;
            float confuseWeight = _enemyPrefabDict[EnemyType.Confuse].Count > 0 ? _waveWeights[i][2] / _enemyPrefabDict[EnemyType.Confuse].Count : 0f;
            _enemyPrefabDict[EnemyType.Normal].ForEach(enemy => _waveInfoDict[i].Add((enemy, normalWeight)));
            _enemyPrefabDict[EnemyType.Special].ForEach(enemy => _waveInfoDict[i].Add((enemy, specialWeight)));
            _enemyPrefabDict[EnemyType.Confuse].ForEach(enemy => _waveInfoDict[i].Add((enemy, confuseWeight)));
            Debug.Log($"웨이브 {i + 1} 설정: {_waveInfoDict[i].Count} 항목");
        }
    }
}