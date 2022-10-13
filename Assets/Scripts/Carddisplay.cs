using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carddisplay : MonoBehaviour
{
    public Card card;

    public GameObject nameText;

    public GameObject descriptionText;

    public Image artwork;

    public Image Cardback;

    public bool cardback_off;
    // Start is called before the first frame update
    void Start()
    {
        //statt das nur einmal am anfang zu machen, in eine funktion machen, die einmal am Anfang augerufen wird und danach jedes mal 
        if (cardback_off)
        {
        
            Cardback.enabled = false;
        }
        
        nameText.GetComponent<TMPro.TextMeshProUGUI>().text = card.name;

        descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = card.description;

        artwork.GetComponent<Image>().sprite = card.artwork;

        
        
    }

    public void test()
    {

    }
}
