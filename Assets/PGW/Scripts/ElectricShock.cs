using UnityEngine;

public class ElectricShock : ISkill
{
    public void Execute()
    {
        if (GameManager.Instance.enemies.Length == 0) return;

        foreach (Enemy enemy in GameManager.Instance.enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.SetSturnState();
            }
        }
        Debug.Log($"모든 적 스턴!");
    }
}