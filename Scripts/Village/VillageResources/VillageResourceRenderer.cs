public class VillageResourceRenderer : ResourceRenderer
{
    private int _currentAmount;
    private IReadOnlyResource _resource;

    public override void Render(IReadOnlyResource resource)
    {
        base.Render(resource);
        _resource = resource;
        resource.AmountChanged += OnAmountChanged;
    }

    private void OnDestroy()
    {
        _resource.AmountChanged -= OnAmountChanged;
    }

    private void OnAmountChanged(int newAmount)
    {
        if (newAmount > _currentAmount)
            PlayGreenAnimation();
        else
            PlayRedAnimation();

        _currentAmount = newAmount;
        AmountText.text = newAmount.ToString();
    }
}