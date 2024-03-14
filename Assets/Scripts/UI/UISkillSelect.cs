using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillSelect : MonoBehaviour
{
    private Image _icon;
    private TextMeshProUGUI _textMeshPro;
    private int _childCount;
    private UIExpBar _expBar;

    void Awake()
    {
        _icon = GetComponentInChildren<Image>();
        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Constructor(Sprite sprite, string text, int chldCount, UIExpBar uIExpBar)
    {
        if (sprite != null) 
            _icon.sprite = sprite;

        _textMeshPro.text = text;
        _childCount = chldCount;
        _expBar = uIExpBar;
    }

    public void OnButtonClicked()
    {
        _expBar.OnSkillSelected(_childCount);
    }

}
