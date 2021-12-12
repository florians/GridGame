using UnityEngine;

public enum Events
{
    Destroy,
    GoDown,
    Hp,
    Trap,
    Upgrade
}

public class ClickEvent : MonoBehaviour
{
    bool isUsed = false;
    public Events e;
    void OnMouseDown()
    {
        GridManager parentScript = gameObject.transform.parent.transform.parent.GetComponent<GridManager>();
        if (e == Events.GoDown && isUsed == true)
        {
            parentScript.player.LevelUp();
            parentScript.GenerateGrid();
        }
        if (!isUsed)
        {
            switch (e)
            {
                case Events.Hp:
                    parentScript.player.AddHp();
                    break;
                case Events.Trap:
                    parentScript.player.RemoveHp();
                    break;
                case Events.Upgrade:
                    parentScript.player.AddUpgrade();
                    break;
                default:
                    break;
            }
            parentScript.OnClickEvent(gameObject);
            isUsed = true;
        }
    }
}
