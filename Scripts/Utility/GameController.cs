//-----------------------------------------------------------------------------------------------------	
// Script controls whole gameplay, UI and all sounds
//-----------------------------------------------------------------------------------------------------	
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class GameController : MonoBehaviour
{

    public Camera gameCamera;
    public PuzzleController puzzle;

    // Background (assembled puzzle preview)
    public Renderer background;
    public bool adjustBackground = true;
    public float backgroundTransparency = 0.1f;

    // Game UI
    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject bg_right;

    public TextMeshProUGUI hintCounterUI;
    public TextMeshProUGUI timerCounterUI;
    public GameObject timerUI;
    public GameObject fx_firework;
    public GameObject fx_fireworkRed;
    public GameObject fx_fireworkGreen;



    // Game rules
    public float timer = 200;                 // Time limit for level
    public int hintLimit = 100;         // Hints limit for level
    public bool invertRules = false;    // Allows to invert basic rules - i.e. player should decompose  the images


    // Important internal variables - please don't change them blindly
    CameraController cameraScript;
    float timerTime = 200.0f;
    float remainingTime;
    bool gameFinished;
    int remainingHints;
    Color backgroundColor;
    static Vector3 oldPointerPosition;

    

    //=====================================================================================================
    // Initialize
    void OnEnable()
    {
        // Prepare Camera
        if (!gameCamera)
            gameCamera = Camera.main;

        gameCamera.orthographic = true;
        gameCamera.orthographicSize = 3.18f;
        cameraScript = gameCamera.GetComponent<CameraController>();


        // Initiate puzzle and prepare background
        if (StartPuzzle(puzzle))
        {
            // Mới sửa check active

            ///puzzle.SetPiecesActive(true);
            PrepareBackground(background);
        }


        // Load saved data
        Load();

        // Prepare UI (disable all redudant at start)   
        if (winUI)
            winUI.SetActive(false);

        if (bg_right)
            bg_right.SetActive(true);

        if (loseUI)
            loseUI.SetActive(false);

        if (pauseUI)
            pauseUI.SetActive(false);

        if (timerCounterUI)
            timerCounterUI.gameObject.SetActive(timer > 0);
        if (timerUI)
            timerUI.SetActive(timer > 0);

        if (hintCounterUI)
        {
            //hintCounterUI.gameObject.SetActive(remainingHints > 0);
            hintCounterUI.text = remainingHints.ToString();
        }


        // Init timer
        timerTime = Time.time + remainingTime;
        Time.timeScale = 1.0f;

        Cursor.lockState = CursorLockMode.Confined;

        if (!puzzle)
            this.enabled = false;
    }

    //-----------------------------------------------------------------------------------------------------	
    // Main game cycle
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Pause();


        if (puzzle && Time.timeScale > 0 && !gameFinished)
        {
            // Process puzzle and react on it state
            switch (puzzle.ProcessPuzzle(
                                            GetPointerPosition(gameCamera),
                                            !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0) && (!cameraScript || !cameraScript.IsCameraMoved()),
                                            GetRotationDirection()
                                          ))
            {
                case PuzzleState.None:
                    ;
                    break;

                case PuzzleState.DragPiece:
                    break;

                case PuzzleState.ReturnPiece:
                    break;

                case PuzzleState.DropPiece:
                    break;

                // Hide all pieces and finish game - if whole puzzle Assembled 	
                case PuzzleState.PuzzleAssembled:
                    if (background && !invertRules)
                        puzzle.SetPiecesActive(false);
                    SpawnEffect();
                    if (winUI)
                        winUI.SetActive(true);
                    if (bg_right)
                        bg_right.SetActive(false);
                    gameFinished = true;
                    break;
            }


            ProcessTimer();
        }
        else  // Show background (assembled puzzle) if gameFinished
            if (gameFinished && (!loseUI || (loseUI && !loseUI.activeSelf)))
            if (!invertRules)
                ShowBackground();


        // Control Camera   
        if (cameraScript && puzzle)
            // if (puzzle.GetCurrentPiece() == null)  cameraScript.ManualUpdate();
            cameraScript.enabled = (puzzle.GetCurrentPiece() == null);

    }

    //-----------------------------------------------------------------------------------------------------	 
    // Get current pointer(mouse or single touch) position  
    static Vector3 GetPointerPosition(Camera _camera)
    {
        Vector3 pointerPosition = oldPointerPosition;

        Vector3 vector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        pointerPosition = oldPointerPosition = _camera.ScreenToWorldPoint(vector);
#else // For mobile
			if (Input.touchCount > 0)  
				pointerPosition = oldPointerPosition = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif


        return pointerPosition;
    }

    //-----------------------------------------------------------------------------------------------------	 
    // Get current rotation basing on mouse or touches
    float GetRotationDirection()
    {
        float rotation = 0;

        // For Desktop - just set rotation to "clockwise" (don't change the permanent speed)
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        if (Input.GetMouseButton(1))
            rotation = 1;
#else // For mobile - calculate angle changing between touches and use it.
            if (Input.touchCount > 1)  
            {
					// If there are two touches on the device... Store both touches.
					Touch touchZero = Input.GetTouch (0);
					Touch touchOne 	= Input.GetTouch (1);

					// Find the angle between positions.
					float currentAngle = Vector2.SignedAngle(touchZero.position, touchOne.position); 
					float previousAngle = Vector2.SignedAngle(touchZero.position - touchZero.deltaPosition, touchOne.position - touchOne.deltaPosition);

					rotation = currentAngle - previousAngle;
			}
                 //Alternative (sign/direction based):  // rotation = (int)Mathf.Sign(Vector2.SignedAngle(Vector2.up, Input.GetTouch(1).position-Input.GetTouch(0).position));
#endif

        return rotation;
    }

    //-----------------------------------------------------------------------------------------------------	 
    // Switch puzzle and background to another
    public void SwitchPuzzle(PuzzleController _puzzle, Renderer _background)
    {
        if (_puzzle && _puzzle != puzzle)
            StartPuzzle(_puzzle);

        if (_background && _background != background)
            PrepareBackground(_background);
    }

    //-----------------------------------------------------------------------------------------------------	 
    // Prepare puzzle and Decompose it if needed
    public bool StartPuzzle(PuzzleController _puzzle)
    {
        if (!_puzzle)
            _puzzle = gameObject.GetComponent<PuzzleController>();

        if (!_puzzle)
        {
            Debug.LogWarning("PuzzleController should be assigned to puzzle property of GameController - check " + gameObject.name);
            return false;
        }


        if (puzzle && puzzle.gameObject != gameObject)
            puzzle.gameObject.SetActive(false);


        puzzle = _puzzle;
        puzzle.gameObject.SetActive(true);


        if (puzzle.pieces == null)
            puzzle.Prepare();

        if (!PlayerPrefs.HasKey(puzzle.name + "_Positions") || !puzzle.enablePositionSaving)
            if (!invertRules)
                puzzle.DecomposePuzzle();
            else
                puzzle.NonrandomPuzzle();


        puzzle.invertedRules = invertRules;

        gameFinished = false;

        return true;
    }

    //-----------------------------------------------------------------------------------------------------	 
    // Show background (assembled puzzle)
    void ShowBackground()
    {
        if (background && backgroundColor.a < 1)
        {
            backgroundColor.a = Mathf.Lerp(backgroundColor.a, 1.0f, Time.deltaTime);
            background.material.color = backgroundColor;
        }

    }

    //-----------------------------------------------------------------------------------------------------	 
    // Prepare background (assembled puzzle)
    void PrepareBackground(Renderer _background)
    {
        if (_background)
        {
            if (background)
                background.gameObject.SetActive(false);

            background = _background;
            background.gameObject.SetActive(true);

            backgroundColor = background.material.color;

            if (backgroundTransparency < 1.0f)
            {
                backgroundColor.a = backgroundTransparency;
                background.material.color = backgroundColor;
            }

            AdjustBackground();
        }
        else
            background = null;

    }

    //-----------------------------------------------------------------------------------------------------	
    // Adjust background to puzzle
    void AdjustBackground()
    {
        if (background && background.transform.parent != puzzle.transform)
        {
            background.transform.gameObject.SetActive(true);
            background.transform.parent = puzzle.transform;


            // Try to adjust background size according to puzzle bounds
            if (adjustBackground && (background as SpriteRenderer).sprite)
            {
                // Temporarily reset Puzzle rotation 
                Quaternion tmpRotation = puzzle.transform.rotation;
                puzzle.transform.localRotation = Quaternion.identity;

                // Reset background transform
                background.transform.localPosition = new Vector3(0, 0, 0.2f);
                background.transform.localRotation = Quaternion.identity;
                background.transform.localScale = Vector3.one;

                // Calculate background scale  to make it the same size as puzzle
                background.transform.localScale = new Vector3(puzzle.puzzleDefaultBounds.size.x / background.bounds.size.x, puzzle.puzzleDefaultBounds.size.y / background.bounds.size.y, background.transform.localScale.z);
                // Aligned background position
                background.transform.position = new Vector3(puzzle.puzzleDefaultBounds.min.x, puzzle.puzzleDefaultBounds.max.y, background.transform.position.z);


                // Shift background if it's origin not in LeftTop corner 		 			 	
                if (Mathf.Abs(background.bounds.min.x - puzzle.puzzleDefaultBounds.min.x) > 1 || Mathf.Abs(background.bounds.max.y - puzzle.puzzleDefaultBounds.max.y) > 1)
                    background.transform.localPosition = new Vector3(background.transform.localPosition.x + background.bounds.extents.x, background.transform.localPosition.y - background.bounds.extents.y, background.transform.localPosition.z);

                // Return proprer puzzle rotation
                puzzle.transform.localRotation = tmpRotation;
            }
        }

    }

    //-----------------------------------------------------------------------------------------------------	 
    // Show Hint and update remainingHints
    public void ShowHint()
    {
        if (remainingHints != 0)
            puzzle.ReturnPiece(-1);

        if (remainingHints > 0)
            remainingHints--;

        if (hintCounterUI)
            hintCounterUI.text = remainingHints.ToString();
    }

    //-----------------------------------------------------------------------------------------------------	 
    // Process Timer
    void ProcessTimer()
    {
        if (timer > 0)
            if (timerTime < Time.time)
            { // Lose game if time is out

                if (loseUI)
                    loseUI.SetActive(true);

                gameFinished = true;
            }
            else
                if (timerCounterUI)
                timerCounterUI.text = string.Format("{0:0}:{1:00}", Mathf.Floor(-(Time.time - timerTime) / 60), -(Time.time - timerTime) % 60);

    }

    //-----------------------------------------------------------------------------------------------------	 
    // Pause game and show pauseUI
    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            if (pauseUI)
                pauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Confined;
            if (pauseUI)
                pauseUI.SetActive(false);
        }

    }


    //-----------------------------------------------------------------------------------------------------	 
    // Reset current puzzle
    public void ResetPuzzle()
    {
        if (puzzle == null)
            return;

        Time.timeScale = 0;

        puzzle.ResetProgress(puzzle.name);

        remainingHints = hintLimit;
        timerTime = Time.time + timer;

        PlayerPrefs.SetInt(puzzle.name + "_hints", hintLimit);
        PlayerPrefs.SetFloat(puzzle.name + "_timer", timer);

        if (hintCounterUI)
        {
            hintCounterUI.gameObject.SetActive(remainingHints > 0);
            hintCounterUI.text = remainingHints.ToString();
        }

        puzzle.DecomposePuzzle();

        Time.timeScale = 1.0f;
    }

    //-----------------------------------------------------------------------------------------------------	 
    // Restart current level
    public void Restart()
    {
        Time.timeScale = 1.0f;

        if (puzzle != null)
        {
            PlayerPrefs.SetString(puzzle.name, "");
            PlayerPrefs.DeleteKey(puzzle.name + "_Positions");
            PlayerPrefs.SetInt(puzzle.name + "_hints", hintLimit);
            PlayerPrefs.SetFloat(puzzle.name + "_timer", timer);
        }

        //SSSceneManager.Instance.LoadMenu("JigsawPuzzle");
        //SceneManager.LoadScene("JigsawPuzzle");

    }

    //-----------------------------------------------------------------------------------------------------	 
    // Load custom level
    public void LoadLevel(int _levelId)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(_levelId);

    }

    //-----------------------------------------------------------------------------------------------------	
    // Save progress (Assembled pieces)
    public void Save()
    {
        if (puzzle != null)
        {
            puzzle.SaveProgress(puzzle.name);
            PlayerPrefs.SetInt(puzzle.name + "_hints", remainingHints);
            PlayerPrefs.SetFloat(puzzle.name + "_timer", timerTime - Time.time);
        }

    }

    //-----------------------------------------------------------------------------------------------------	
    // Load puzzle (Assembled pieces)
    public void Load()
    {
        if (!puzzle)
            return;
        else
            puzzle.LoadProgress(puzzle.name);


        if (PlayerPrefs.HasKey(puzzle.name + "_hints"))
        {
            remainingHints = PlayerPrefs.GetInt(puzzle.name + "_hints");
            if (hintCounterUI)
                hintCounterUI.text = remainingHints.ToString();
        }
        else
        {
            Debug.Log("No saved data found for: " + puzzle.name + "_hints");
            remainingHints = hintLimit;
        }


        if (PlayerPrefs.HasKey(puzzle.name + "_timer"))
            remainingTime = PlayerPrefs.GetFloat(puzzle.name + "_timer");
        else
        {
            Debug.Log("No saved data found for: " + puzzle.name + "_timer");
            remainingTime = timer;
        }

    }

    /// <summary>
    /// hiển thị effect pháo hoa
    /// </summary>
    void SpawnEffect()
    {
        GameObject spawnedVFX = Instantiate(fx_firework, transform.position, transform.rotation) as GameObject;
        Destroy(spawnedVFX, 10f);
        GameObject spawnedVFX1 = Instantiate(fx_fireworkGreen, transform.position, transform.rotation) as GameObject;
        Destroy(spawnedVFX1, 10f);
        GameObject spawnedVFX2 = Instantiate(fx_fireworkRed, transform.position, transform.rotation) as GameObject;
        Destroy(spawnedVFX2, 10f);
    }

    //-----------------------------------------------------------------------------------------------------	
    // Save progress if player closes the application
    public void OnApplicationQuit()
    {
        Save();
        PlayerPrefs.Save();
    }

    public void onTest()
    {
        //List<int> list = GameManager.Instance.getPieceActive();
        //foreach(var item in list)
        //{
        //    Debug.Log("id " + item);
        //    Debug.Log("piece name " + puzzle.pieces[item].transform.gameObject.name);
        //}
    }    
    //-----------------------------------------------------------------------------------------------------	
}