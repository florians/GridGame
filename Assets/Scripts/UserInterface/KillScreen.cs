using UnityEngine;

public class KillScreen : MonoBehaviour
{
    [SerializeField]
    GameObject killScreen;
    [SerializeField]
    GameObject gridContainer;
    public static bool push = false;

    void Update()
    {
        if (push) {
            transform.GetChild(1).gameObject.SetActive(true);
            push = false;
        }
    }
    public void Replay()
    {
        gridContainer.GetComponent<GridManager>().Replay();
        killScreen.SetActive(false);
    }
}
