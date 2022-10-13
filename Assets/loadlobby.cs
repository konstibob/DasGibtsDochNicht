using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro;

public class loadlobby : MonoBehaviourPunCallbacks
{
    public void LoadRoom()
    {
        //alle leaven den room 
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        StartCoroutine(WaitForRoomLeave());
    }

    IEnumerator WaitForRoomLeave()
    {
        while (PhotonNetwork.InRoom)
            yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("JoinIn");
    }
}
