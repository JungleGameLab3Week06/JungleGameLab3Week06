using UnityEngine;
using static Define;
using System.Collections.Generic;

public class Water : ISkill
{
    int _baseDamage = 1;

    public void Execute()
    {
        if (GameManager.Instance.enemyList == null || GameManager.Instance.enemyList.Count == 0)
        {
            Debug.Log("적 리스트가 비어 있습니다!");
            return;
        }

        PlayerSkill playerSkill = GameManager.Instance.PlayerController.GetComponent<PlayerSkill>();

        // 맨 앞의 적에게 baseDamage를 줌
        List<Enemy> target = GameManager.Instance.GetFrontEnemies();

        foreach (Enemy enemy in target)
        {
            if (enemy != null)
            {
                playerSkill.ExcuteEffect(ElementalEffect.Water, enemy.transform.position);
                enemy.TakeDamage(_baseDamage);
                Debug.Log($"맨 앞의 적에게 {_baseDamage} 데미지!");
            }
            else
            {
                Debug.Log("활성화된 적이 없습니다!");
            }
        }
    }
}