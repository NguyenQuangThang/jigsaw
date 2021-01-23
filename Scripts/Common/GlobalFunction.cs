using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalFunction
{
    public static GameObject AddChild(GameObject parent, GameObject prefab)
    {
        return AddChild(parent, prefab, Vector3.zero);
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab, Vector3 pos)
    {
        return AddChild(parent, prefab, pos, prefab.transform.localRotation, prefab.transform.localScale);
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        GameObject addObj = GameObject.Instantiate(prefab) as GameObject;

        addObj.transform.SetParent(parent.transform);
        addObj.transform.localScale = scale; //Vector3.one;
        addObj.transform.localPosition = pos;
        addObj.transform.localRotation = rot;
        return addObj;
    }

    public static void DestroyAllChild(Transform t, bool immediate = false)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in t)
            children.Add(child.gameObject);

        children.ForEach(child =>
        {
            if (immediate)
                GameObject.DestroyImmediate(child);
            else
                GameObject.Destroy(child);
        });
    }

    public static Sprite GetSprite(string path, string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        List<string> names = new string[sprites.Length].ToList();
        for (var i = 0; i < names.Count; i++)
        {
            names[i] = sprites[i].name;
        }

        int numb = names.FindIndex(x => x == name);
        if (numb >= 0 && numb < sprites.Length)
            return sprites[numb];
        return null;
    }

}
