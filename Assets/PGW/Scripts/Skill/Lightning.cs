using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Lightning : Skill
{
    int _baseDamage = 3;
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
        List<Enemy> enemies = (playerSkill.IsLightningStrong) ? GameManager.Instance.CurrentEnemyList : GameManager.Instance.GetFrontEnemies();
        int damage = (playerSkill.IsLightningStrong) ? _strongDamage : _baseDamage;
        WideAttack(enemies, ElementalEffect.Lightning, damage);
    }
}