using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class ComboCounter : MonoBehaviour
{
    public int counter = 0;
    public GameObject player;
    private PlayerController playerController;
    public TMP_Text comboText;
    public float comboWindowTime = 0.2f;
    private float comboTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!playerController.IsGrounded() || playerController.rampSliding)
        {
            comboTimer = comboWindowTime;
        }
        else
        {
            comboTimer -= Time.deltaTime;
        }

        if(comboTimer < 0f)
        {
            counter = 0;
        }

        if(counter == 0)
        {
            comboText.text = "";
        }
        else if(counter > 0)
        {
            comboText.text = ("Combo\r\nx" + counter);
        }
    }

    async public void AddCount()
    {
        await Task.Delay(100);
        counter += 1;
        //Debug.Log(comboCounter);
    }

    public float GetCount()
    {
        return counter;
    }
}
