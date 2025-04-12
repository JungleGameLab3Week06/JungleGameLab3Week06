using UnityEngine;
using static Define;

public class Fog : ISkill
{
    public void Execute()
    {
        PlayerSkill playerSkill = GameManager.Instance.PlayerController.GetComponent<PlayerSkill>();
        playerSkill.ExcuteEffect(ElementalEffect.Fog, new Vector3(0, 0, 0));
        GameManager.Instance.isLightningStrong = true;
    }
}