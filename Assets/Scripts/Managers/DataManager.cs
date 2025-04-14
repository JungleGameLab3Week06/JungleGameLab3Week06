using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;
using static DataManager;


// 데이터 관리
// 스킬, 적, 웨이브 등 정보
public class DataManager
{
    // 적 Prefab 딕셔너리
    Dictionary<EnemyType, List<Enemy>> _enemyPrefabDict = new Dictionary<EnemyType, List<Enemy>>();

    [Header("Wave")]
    int _maxWave = 3;
    public Dictionary<int, List<(Enemy enemy, float weight)>> WaveInfoDict => _waveInfoDict;
    Dictionary<int, List<(Enemy enemy, float weight)>> _waveInfoDict = new Dictionary<int, List<(Enemy enemy, float weight)>>(); // key: 웨이브 번호, value: List<(적 Prefab, 가중치)>

    float[][] _waveWeights =
    {
        // [0]: Normal, [1]: Special, [2]: Confuse
        new float[] { 1f, 0f, 0f },         // 웨이브 1: Normal 100%, Special 0%, Confuse 0%
        new float[] { 0.7f, 0f, 0.3f },     // 웨이브 2: Normal 70%, Special 0%, Confuse 30%
        new float[] { 0.3f, 0.4f, 0.3f }    // 웨이브 3: Normal 30%, Special 40%, Confuse 30%
    };

    [Header("스킬")]
    public Dictionary<ElementalEffect, Skill> SkillDict => _skillDict;
    Dictionary<ElementalEffect, Skill> _skillDict = new Dictionary<ElementalEffect, Skill>();   // 스킬 딕셔너리

    Dictionary<string, SkillInfo> _skillInfoDict = new Dictionary<string, SkillInfo>();
    public Dictionary<string, SkillInfo> SkillInfoDict => _skillInfoDict;


    public void Init()
    {
        // 적 Prefab 로드 및 웨이브 정보 설정
        _enemyPrefabDict.Add(EnemyType.Normal, Resources.LoadAll<Enemy>("Prefabs/Enemies/Normal").ToList());
        _enemyPrefabDict.Add(EnemyType.Special, Resources.LoadAll<Enemy>("Prefabs/Enemies/Special").ToList());
        _enemyPrefabDict.Add(EnemyType.Confuse, Resources.LoadAll<Enemy>("Prefabs/Enemies/Confuse").ToList());

        SetWaveInfo();
        //Debug.Log($"몬스터 프리팹 로드 완료 {_enemyPrefabDict[EnemyType.Normal].Count} {_enemyPrefabDict[EnemyType.Special].Count} {_enemyPrefabDict[EnemyType.Confuse].Count}");

        // 스킬 Prefab 로드
        GameObject[] skillEffectPrefabs = Manager.Resource.LoadAll<GameObject>($"Prefabs/Skills");
        foreach (GameObject skillEffectPrefab in skillEffectPrefabs)
        {
            string skillEffectPrefabName = skillEffectPrefab.name;
            if (Enum.TryParse(skillEffectPrefabName, out ElementalEffect elementalEffectType))
            {
                _skillDict.Add(elementalEffectType, skillEffectPrefab.GetComponent<Skill>());
            }
        }

        // 스킬 정보 로드
        LoadSkillInfo();
    }

    // 웨이브 정보 설정
    public void SetWaveInfo()
    {
        for(int i=0; i< _maxWave; i++)
        {
            _waveInfoDict[i] = new List<(Enemy, float)>();
            float normalWeight = _enemyPrefabDict[EnemyType.Normal].Count > 0 ? _waveWeights[i][0] / _enemyPrefabDict[EnemyType.Normal].Count : 0f;
            float specialWeight = _enemyPrefabDict[EnemyType.Special].Count > 0 ? _waveWeights[i][1] / _enemyPrefabDict[EnemyType.Special].Count : 0f;
            float confuseWeight = _enemyPrefabDict[EnemyType.Confuse].Count > 0 ? _waveWeights[i][2] / _enemyPrefabDict[EnemyType.Confuse].Count : 0f;
            _enemyPrefabDict[EnemyType.Normal].ForEach(enemyPrefab => _waveInfoDict[i].Add((enemyPrefab, normalWeight)));
            _enemyPrefabDict[EnemyType.Special].ForEach(enemyPrefab => _waveInfoDict[i].Add((enemyPrefab, specialWeight)));
            _enemyPrefabDict[EnemyType.Confuse].ForEach(enemyPrefab => _waveInfoDict[i].Add((enemyPrefab, confuseWeight)));
            Debug.Log($"웨이브 {i + 1} 설정: {_waveInfoDict[i].Count} 항목");
        }
    }

    public void LoadSkillInfo()
    {
        TextAsset json = Manager.Resource.Load<TextAsset>("JSON/SkillInfo");

        //if (json == null)
        //    Debug.LogError("못가져옴~");
        //else
        //    Debug.LogError(json.ToString());

        var test = JsonUtility.FromJson<SkillInfoList>(json.text);

        //Debug.LogError(test.skillInfoList.Count);

        for(int i=0; i<test.skillInfoList.Count; i++)
        {
            var info = test.skillInfoList[i];
            _skillInfoDict[info.SkillName] = info;
            //Debug.LogError($"{test.skillInfoList[i].SkillName} {test.skillInfoList[i].SkillDescription}");
        }
    }
    [Serializable]
    public class SkillInfo
    {
        public string SkillName;
        public string SkillDescription;
    }

    [Serializable]
    public class SkillInfoList
    {
        public List<SkillInfo> skillInfoList = new List<SkillInfo>();
    }

}