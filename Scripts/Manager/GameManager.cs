using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public int rows = 2;
    public int cols = 2;

    public int widthPiece;
    public int heightPiece;

    public Sprite[] spritesClassic;
    public Sprite[] spritesFashion;
    public Sprite[] spritesFreeStyle;
    public Sprite[] spritesGhost;
    public Sprite[] spritesHalloween;
    public Sprite[] spritesKute;
    public Sprite[] spritesLol;
    public Sprite[] spritesNoel;


    public ItemTheme themeCurent;
    public Sprite spriteCurrent;
    public bool openChanged = false;
    public int levelCurrent = 1;
    public int numberPiece = 0;
    public int numMaxPiece = 0;
    public int countPiece = 0;



    public List<int> listIdPiece = new List<int>();
    public void OnSwitchPieces(int mode)
    {
        switch (mode)
        {
            case 4:
                rows = 2;
                cols = 2;
                break;
            case 6:
                rows = 2;
                cols = 3;
                break;
            case 9:
                rows = 3;
                cols = 3;
                break;
            case 16:
                rows = 4;
                cols = 4;
                break;
            case 25:
                rows = 5;
                cols = 5;
                break;
            case 36:
                rows = 6;
                cols = 6;
                break;
        }
    }

    public void getListClassic()
    {
        if (spritesClassic == null || spritesClassic.Length == 0)
        {
            string path = "Icon/Classic";
            spritesClassic = Resources.LoadAll<Sprite>(path);
        }
    }

    public Sprite[] getListImgeByTheme(ItemTheme typeTheme)
    {
        if (typeTheme.type == TypeTheme.Classic)
        {
            if (spritesClassic == null)
            {
                string path = "Icon/Classic";
                spritesClassic = Resources.LoadAll<Sprite>(path);
            }
            return spritesClassic;
        }
        else if (typeTheme.type == TypeTheme.Fashion)
        {
            if (spritesFashion == null)
            {
                string path = "Icon/Fashion";
                spritesFashion = Resources.LoadAll<Sprite>(path);
            }
            return spritesFashion;
        }
        else if (typeTheme.type == TypeTheme.FreeStyle)
        {
            if (spritesFreeStyle == null)
            {
                string path = "Icon/FreeStyle";
                spritesFreeStyle = Resources.LoadAll<Sprite>(path);
            }
            return spritesFreeStyle;
        }
        else if (typeTheme.type == TypeTheme.Ghost)
        {
            if (spritesGhost == null)
            {
                string path = "Icon/Ghosts";
                spritesGhost = Resources.LoadAll<Sprite>(path);
            }
            return spritesGhost;
        }
        else if (typeTheme.type == TypeTheme.Halloween)
        {
            if (spritesHalloween == null)
            {
                string path = "Icon/Halloween";
                spritesHalloween = Resources.LoadAll<Sprite>(path);
            }
            return spritesHalloween;
        }
        else if (typeTheme.type == TypeTheme.Kute)
        {
            if (spritesKute == null)
            {
                string path = "Icon/Kute";
                spritesKute = Resources.LoadAll<Sprite>(path);
            }
            return spritesKute;
        }
        else if (typeTheme.type == TypeTheme.Lol)
        {
            if (spritesLol == null)
            {
                string path = "Icon/LOL";
                spritesLol = Resources.LoadAll<Sprite>(path);
            }
            return spritesLol;
        }
        else if (typeTheme.type == TypeTheme.Noel)
        {
            if (spritesNoel == null)
            {
                string path = "Icon/Noel";
                spritesNoel = Resources.LoadAll<Sprite>(path);
            }
            return spritesNoel;
        }
        else return null;
    }

    public Texture2D getTextureJigsaw()
    {
        string url = "Data/" + themeCurent.nameTheme + "/" + spriteCurrent.name;
        string[] c = spriteCurrent.name.Split('_');
        levelCurrent = Int32.Parse(c[1]);
        Texture2D texture = Resources.Load<Texture2D>(url);
        return texture;
    }

    public Texture2D getNextTexture()
    {
        string url = "Data/" + themeCurent.nameTheme + "/" + themeCurent.nameTheme + "_" + (levelCurrent + 1);
        Texture2D texture = Resources.Load<Texture2D>(url);
        if (texture == null)
        {
            levelCurrent = 1;
            url = "Data/" + themeCurent.nameTheme + "/" + themeCurent.nameTheme + "_" + (1);
            texture = Resources.Load<Texture2D>(url);
        }
        else
            levelCurrent += 1;

        return texture;

    }

    /// <summary>
    /// r
    /// </summary>
    Vector3 v3;
    public Vector3 getSizePieceFake()
    {
        switch (rows)
        {
            case 2:
                v3 = new Vector3(0.2f, 0.3f, 0.2f);
                break;
            case 3:
                v3 = new Vector3(0.3f, 0.4f, 0.3f);
                break;
            case 4:
                v3 = new Vector3(0.4f, 0.6f, 0.2f);
                break;
            case 5:
                v3 = new Vector3(0.4f, 0.6f, 0.2f);
                break;
            case 6:
                v3 = new Vector3(0.5f, 0.7f, 0.2f);
                break;
        }
        if(rows == 2 && cols == 3)
            v3 = new Vector3(0.3f, 0.3f, 0.3f);
        return v3;
    }
    //public List<int> getPieceActive()
    //{
    //    List<int> listActive = new List<int>();
    //    if (listIdPiece.Count > 4)
    //    {
    //        for (int i = 0; i < 4; i++)
    //        {
    //            listActive.Add(listIdPiece[i]);
    //            listIdPiece.RemoveAt(i);
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < listIdPiece.Count; i++)
    //        {
    //            listActive.Add(listIdPiece[i]);
    //            listIdPiece.RemoveAt(i);
    //        }
    //    }
    //    return listActive;
    //}
}
