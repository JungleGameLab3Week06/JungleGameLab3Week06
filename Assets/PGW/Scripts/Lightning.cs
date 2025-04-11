using UnityEngine;

public class Lightning : ISkill
{
    int _baseDamage = 3;
    int _strongDamage = 2;

    public void Execute()
    {
        if (GameManager.Instance.enemies.Length == 0) return;

        if (GameManager.Instance.isLightningStrong)
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

        // isStrong이 false면 맨 앞의 적에게 baseDamage를 줌
        Enemy target = GameManager.Instance.enemies
            .Where(e => e.gameObject.activeSelf)
            .OrderBy(e => e.transform.position.x)
            .FirstOrDefault();

        if (target != null)
        {
            target.TakeDamage(_baseDamage);
            Debug.Log($"맨 앞의 적에게 {_baseDamage} 데미지!");
        }
    }
}