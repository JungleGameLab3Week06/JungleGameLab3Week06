using UnityEngine;
using System.Linq;
using static Define;

public class Lightning : ISkill
{
    int _baseDamage = 3;
    int _strongDamage = 2;

    public void Execute()
    {
        if (GameManager.Instance.enemyList == null || GameManager.Instance.enemyList.Count == 0)
        {
            Debug.Log("적 리스트가 비어 있습니다!");
            return;
        }

        PlayerSkill playerSkill = GameManager.Instance.PlayerController.GetComponent<PlayerSkill>();

        if (GameManager.Instance.isLightningStrong)
        {
            // isLightningStrong이 true면 모든 적에게 strongDamage를 줌
            foreach (Enemy enemy in GameManager.Instance.enemyList)
            {
                if (enemy.gameObject.activeSelf)
                {
                    playerSkill.ExcuteEffect(ElementalEffect.Lightning, enemy.transform.position);
                    enemy.TakeDamage(_strongDamage);
                }
            }
            Debug.Log($"모든 적에게 {_strongDamage} 데미지!");
        }
        else
        {
            // isLightningStrong이 false면 맨 앞의 적에게 baseDamage를 줌
            Enemy target = GameManager.Instance.enemyList
                .Where(e => e.gameObject.activeSelf)
                .OrderBy(e => e.transform.position.x)
                .FirstOrDefault();

            if (target != null)
            {
                playerSkill.ExcuteEffect(ElementalEffect.Lightning, target.transform.position);
                target.TakeDamage(_baseDamage);
                Debug.Log($"맨 앞의 적에게 {_baseDamage} 데미지!");
            }
            else
            {
                Debug.Log("활성화된 적이 없습니다!");
            }
        }
    }

}