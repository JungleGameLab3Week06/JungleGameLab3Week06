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
        PlayerController.Instance.PlayerSkill.ExcuteEffect(ElementalEffect.Grease, Vector3.down * 5);
        PlayerController.Instance.PlayerSkill.IsFireStrong = true;
    }

    void OnDestroy()
    {
        PlayerController.Instance.PlayerSkill.IsFireStrong = false;
    }
}