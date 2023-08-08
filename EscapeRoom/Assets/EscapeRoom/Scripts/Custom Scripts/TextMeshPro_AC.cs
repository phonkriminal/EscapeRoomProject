using UnityEngine;
using System.Collections;
using AC;
using UnityEngine.UI;
using TMPro;

public class TextMeshPro_AC : MonoBehaviour
{
    public Text textToCopyFrom;
    public TextMeshProUGUI textMeshToCopyTo;

    private void Update()
    {
        textMeshToCopyTo.text = textToCopyFrom.text;

    }
}