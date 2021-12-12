using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            transform.Find("KillScreen").gameObject.SetActive(true);
            push = false;
        }
    }
    public void Replay()
    {
        gridContainer.GetComponent<GridManager>().Replay();
        killScreen.SetActive(false);
    }
}
