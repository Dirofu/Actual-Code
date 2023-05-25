using UnityEngine;
using UnityEngine.UI;

public class AbilityImprovementButton : ImprovementButton
{
    [SerializeField] private string _id;
    [SerializeField] private Image _acquiredImage;
    [SerializeField] private ImprovementInfo _improvementInfo;

    public override ImprovementInfo Info => _improvementInfo;
    public bool IsAcquired { get; private set; }
    public override bool CanAcquire => base.CanAcquire && IsAcquired == false;

    private void Start()
    {
        IsAcquired = PlayerPrefs.GetInt(_id) == 1;
        _acquiredImage.gameObject.SetActive(IsAcquired);
    }

    protected override void Improve()
    {
        _acquiredImage.gameObject.SetActive(true);
        SetAcquired(true);
    }

    public void SetAcquired(bool value)
    {
        IsAcquired = value;
        int acquired = value ? 1 : 0;
        PlayerPrefs.SetInt(_id, acquired);
        PlayerPrefs.Save();
    }
}
