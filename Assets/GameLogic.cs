using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.SceneManagement;  

public class GameLogic : MonoBehaviour
{
    public GameObject YourTurnText;
    
    public GameObject CardDropArea;

    private int playercount;

    public int playerturn;

    private int suspectplayer;

    private int couldwin = -1;

    public GameObject UI;

    public GameObject Voting;

    public GameObject SusButton;

    public TMP_InputField Wort;

    public GameObject WortInput;

    public GameObject WaitScreen;

    public GameObject Abstimmung;

    public GameObject Deck;

    public GameObject Players;

    public GameObject Playerhand;

    int provotes;

    int contravotes;

    void Start()
    {
        //unbedingt gucken dass das vllt nur für master client gemacht wird weil sonst wird das für ein großes durcheinander sorgen.
        provotes = 0;
        contravotes = 0;
        int playerturn = 0;
        playercount = PhotonNetwork.PlayerList.Length;
        Playerturn(playerturn);
        ShowYourTurn(true);        
    }

    
    public void ShowYourTurn(bool active)
    {
        //von allen anderen das ding auf false setzen
        if (PhotonNetwork.PlayerList[playerturn].NickName == PhotonNetwork.NickName)
        {
            YourTurnText.SetActive(active);
        }
        else
        {
            YourTurnText.SetActive(false);
        }
    }
    
    public void Playerturn(int playerid)
    {
        //nochmal hier all leute aufrufen

        //mach nicht von playerid sondern von dem anderen. 
        if(PhotonNetwork.PlayerList[playerid].NickName == PhotonNetwork.NickName)   
        {
            tag = "Carddrop";

            //make carddrop true for main user
            CardDrop carddrop = CardDropArea.GetComponent<CardDrop>();
            carddrop.playerturn = true;
            ShowYourTurn(true);

            //funktion macht so dass alle anderen die Karten nciht benutzen können
            this.GetComponent<PhotonView>().RPC("AllOthersUnableToMove", RpcTarget.Others, tag);
        }
    }

    [PunRPC]
    public void AllOthersUnableToMove(string tag)
    {
        GameObject element = GameObject.FindWithTag(tag);
        element.GetComponent<CardDrop>().playerturn = false;
    }

    public void SwitchTurns()
    {
        bool couldwinBool = Playerhand.transform.childCount == 0;

        //calles this function by all players cause normally switchturns is only called by one player -> all variables change
        this.GetComponent<PhotonView>().RPC("SwitchTurnsVar", RpcTarget.All, couldwinBool);
    }

    [PunRPC]
    public void SwitchTurnsVar(bool couldwinBool)
    {
        if (couldwin != -1) {
            GameOver(couldwin);
        }
        if (couldwinBool) {
            couldwin = playerturn;
        } else {
            couldwin = -1;
        }
        

        Deck.GetComponent<DeckLogic>().AddCardNum(PhotonNetwork.PlayerList[playerturn].NickName,-1);

        //guck ob oldplayer 0 karten hat. Wenn ja -> 
        if (playerturn == playercount - 1)
            playerturn = 0;            
        else 
            playerturn++;
        ShowYourTurn(true);
        Playerturn(playerturn);
    }


    public void OnClickSus()
    {
        //this is called only by user whos turn it is
        this.GetComponent<PhotonView>().RPC("ChangeScreen", RpcTarget.All);

        //do this to all player after the suspect player has typed in the word
        
    }

    [PunRPC]
    public void ChangeScreen()
    {
        int suspectplayer = playerturn;
        
        //wenn der user fertig ist dann kommt das 
        
        if(suspectplayer == 0)
        {
            suspectplayer = playercount - 1;
        }

        else
        {
            suspectplayer--;
        }

        ShowYourTurn(false);
        if(PhotonNetwork.PlayerList[suspectplayer].NickName == PhotonNetwork.NickName)
        {
            UI.SetActive(false);
            WortInput.SetActive(true);
        }

        else
        {
            UI.SetActive(false);
            WaitScreen.SetActive(true);


            Transform text = WaitScreen.transform.Find("WaitText");
            text.gameObject.GetComponent<TextMeshProUGUI>().text = "Warten auf die Eingabe von: " + PhotonNetwork.PlayerList[suspectplayer].NickName;
        }
    }

