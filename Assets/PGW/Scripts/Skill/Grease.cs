using UnityEngine;
using static Define;

public class Grease : ISkill
{
    public void Execute()
    {
        PlayerSkill playerSkill = GameManager.Instance.PlayerController.GetComponent<PlayerSkill>();
        playerSkill.ExcuteEffect(ElementalEffect.Grease, new Vector3(0, 0, 0));
        GameManager.Instance.isFireStrong = true;
    }
}