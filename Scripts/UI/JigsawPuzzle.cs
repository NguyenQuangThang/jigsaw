using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JigsawPuzzle : MonoBehaviour
{
    public RuntimeGeneration runtimeGeneration;

    public GameObject changePiecePopup;
    public GameObject bg_center;
    public GameObject bg_left;
    public GameObject uiGamePlay;

    private bool isCreated = false;


    void Start()
    {
        uiGamePlay.SetActive(false);
        if (!GameManager.Instance.openChanged)
        {
            GameManager.Instance.openChanged = true;
            changePiecePopup.SetActive(true);
        }
        else
        {
            changePiecePopup.SetActive(false);
            CreatePlayGame();
        }

    }

    public void OnOptionsClicked()
    {
        isCreated = true;
        changePiecePopup.SetActive(true);
    }

    public void OnOptionsClose()
    {
        if (!isCreated)
        {
            CreatePlayGame();
        }
        changePiecePopup.SetActive(false);
    }

    /// <summary>
    /// Create game
    /// </summary>
    public void CreatePlayGame()
    {
        //Reset();
        isCreated = true;
        uiGamePlay.SetActive(true);

        changePiecePopup.SetActive(false);
        runtimeGeneration.GeneratePuzzle(false);
        bg_left.SetActive(true);
        bg_center.SetActive(true);

        //Reset();

    }

    public void NextCreatePlayGame()
    {
        //Reset();
        changePiecePopup.SetActive(false);
        runtimeGeneration.GeneratePuzzle(true);
        bg_left.SetActive(true);
        bg_center.SetActive(true);

    }

    public void onOpenThemeScene()
    {
        SceneManager.LoadScene(Constants.THEME_SCENE);
    }

    public void onMode4()
    {
        GameManager.Instance.OnSwitchPieces(4);

    }
    /// <summary>
    /// reset count piece active
    /// </summary>
    private void Reset()
    {
        GameManager.Instance.countPiece = 0;
        GameManager.Instance.numberPiece = 0;
    }
    public void onMode6()
    {
        GameManager.Instance.OnSwitchPieces(6);

    }

    public void onMode9()
    {
        GameManager.Instance.OnSwitchPieces(9);

    }

    public void onMode16()
    {
        GameManager.Instance.OnSwitchPieces(16);

    }

    public void onMode25()
    {
        GameManager.Instance.OnSwitchPieces(25);

    }

    public void onMode36()
    {
        GameManager.Instance.OnSwitchPieces(36);

    }
}
