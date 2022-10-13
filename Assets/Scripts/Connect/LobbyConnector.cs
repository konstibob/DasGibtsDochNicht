using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro;

public class LobbyConnector : MonoBehaviourPunCallbacks
{
    public TMP_InputField CreateInput;
    public TMP_InputField JoinInput;

    public TMP_Text lobbyName;
    
    private int playerID = 0;

    public GameObject canvas;
    public GameObject lobby;

    //Connect_To_Server connect_to_server;

    public Transform parentOfName;
    public TMP_Text userNamePrefab;   //Hier den datentypen ändern dann sollte alles hinhauen
    
    public GameObject startGameBtn;

    public static bool alreadyInLobby = false;


    
    private void Start()
    {   
        if (alreadyInLobby)
        {
            canvas.SetActive(false);
            lobby.SetActive(true);
            alreadyInLobby = false;
        }

        else {
        PhotonNetwork.JoinLobby();
        }
    }

    //das ist sehr schwer zu testen grade leider noch
    public void OnClickCreate()
    {
        bool roomExists = false;
        Debug.Log("test :) ");
        
        //gucken ob der raumname vllt schon besetzt ist nicht vergessen

        if(!roomExists)
        {
        
            if (CreateInput.text.Length >= 1)
            {
                //maximal 10 leute in einem Raum 
                RoomOptions options = new RoomOptions();
                options.MaxPlayers = 10;
                PhotonNetwork.CreateRoom(CreateInput.text,options);
            }
            else
            {
                //do some error handling -> display message on screen that its not enough
            }
        }
    }
    
    
    public void OnClickJoin()
    {
        PhotonNetwork.JoinRoom(JoinInput.text);

        //prüfen ob der bereits im raum 
        //dafür mgl joinen prüfen dann leaven
        int usernameCounter = 0;

        foreach (var player in PhotonNetwork.PlayerList)
            if (player.NickName == PhotonNetwork.NickName)
                usernameCounter++;

        if (usernameCounter > 1)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    
    public void OnClickLeave()
    {
        PhotonNetwork.LeaveRoom();
        canvas.SetActive(true);
        lobby.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Huh");
        
        canvas.SetActive(false);
        lobby.SetActive(true);

        //display lobby name

        //CreateInput.text beim nächsten user leer -> kein lobbyname mehr 
        if (PhotonNetwork.IsMasterClient)
        {
            lobbyName.text = CreateInput.text;
        }

        else 
        {
            lobbyName.text = JoinInput.text;
        }
        //display player name
        //möglicherweise alternativ diese UpdatePlayerList jedes mal machen (jede sekunde
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        foreach(Transform child in parentOfName)
        {
            Destroy(child.gameObject);
        }

        //add playernames to list
        //das geht so eig. aber ist irgendwie o
        
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            print(player.NickName); //add many new ones 
            TMP_Text playerName = Instantiate(userNamePrefab,parentOfName);
            playerName.text = player.NickName;
        }   
    }

    public override void OnPlayerEnteredRoom(Player newplayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player newplayer)
    {
        UpdatePlayerList();
    }


    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 2) //playercount mindestens 3 machen als bedingung vllt -> Dazu playercount nach jedem join +1 und nach jedem leave -1 
        {
            startGameBtn.SetActive(true);
        }

        else
        {
            startGameBtn.SetActive(false);
        }
    }
    
}
