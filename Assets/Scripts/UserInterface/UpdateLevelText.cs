using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLevelText : MonoBehaviour
{
    Text text;
    public static int val;
    public static bool push = false;
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (push) {
            text.text = "" + val;
            push = false;
        }
    }
}
