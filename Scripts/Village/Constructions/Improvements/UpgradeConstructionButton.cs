public class UpgradeConstructionButton : ImprovementButton
{
    public override ImprovementInfo Info => Construction.GetNextLevel();

    protected override void ChildInit()
    {
        CheckConstructionLevel();
    }

    protected override void Improve()
    {
        Construction.Upgrade();
        CheckConstructionLevel();
    }

    private void CheckConstructionLevel()
    {
        if (Construction.HasNextLevel == false)
            gameObject.SetActive(false);
    }
}
