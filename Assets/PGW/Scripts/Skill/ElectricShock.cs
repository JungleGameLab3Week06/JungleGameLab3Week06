using UnityEngine;
using static Define;

public class ElectricShock : ISkill
{
    int _strongDamage = 2;

    public void Execute()
    {
        if (GameManager.Instance._currentEnemyList == null || GameManager.Instance._currentEnemyList.Count == 0)
        {
            Debug.Log("적 리스트가 비어 있습니다!");
            return;
        }

        PlayerSkill playerSkill = GameManager.Instance.PlayerController.GetComponent<PlayerSkill>();
        
        if (GameManager.Instance.isLightningStrong)
        {
            // isLightningStrong이 true면 모든 적에게 strongDamage를 줌
            foreach (Enemy enemy in GameManager.Instance._currentEnemyList)
            {
                if (enemy.gameObject.activeSelf)
                {
                    playerSkill.ExcuteEffect(ElementalEffect.ElectricShock, new Vector3(0, 0, 0));
                    enemy.TakeDamage(_strongDamage);
                }
            }
            Debug.Log($"모든 적에게 {_strongDamage} 데미지!");
        }

        foreach (Enemy enemy in GameManager.Instance._currentEnemyList)
        {
            if (enemy.gameObject.activeSelf)
            {  
                enemy.ApplyState(Define.EnemyState.Shock);
            }
        }
        Debug.Log($"모든 적 스턴!");
    }
}