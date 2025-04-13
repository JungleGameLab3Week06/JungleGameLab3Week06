using UnityEngine;
using static Define;

public class Wall : Skill
{
    void Start()
    {
        Init();
    }

    public override void Execute()
    {
        PlayerSkill playerSkill = PlayerController.Instance.PlayerSkill;
        playerSkill.ExcuteEffect(ElementalEffect.Wall, playerSkill.transform.position + new Vector3(2, 0, 0));
    }
}
