using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class winorlose : MonoBehaviour
{
    public static string podiumText;

    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = podiumText;
    }
}
