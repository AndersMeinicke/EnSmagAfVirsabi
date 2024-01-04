using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLanguageChanger : MonoBehaviour
{
    public string DanskKopipasta;
    public string EngelskKopipasta;
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    public void Activate()
    {
        if(text.text == DanskKopipasta)
        {
            text.text = EngelskKopipasta;
        }
        else
        {
            text.text = DanskKopipasta;
        }
    }
}
