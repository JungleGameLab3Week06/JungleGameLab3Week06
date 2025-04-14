using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Fire : Skill
{
    int _baseDamage = 1;
    int _strongDamage = 2;

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

        List<Enemy> enemies = GameManager.Instance.CurrentEnemyList;
        if (playerSkill.IsFireStrong)
        {
            WideAttack(enemies, ElementalEffect.StrongFire, _strongDamage, false);
            playerSkill.DestroyGrease();
            Debug.Log($"모든 적에게 {_strongDamage} 데미지!");
        }

        enemies = GameManager.Instance.GetFrontEnemies();
        WideAttack(enemies, ElementalEffect.Flame, _baseDamage);
    }
}