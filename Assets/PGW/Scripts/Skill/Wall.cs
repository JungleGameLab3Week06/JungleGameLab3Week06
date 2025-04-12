using UnityEngine;
using static Define;

public class Wall : ISkill
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Execute()
    {
        PlayerSkill playerSkill = GameManager.Instance.PlayerController.GetComponent<PlayerSkill>();
        playerSkill.ExcuteEffect(ElementalEffect.Wall, playerSkill.transform.position + new Vector3(2, 0, 0));
    }
}
