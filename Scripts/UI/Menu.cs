using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GridLayoutGroup gridTheme;
    public GameObject prefab;

    private void Start()
    {
        loadUI();
    }

    public void onShowChooseTheme()
    {
        SceneManager.LoadScene(Constants.THEME_SCENE);
    }

    public void onGoHome()
    {
        SceneManager.LoadScene(Constants.HOME_SCENE);
    }

    private void loadUI()
    {
        GlobalFunction.DestroyAllChild(gridTheme.transform);
        for (int i = 0; i < Constants.itemThemes.Length; i++)
        {
            GameObject go = GlobalFunction.AddChild(gridTheme.gameObject, prefab);
            ItemThemeCell script = go.GetComponent<ItemThemeCell>();
            script.SetData(Constants.itemThemes[i]);
            script.callback = onCallClicked;
        }

    }
    void onCallClicked(ItemTheme data)
    {
        GameManager.Instance.themeCurent = data;
        SceneManager.LoadScene(Constants.THEME_SCENE);
    }


}
