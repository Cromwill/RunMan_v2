using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement/add Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _showName;
    [Multiline]
    [SerializeField] private string _description;
    [SerializeField] private bool _isRecord;

    [SerializeField] private float[] _conditions;
    [SerializeField] private float _rewardIndex;

    public int ID => _id;
    public string Name => _name;
    public string ShowName => _showName;
    public string Description => _description;
    public bool IsRecord => _isRecord;

    public float[] Condition => _conditions;

    public void AddValue(float value)
    {
        AchievementDataStorage.AddAchievement(this, value);
    }

    public float GetCurrentValue()
    {
        return AchievementDataStorage.GetAchievementValue(this);
    }

    public bool IsRewardCollection(int index)
    {
        var collectedRewardIndex = AchievementDataStorage.GetAchievementReward(this);

        return collectedRewardIndex > index;
    }

    public int GetReward()
    {
        var collectedRewardIndex = AchievementDataStorage.GetAchievementReward(this);
        AchievementDataStorage.SaveAchievementReward(this, collectedRewardIndex + 1);
        return (int)(_conditions[collectedRewardIndex + 1] * _rewardIndex);
    }
}
