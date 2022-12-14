using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    public int counter;
    public GameObject player;
    public TMP_Text comboText;
    public TMP_Text syncText;
    public float comboWindowTime = 0.1f; //Creates a brief grace period after the player hits the ground within which they can continue their combo if they hit another rocket jump within the window
    private float _comboTimer;
    private PlayerController _playerController;
    private float _textScalingTime;
    public float textScaleAmount = 1.2f;
    public float rocketHits;
    private float _lastRocketHit;
    public float syncWindowTime = 0.05f; //The window in which two rockets must explode by the player in order to register as a sync jump
    private float _syncTextDisplayValue;
    public float syncTextDisplayTime = 2f;
    private float _syncTextTimer;
    public ScreenShake screenShake;

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

        //Sets sync text UI
        if (rocketHits > 1)
        {
            _syncTextDisplayValue = rocketHits;
            _syncTextTimer = syncTextDisplayTime;
        }

        if(_syncTextDisplayValue > 1)
        {
            _syncTextTimer -= Time.deltaTime;
        }
        else
        {
            _syncTextTimer = 0f;
        }

        if(_syncTextTimer > 0f)
        {
            syncText.text = "SYNC\r\nx" + _syncTextDisplayValue;
        }
        else
        {
            syncText.text = "";
            _syncTextDisplayValue = 0f;
        }

        if (_lastRocketHit > 0)
        {
            _lastRocketHit -= Time.deltaTime;
        }
        else
        {
            rocketHits = 0;
        }

        syncText.transform.localScale = new Vector3(textScale, textScale, textScale);
    }

    async public void AddCount()
    {
        //Creates a delay when triggered to solve issues with detecting a rocket jump when the player is grounded
        await Task.Delay(1);
        _textScalingTime = 0.1f;
        //Increased counter by 1
        counter += 1;
        rocketHits += 1;
        _lastRocketHit = syncWindowTime;
        screenShake.Shake();
    }


    /*public float GetCount()
    {
        return counter;
    }*/
}
