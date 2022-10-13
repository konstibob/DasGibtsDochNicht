using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;
using System.Linq;

public class DeckLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public GameObject Kartenanzahl;
    private TMP_Text TextToChange;
    public GameObject PlayerHand;

    public GameObject Players;
    public List<GameObject> Deck = new List<GameObject>();
    // Start is called before the first frame update

    //maybe change image based on card amounts 
    void Start()
    {
        Kartenanzahl.SetActive(false);
        TextToChange = Kartenanzahl.GetComponent<TMP_Text>();
        
        Deck = Shuffle(Deck);
        
        GiveCards();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Kartenanzahl.SetActive(true);

        
        Debug.Log(Deck.Count);
        TextToChange.text = Deck.Count.ToString() + " Karten übrig"; //CardDeck.Count  
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Kartenanzahl.SetActive(false);
    }
    

    public static List<GameObject> Shuffle(List<GameObject> deck)
    {
        
        for(int n = deck.Count - 1; n > 1; n--)
        {  
            int k = Random.Range(0,n); //random number generierung kann man natürlich sauberer machen aber erstmal egal

            //swap zwischen k und n 
            GameObject tmp = deck[n];
            deck[n] = deck[k];
            deck[k] = tmp;            
        }

        return deck;
    }
    

    //function that gives cards to all enemys for the first time in the game
    public void GiveCards()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length ; i++) 
        {
            int carddraw = 3; //how many cards everyone gets
            DrawCards(PhotonNetwork.PlayerList[i].NickName, carddraw);
        }
    }

    public void DrawCards(string nickName, int requestedCarddraw) 
    {
        if (PhotonNetwork.NickName != nickName) { return; }
        int carddraw = System.Math.Min(Deck.Count, requestedCarddraw);
        
        int cardnum = 0;
        while(cardnum != carddraw)  //anzahl der Karten die jeder kriegt
        {
            //add card to playerhand
            Instantiate(Deck[cardnum], PlayerHand.transform);

            //increase cardnumber you give to player
            cardnum++;
        }
        
        GetComponent<PhotonView>().RPC("AddCardNum", RpcTarget.Others, nickName, carddraw);
        GetComponent<PhotonView>().RPC("RemoveCardsFromDeck", RpcTarget.All, carddraw);
    }

    [PunRPC]
    public void AddCardNum(string nickName, int num)
    {
        foreach (Transform playerTransform in Players.transform)
        {
            var player = playerTransform.gameObject;

            if (player.name == nickName) 
            {
                player.GetComponent<CardCounter>().Count += num;
            }
        }
    }

    [PunRPC]
    public void RemoveCardsFromDeck(int num)
    {
        for (int i = num; i > 0; i--)
        {
            Deck.RemoveAt(0);
        }
    }

    public bool IsEmpty() {
        return Deck.Count <= 0;
    }
}
