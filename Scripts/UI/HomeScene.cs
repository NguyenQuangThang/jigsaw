using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScene : MonoBehaviour
{
    public GameObject popupSetting;

    public void onStartGame()
    {
        SceneManager.LoadScene(Constants.MENU_SCENE);
    }

    public void onSettingPopup()
    {
        popupSetting.SetActive(true);
    }

    public void onMusic()
    {

    }

    public void onCloseSettingPopup()
    {
        popupSetting.SetActive(false);
    }
}
