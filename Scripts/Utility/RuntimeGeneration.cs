//-----------------------------------------------------------------------------------------------------	
//  Simple demo script to help in puzzle-generation demonstration
//-----------------------------------------------------------------------------------------------------	
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class RuntimeGeneration : MonoBehaviour
{
    //public GameObject uiGamePlay;
    public Texture2D image;                         // Will be used as main puzzle image
    public bool generateBackground = true;          // Automatically generate puzzle background from the source image
    public bool alignWithCamera = true;             // Automatically align puzzle/background with camera center
    public bool clearOldSaves = true;               // Clear existing Save data data during generation
    [TextArea]
    public string pathToImage;                      // pathToImage should starts from "http://"(for online image)  or  from "file://" (for local) 

    public PuzzleGenerator_Runtime puzzleGenerator;
    public GameController gameController;
    public Text rows;
    public Text cols;

    //private void Start()
    //{
    //    uiGamePlay.SetActive(false);
    //}
    //============================================================================================================================================================
    public void GeneratePuzzle(bool isNext = false)
    {
        //uiGamePlay.SetActive(true);
        GameManager.Instance.countPiece = 0;

        if (puzzleGenerator == null || gameController == null)
        {
            Debug.LogWarning("Please assign puzzleGenerator and gameController to " + gameObject.name + "RuntimeGenerator");
            return;
        }

        gameController.enabled = false;

        //Delete previously generated puzzle
        if (gameController.puzzle != null)
            Destroy(gameController.puzzle.gameObject);
        if (gameController.background != null)
            Destroy(gameController.background.gameObject);

        if (!isNext)
            image = GameManager.Instance.getTextureJigsaw();
        else
            image = GameManager.Instance.getNextTexture();

        //image = TextureUtility.Scale(image, 2169, 1201);
        if (!image)
            puzzleGenerator.CreateFromExternalImage(pathToImage);
        else
            gameController.puzzle = puzzleGenerator.CreatePuzzleFromImage(image);


        StartCoroutine(StartPuzzleWhenReady());
    }

    //-----------------------------------------------------------------------------------------------------
    IEnumerator StartPuzzleWhenReady()
    {
        while (puzzleGenerator.puzzle == null)
        {
            yield return null;
        }

        if (clearOldSaves)
        {
            PlayerPrefs.DeleteKey(puzzleGenerator.puzzle.name);
            PlayerPrefs.DeleteKey(puzzleGenerator.puzzle.name + "_Positions");
        }

        gameController.puzzle = puzzleGenerator.puzzle;

        // Generate backround if needed
        if (generateBackground)
            gameController.background = puzzleGenerator.puzzle.GenerateBackground(puzzleGenerator.GetSourceImage());
        //puzzleGenerator.puzzle.OnDrawGizmos();
        // Align with Camera if needed
        if (alignWithCamera)
            puzzleGenerator.puzzle.AlignWithCameraCenter(gameController.gameCamera);

        gameController.enabled = true;
    }

    //-----------------------------------------------------------------------------------------------------	
    public void SetRows(float _amount)
    {
        if (puzzleGenerator != null)
            puzzleGenerator.rows = (int)_amount;

        if (rows != null)
            rows.text = ((int)_amount).ToString() + " Row";
    }

    //-----------------------------------------------------------------------------------------------------	
    public void SetCols(float _amount)
    {
        if (puzzleGenerator != null)
            puzzleGenerator.cols = (int)_amount;

        if (cols != null)
            cols.text = ((int)_amount).ToString() + " Col";
    }

    //-----------------------------------------------------------------------------------------------------	
}