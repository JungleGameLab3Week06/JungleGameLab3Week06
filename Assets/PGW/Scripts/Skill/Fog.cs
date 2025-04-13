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
        playerSkill.ExcuteEffect(ElementalEffect.Fog, Vector3.down * 5);
        playerSkill.IsLightningStrong = true;
    }

    void OnDestroy()
    {
        playerSkill.IsLightningStrong = false;
    }
}