using UnityEngine;
using static Define;
using System.Collections.Generic;

public class Water : Skill
{
    int _baseDamage = 1;

    void Start()
    {
        Init();
    }

    public override void Execute()
    {
        if (GameManager.Instance.CurrentEnemyList == null || GameManager.Instance.CurrentEnemyList.Count == 0)
        {
            Debug.Log("적 리스트가 비어 있습니다!");
            return;
        }

        PlayerSkill playerSkill = PlayerController.Instance.PlayerSkill;

        // 맨 앞의 적에게 baseDamage를 줌
        List<Enemy> enemies = GameManager.Instance.GetFrontEnemies();
        WideAttack(enemies, ElementalEffect.Water, _baseDamage);
    }
}