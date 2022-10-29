using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    public int counter;
    public GameObject player;
    public TMP_Text comboText;
    public float comboWindowTime = 0.2f;
    private float _comboTimer;
    private PlayerController _playerController;

    // Start is called before the first frame update
    private void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_playerController.IsGrounded() || _playerController.rampSliding)
        {
            _comboTimer = comboWindowTime;
        }
        else
        {
            _comboTimer -= Time.deltaTime;
        }

        if (_comboTimer < 0f)
        {
            counter = 0;
        }

        if (counter == 0)
        {
            comboText.text = "";
        }
        else if (counter > 0)
        {
            comboText.text = "Combo\r\nx" + counter;
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
