using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class CardDrop : MonoBehaviourPun, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject AllCards;

    public GameObject GameLogic;

    public bool playerturn;

    void Awake()
    {
        playerturn = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
    }

    public void OnDrop(PointerEventData eventData)
    {
        //make players turns switch
        

        //make Card dropped visible for everyone
        
        if(playerturn)
        {
            Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

            //drags object in center Carddrop Area
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            d.return_parent = this.transform;
            
            //call switchturns() on gamelogic
            //switchturns soll playerturn + 1 solange die zahl nicht größer ist als die Liste alle spieler 
            //machen und dann bei dem diesen your turn kurz aufklicken lassen 
            //danach ruft 
            //aber dies sollte der master client aufrufen damit nur bei ihm die values geändert werden
            GameLogic.GetComponent<GameLogic>().SwitchTurns();

            

            var cardname = eventData.pointerDrag.GetComponent<Carddisplay>().card.name;
            this.GetComponent<PhotonView>().RPC("CardWasDropped", RpcTarget.Others, cardname);
        }
    }
    
    [PunRPC]
    public void CardWasDropped(string cardname)
    {
        foreach (var cardPrefab in AllCards.GetComponent<AllCards>().Cards)
        {
            if (cardPrefab.GetComponent<Carddisplay>().card.name == cardname)
            {
                Instantiate(cardPrefab, transform);
                break;
            }
        }
    }
}
