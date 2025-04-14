using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ElectricShock : Skill
{
    int _strongDamage = 4;

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
        List<Enemy> enemies = GameManager.Instance.GetFrontEnemies();
        if (playerSkill.IsLightningStrong)
        {
            WideAttack(enemies, ElementalEffect.StrongLightning, _strongDamage, true);
            playerSkill.DestroyFog();
        }

        enemies = GameManager.Instance.CurrentEnemyList;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.ApplyState(EnemyState.Shock);
                PlayerController.Instance.PlayerSkill.ExcuteEffect(ElementalEffect.EnemyShock, enemy.transform.position);
            }
        }
        Debug.Log($"모든 적 스턴!");
    }
}