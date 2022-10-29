using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboText : MonoBehaviour
{
    public GameObject player;
    private ComboCounter comboCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        comboCounter = player.GetComponent<ComboCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshPro>().SetText("COMBO\r\n"+comboCounter.GetCount());
    }
}
