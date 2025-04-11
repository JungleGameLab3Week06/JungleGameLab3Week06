using UnityEngine;

public class Diffusion : ISkill
{
    int _baseDamage = 1;
    int _strongDamage = 2;

    public void Execute()
    {
        if (GameManager.Instance.enemies.Length == 0) return;

        if (GameManager.Instance.isFireStrong || GameManager.Instance.isLightningStrong)
        {
            // isStrong이 true면 모든 적에게 strongDamage를 줌
            foreach (Enemy enemy in GameManager.Instance.enemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    enemy.TakeDamage(_strongDamage);
                }
            }
            Debug.Log($"모든 적에게 {_strongDamage} 데미지!");
        }

        // isStrong이 true면 모든 적에게 strongDamage를 줌
        foreach (Enemy enemy in GameManager.Instance.enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.TakeDamage(_baseDamage);
            }
        }
        Debug.Log($"모든 적에게 {_baseDamage} 데미지!");
    }
}