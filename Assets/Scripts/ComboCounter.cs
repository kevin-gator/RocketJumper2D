using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    public int counter;
    public GameObject player;
    public TMP_Text comboText;
    public float comboWindowTime = 0.1f; // Time before combo drops after landing
    private float _comboTimer;
    private PlayerController _playerController;
    private float _textScalingTime;
    public float textScaleAmount = 1.2f;

    // Start is called before the first frame update
    private void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_playerController.IsGrounded() || _playerController.rampSliding) //If player is off the ground or rampsliding
        {
            _comboTimer = comboWindowTime; //combo timer is set to the window time
        }
        else //If player is grounded and not rampsliding
        {
            _comboTimer -= Time.deltaTime; //combo timer decreases with time
        }

        if (_comboTimer < 0f) //Sets combo counter to 0 after the combo window expires
        {
            counter = 0;
        }

        //Sets combo text UI
        if (counter == 0)
        {
            comboText.text = "";
        }
        else if (counter > 1)
        {
            comboText.text = "COMBO\r\nx" + counter;
        }

        if (_textScalingTime >= 0f)
        {
            _textScalingTime -= Time.deltaTime;
        }
        else
        {
            _textScalingTime = 0f;
        }

        float textScale = 1 + _textScalingTime * textScaleAmount;

        comboText.transform.localScale = new Vector3(textScale, textScale, textScale);
    }

    async public void AddCount()
    {
        //Creates a 100 ms delay when triggered to solve issues with detecting a rocket jump when the player is grounded
        await Task.Delay(100);
        //Increased counter by 1
        _textScalingTime = 0.1f;

        counter += 1;
        //Debug.Log(comboCounter);
    }

    /*public float GetCount()
    {
        return counter;
    }*/
}
