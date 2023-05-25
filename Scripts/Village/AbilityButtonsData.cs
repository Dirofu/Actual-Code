using UnityEngine;

public class AbilityButtonsData : MonoBehaviour
{
    [SerializeField] private AbilityImprovementButton _damage0Button;
    [SerializeField] private AbilityImprovementButton _damage1Button;

    private SaveSystem _saveSystem;

    public AbilityImprovementButton Damage0Button => _damage0Button;
    public AbilityImprovementButton Damage1Button => _damage1Button;

    public void SetSave(SaveSystem save)
    {
        _saveSystem = save;
        SetAcquired(_saveSystem.SaveData);
    }

    public void SetAcquired(SaveData saves)
    {
        _damage0Button.SetAcquired(saves.Damage0Button);
        _damage1Button.SetAcquired(saves.Damage1Button);
    }
}
