using UnityEngine;

public static class SaveManager 
{
    public static int GetLevel()
    {
        return PlayerPrefs.GetInt("level", 1);
    }

    public static void SetLevel(int lv)
    {
        Debug.Log("Level setted in " + lv.ToString());
        PlayerPrefs.SetInt("level", lv);
    }
}
