using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class ChooseTheme : MonoBehaviour
{
    public GridLayoutGroup gridSprite;
    public GameObject prefab;
    public TextMeshProUGUI titleTheme;

    Sprite[] list;
    private void Start()
    {
        list = GameManager.Instance.getListImgeByTheme(GameManager.Instance.themeCurent);
        titleTheme.text = GameManager.Instance.themeCurent.nameTheme;

        loadUI();
    }

    private void loadUI()
    {
        GlobalFunction.DestroyAllChild(gridSprite.transform);
        for(int i = 0; i < list.Length; i++)
        {
            GameObject go = GlobalFunction.AddChild(gridSprite.gameObject, prefab);
            ItemPicCell script = go.GetComponent<ItemPicCell>();
            script.SetData(list[i]);
            script.callback = callOpenScene;

        }

    }
    public void onJigsawPuzzle()
    {
        SceneManager.LoadScene(Constants.JIGSAW_PUZZLE);
    }

    public void onOpenMenu()
    {
        SceneManager.LoadScene(Constants.MENU_SCENE);
    }

    void callOpenScene(Sprite sprite)
    {
        GameManager.Instance.spriteCurrent = sprite;
        SceneManager.LoadScene(Constants.JIGSAW_PUZZLE);
    }
}
