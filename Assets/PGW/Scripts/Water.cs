public class Water : ISkill
{
    int _baseDamage = 1;

    public void Execute()
    {
        if (GameManager.Instance.enemies.Length == 0) return;

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