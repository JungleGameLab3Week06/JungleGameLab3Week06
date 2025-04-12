using UnityEngine;
using static Define;

public class Diffusion : ISkill
{
    int _baseDamage = 1;
    int _strongDamage = 2;

    public void Execute()
    {
        if (GameManager.Instance.enemyList == null || GameManager.Instance.enemyList.Count == 0)
        {
            Debug.Log("적 리스트가 비어 있습니다!");
            return;
        }

        PlayerSkill playerSkill = GameManager.Instance.PlayerController.GetComponent<PlayerSkill>();
        if (GameManager.Instance.isFireStrong || GameManager.Instance.isLightningStrong)
        {
            // isStrong이 true면 모든 적에게 strongDamage를 줌
            foreach (Enemy enemy in GameManager.Instance.enemyList)
            {
                if (enemy.gameObject.activeSelf)
                {
                    playerSkill.ExcuteEffect(ElementalEffect.Diffusion, new Vector3(0, 0, 0));
                    enemy.TakeDamage(_strongDamage);
                }
            }
            Debug.Log($"모든 적에게 {_strongDamage} 데미지!");
        }

        foreach (Enemy enemy in GameManager.Instance.enemyList)
        {
            if (enemy.gameObject.activeSelf)
            {
                playerSkill.ExcuteEffect(ElementalEffect.Diffusion, new Vector3(0, 0, 0));
                enemy.TakeDamage(_baseDamage);
            }
        }
        Debug.Log($"모든 적에게 {_baseDamage} 데미지!");
    }
}