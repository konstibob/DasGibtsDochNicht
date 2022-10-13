using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class AddPlayers : MonoBehaviour
{
    public GameObject Player;
    public GameObject Parent;
    private int playerlocation;

    // Start is called before the first frame update
    void Awake()
    {
        //Setup der Spieler für den Server
        //dummerweise zählz global.playercount die anzahl der spieler im spiel und nicht in der lobby. Das unbedingt ändern!
        
        //PhotonNetwork.PlayerList.Count
        for (int i = 0; i < PhotonNetwork.PlayerList.Length ; i++) 
        {
            if (PhotonNetwork.PlayerList[i].NickName != PhotonNetwork.NickName) //PhotonNetwork.NickName
            {
                continue;
            }

            else 
            {
                playerlocation = i;
                break;
            }
        }  
    
         
        for (int j = playerlocation + 1; j < PhotonNetwork.PlayerList.Length; j++)
        {
            GameObject x = Instantiate(Player,Parent.transform);
            x.name = PhotonNetwork.PlayerList[j].NickName;
            GameObject Text = x.transform.GetChild(0).gameObject;
            Text.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[j].NickName;
        }

        for (int k = 0; k < playerlocation; k++)
        {
            GameObject x = Instantiate(Player,Parent.transform);
            x.name = PhotonNetwork.PlayerList[k].NickName;
            GameObject Text = x.transform.GetChild(0).gameObject;
            Text.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[k].NickName;
        }

    }
    
            
            
            //Debug.Log(PhotonNetwork.PlayerList[i].NickName);
        
        
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
