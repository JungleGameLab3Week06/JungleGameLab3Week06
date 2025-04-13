using UnityEngine;
using static Define;

public class Grease : Skill
{
    void Start()
    {
        Init();
    }

    public override void Execute()
    {
        PlayerController.Instance.PlayerSkill.ExcuteEffect(ElementalEffect.Grease, new Vector3(0, 0, 0));
        PlayerController.Instance.PlayerSkill.IsFireStrong = true;
    }
}