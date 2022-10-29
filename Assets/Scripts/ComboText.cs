using TMPro;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    public GameObject player;
    private ComboCounter _comboCounter;

    // Start is called before the first frame update
    private void Start()
    {
        _comboCounter = player.GetComponent<ComboCounter>();
    }

    // Update is called once per frame
    private void Update()
    {
        gameObject.GetComponent<TextMeshPro>().SetText("COMBO\r\n" + _comboCounter.GetCount());
    }
}
