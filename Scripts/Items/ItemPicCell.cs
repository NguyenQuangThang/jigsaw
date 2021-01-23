using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPicCell : MonoBehaviour
{
    public Image img;
    public System.Action<Sprite> callback;
    Sprite _sprite;

    public void SetData(Sprite sprite)
    {
        _sprite = sprite as Sprite;
        img.sprite = sprite;
    }

    public void onClicked()
    {
        if (callback != null)
            callback(_sprite);
    }
}