    private void Update()
    {
        
        if(PhotonNetwork.PlayerList[playerturn].NickName == PhotonNetwork.NickName && CardDropArea.transform.childCount >= 2)
        {
            //if there are at least 2 cards in the middle only then
            SusButton.SetActive(true);
        }

        else
        {
            SusButton.SetActive(false);
        }
    }

    public void ConfirmWord()
    {
        //get word typed into box 
        
        string EnteredWord = PhotonNetwork.PlayerList[suspectplayer].NickName +  " meint "+ Wort.text + " passt zu allen gelegten Karten";
        //change your screen to make it 
        WortInput.SetActive(false);
        Abstimmung.SetActive(true);

        //diesen screen wegmachen und einen anderen hinklatschen

        this.GetComponent<PhotonView>().RPC("VotingSetup", RpcTarget.Others, EnteredWord);
        
    }

    [PunRPC]
    public void VotingSetup(string EnteredWord)
    {
        //mach hier den anderen kram
        Transform inputtext = Voting.transform.Find("Caption");

        inputtext.gameObject.GetComponent<TextMeshProUGUI>().text = EnteredWord;

        Voting.SetActive(true);
        WaitScreen.SetActive(false);

    }

    public void Voteagainst()
    {
        //change your screen 
        Voting.SetActive(false);
        Abstimmung.SetActive(true);
        //change val on all screens
        this.GetComponent<PhotonView>().RPC("Vote", RpcTarget.All,true);
    }

    public void Votefor()
    {
        //change your screen 
        Voting.SetActive(false);
        Abstimmung.SetActive(true);
        //change val on all screens
        this.GetComponent<PhotonView>().RPC("Vote", RpcTarget.All,false);
    }

    [PunRPC]
    public void Vote(bool voting)
    {
        bool finsihedvote = false;

        if (voting)
        {
            contravotes++;
            //update on screen
            
            Transform Kontra = Abstimmung.transform.Find("Kontra");

            Kontra.gameObject.GetComponent<TextMeshProUGUI>().text = contravotes.ToString();
            
        }
        else 
        {
            provotes++;
            //update on screen

            Transform Pro = Abstimmung.transform.Find("Pro");
            
            Pro.gameObject.GetComponent<TextMeshProUGUI>().text = contravotes.ToString();
        }
        
        //check if votes bei dem einem oder anderem mehr sind dann zieht der jeweilige spieler so viele Karten wie in der Mitte liegen - 1
        int neededvotes = PhotonNetwork.PlayerList.Length / 2;

        //update somewhere else
        int cardsinmid = CardDropArea.transform.childCount;

        int increaseamount = 0;

        if (provotes == neededvotes)
        {
            Abstimmung.SetActive(false);
            UI.SetActive(true);
            
            Deck.GetComponent<DeckLogic>().DrawCards(PhotonNetwork.PlayerList[playerturn].NickName, cardsinmid - 1);

            //reset everything
            finsihedvote = true;            
        }
        
        if (contravotes == neededvotes)
        {
            Abstimmung.SetActive(false);
            UI.SetActive(true);
            
            Deck.GetComponent<DeckLogic>().DrawCards(PhotonNetwork.PlayerList[suspectplayer].NickName, cardsinmid - 1);

            //reset everything
            finsihedvote = true;

            couldwin = -1;
        }

        ShowYourTurn(true);

        if (finsihedvote)
        {
            provotes = 0;
            contravotes = 0;
            //set numbers back to 0
            Transform Kontra_new = Abstimmung.transform.Find("Kontra");
            Transform Pro_new = Abstimmung.transform.Find("Pro");
            Kontra_new.gameObject.GetComponent<TextMeshProUGUI>().text = "0";
            Pro_new.gameObject.GetComponent<TextMeshProUGUI>().text = "0";


            Voting.SetActive(false);
            
            //reset input fields

            //remove alle Karten aus der Mitte
            foreach(Transform child in CardDropArea.transform)
            {
                Destroy(child.gameObject);
            }
        }

        //guck ob irgendein spieler jetzt immernoch keine Karten hat und dann finsih das game
    }

    public void GameOver(int playerid)
    {
        
        if(PhotonNetwork.PlayerList[playerid].NickName == PhotonNetwork.NickName)
        {
            winorlose.podiumText = "You win :)";
        }
        else 
        {
            winorlose.podiumText = "You lose :(";
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Podium");
    }
}