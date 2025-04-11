public class Fog : ISkill
{
    public void Execute()
    {
        GameManager.Instance.isLightningStrong = true;
    }
}