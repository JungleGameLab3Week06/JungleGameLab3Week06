using UnityEngine;
using static Define;

public class Fog : Skill
{
    void Start()
    {
        Init();
    }

    public override void Execute()
    {
        PlayerSkill playerSkill = PlayerController.Instance.PlayerSkill;
        playerSkill.ExcuteEffect(ElementalEffect.Fog, new Vector3(0, 0, 0));
        playerSkill.IsLightningStrong = true;
    }
}