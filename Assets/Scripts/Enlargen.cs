using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enlargen : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    Vector3 cachedScale;

    void Start()
    {
        cachedScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //make it large 
        //propably also increase spawnbox
        Debug.Log("Hi");
        //transform.SetAsFirstSibling ();
        transform.localScale += new Vector3(1F, 1F, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //make it small again
        //propably also increase spawnbox
        
        transform.localScale = cachedScale;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Hi");
    }
}
