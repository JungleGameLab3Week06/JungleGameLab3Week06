using UnityEngine;

public class ElectricShock : ISkill
{
    public void Execute()
    {
        if (GameManager.Instance.enemyList == null || GameManager.Instance.enemyList.Count == 0)
        {
            Debug.Log("적 리스트가 비어 있습니다!");
            return;
        }

        foreach (Enemy enemy in GameManager.Instance.enemyList)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.ApplyState(Define.EnemyState.Shock);
            }
        }
        Debug.Log($"모든 적 스턴!");
    }
}