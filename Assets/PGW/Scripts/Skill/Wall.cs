using UnityEngine;

public class Wall : ISkill
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Execute()
    {
        // 벽 스킬을 사용하여 적에게 피해를 입히는 로직을 여기에 작성합니다.
        // 예를 들어, 적의 HP를 감소시키거나 상태를 변경하는 등의 작업을 수행할 수 있습니다.
        Debug.Log("Wall skill executed!");
    }
}
