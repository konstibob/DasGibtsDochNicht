using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End_Turn : MonoBehaviour
{
    public GameObject yourButton;
    public GameObject Carddrop;

    void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
        foreach(Transform child in Carddrop.transform)
        {
            GameObject.Destroy(child.gameObject);

        }

	}

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.UI.Button btn  = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
