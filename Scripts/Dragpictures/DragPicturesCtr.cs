using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragPicturesCtr : MonoBehaviour
{
    private int pointsToWin;
    private int currentPoints;
    public GameObject mySwords;
    public GameObject winPopup;

    // Start is called before the first frame update
    void Start()
    {
        pointsToWin = mySwords.transform.childCount;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentPoints >= pointsToWin)
        {
            winPopup.SetActive(true);
        }
    }
    public void AddPoints()
    {
        currentPoints++;
    }

    public void onRestart()
    {
        SceneManager.LoadScene("DragPictures");
    }
}
