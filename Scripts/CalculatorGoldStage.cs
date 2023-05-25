using UnityEngine;

[ExecuteInEditMode]
public class CalculatorGoldStage : MonoBehaviour
{
    [SerializeField] private StageSettings _stage;
    [SerializeField] private int _gold;
    private StageSettings _currentStage;

    private void Update()
    {
        int gold = 0;
        int levelCount = _stage.LevelCount;

        for (int i = 0; i < levelCount; i++)
        {
            gold += _stage.GetGoldReward(i);
        }

        _gold = gold;
    }
}
