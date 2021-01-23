using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemThemeCell : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI nameTheme;
    public System.Action<ItemTheme> callback;
    ItemTheme _data;
    public void SetData(ItemTheme data)
    {
        _data = data as ItemTheme;
        string url = "Theme/Theme_" + data.nameTheme;
        img.sprite = Resources.Load<Sprite>(url);
        nameTheme.text = data.nameTheme;
    }

    public void OnClicked()
    {
        if (callback != null)
            callback(_data);
    }
}
