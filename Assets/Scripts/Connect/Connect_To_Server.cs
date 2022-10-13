using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro;


public class Connect_To_Server : MonoBehaviourPunCallbacks
{
    public TMP_InputField  usernameInput;
    public TMP_Text buttonText;
    


    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 1)
        {   
            PhotonNetwork.NickName = usernameInput.text;
            bool alreadyExists = false;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.NickName == PhotonNetwork.NickName)
                {
                    alreadyExists = true;
                    break;
                }
            }

            if (alreadyExists)
            {
                buttonText.text = "Please Choose other name";
            }

            else 
            {
                buttonText.text = "Connecting...";
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        //DontDestroyOnLoad(this);
        SceneManager.LoadScene("JoinIn");
    }
}