using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplaySomeStuff : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    public TextMeshProUGUI detailsText;

    private void OnEnable()
    {
        for (var i = 0; i < buttons.Count; ++i)
        {
            var capturedButtonIndex = i;
            buttons[i].onClick.AddListener(() => { 
                Debug.Log(capturedButtonIndex);
                detailsText.text = $"Pressed: {capturedButtonIndex.ToString()}";
            });
        }
    }
}
