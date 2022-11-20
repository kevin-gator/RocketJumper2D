using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTextTrigger : MonoBehaviour
{
    public TMP_Text tutorialText;
    public string textToDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tutorialText.text = textToDisplay;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        tutorialText.text = "";
    }
}
