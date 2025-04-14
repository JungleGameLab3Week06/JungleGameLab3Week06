using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Diffusion : Skill
{
    int _baseDamage = 1;
    int _strongFireDamage = 2;
    int _strongLightningDamage = 4;

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

        if(playerSkill.IsFireStrong)
        {
            WideAttack(enemies, ElementalEffect.StrongFire, _strongFireDamage, false);
            Debug.Log($"모든 적에게 {_strongFireDamage} 데미지!");
            playerSkill.DestroyGrease();

        }

        WideAttack(enemies, ElementalEffect.Diffusion, _baseDamage, true);
        Debug.Log($"모든 적에게 {_baseDamage} 데미지!");

        if (playerSkill.IsLightningStrong)
        {
            enemies = GameManager.Instance.GetFrontEnemies();
            WideAttack(enemies, ElementalEffect.StrongLightning, _strongLightningDamage, true);
            playerSkill.DestroyFog();
        }
    }
}