public class Grease : ISkill
{
    public void Execute()
    {
        GameManager.Instance.isFireStrong = true;
    }
}