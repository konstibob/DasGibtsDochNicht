using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TMP_Text LobbyName;
    // Start is called before the first frame update
     void Awake()
    {
    DontDestroyOnLoad(this);
    }

    public void SetRoomname()
    {
        Debug.Log("wow");
       //LobbyName.text = "test";
    }
}
