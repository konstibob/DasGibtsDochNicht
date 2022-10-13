using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardCounter : MonoBehaviour
{
    public TMP_Text text;
    private int _Count = 0;
    public int Count {
        get { return _Count; }
        set { _Count = value; UpdateText(); }
    }

    public void UpdateText()
    {
        text.text = "Karten :" + _Count.ToString();
    }
}
