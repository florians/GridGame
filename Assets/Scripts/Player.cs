using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Player", order = 1)]
public class Player : ScriptableObject
{
    public int baseLevel = 1;
    public int baseHp = 5;
    public int baseUpgrade = 3;

    public int currentLevel;
    public int currentHp;
    public int currentUpgrade;

    public void Init()
    {
        currentLevel = baseLevel;
        currentHp = baseHp;
        currentUpgrade = baseUpgrade;
        UpdateUi();
    }

    public void UpdateUi()
    {
        UpdateHpText.push = true;
        UpdateHpText.val = currentHp;
        UpdateLevelText.push = true;
        UpdateLevelText.val = currentLevel;
    }
    public void LevelUp()
    {
        currentLevel++;
        UpdateUi();
    }
    public void RemoveHp()
    {
        if (currentHp > 0)
        {
            currentHp--;
        }
        if (currentHp == 0)
        {
            KillScreen.push = true;
        }
        UpdateUi();
    }
    internal void AddHp()
    {
        currentHp++;
        UpdateUi();
    }
    public void AddUpgrade()
    {
        currentUpgrade++;
    }
}
