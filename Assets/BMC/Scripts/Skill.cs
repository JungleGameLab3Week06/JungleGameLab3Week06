using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill : MonoBehaviour
{
    protected PlayerSkill playerSkill;

    public void Init()
    {
        playerSkill = PlayerController.Instance.PlayerSkill;
    }

    public virtual void Execute()
    {
        // 기본 스킬 실행 로직
        Debug.Log("기본 스킬 실행");
    }

    /// <summary>
    /// 광역 공격
    /// </summary>
    /// <param name="enemyList"> 적용할 적 리스트 </param>
    /// <param name="elementalEffect"> 조합된 마법 효과 </param>
    /// <param name="damage"> 데미지 </param>
    /// <param name="isSpawnTarget"> 적 자리에 효과 발생 여부 </param>
    public void WideAttack(List<Enemy> enemyList, ElementalEffect elementalEffect, int damage, bool isSpawnTarget = true)
    {
        if (enemyList == null)
            Debug.LogError("적 리스트가 null~");

        for (int i = 0; i < enemyList.Count; i++)
        {
            if(enemyList[i] == null)
                continue;

            if (enemyList[i] != null)
            {
                Vector3 position = (isSpawnTarget) ? enemyList[i].transform.position : Vector3.zero;

                if (playerSkill == null)
                {
                    playerSkill = PlayerController.Instance.PlayerSkill;
                }

                playerSkill.ExcuteEffect(elementalEffect, position);
                enemyList[i].TakeDamage(damage);
            }
        }
        Debug.Log($"모든 적에게 {damage} 데미지!");
    }
}