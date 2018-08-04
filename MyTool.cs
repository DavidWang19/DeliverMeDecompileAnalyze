using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class MyTool
{
    private static bool debugData;

    public static string[] GetArrayByFirstElement(string[][] arrayCollection, string firstElement)
    {
        for (int i = 0; i < arrayCollection.Length; i++)
        {
            if (arrayCollection[i][0] == firstElement)
            {
                return arrayCollection[i];
            }
        }
        return new string[0];
    }

    private static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100f, SpriteMeshType spriteType = 0)
    {
        Sprite sprite = new Sprite();
        Texture2D textured = LoadTexture(FilePath);
        return Sprite.Create(textured, new Rect(0f, 0f, (float) textured.get_width(), (float) textured.get_height()), new Vector2(0f, 0f), PixelsPerUnit, 0, spriteType);
    }

    public static Sprite LoadSprite(string path)
    {
        Sprite sprite = new Sprite();
        if (debugData)
        {
            if (File.Exists(Application.get_dataPath() + "/" + path + ".png"))
            {
                sprite = LoadNewSprite(Application.get_dataPath() + "/" + path + ".png", 100f, 0);
            }
            return sprite;
        }
        return Resources.Load<Sprite>(path);
    }

    public static string LoadText(string path)
    {
        string str = string.Empty;
        if (debugData)
        {
            if (File.Exists(Application.get_dataPath() + "/" + path + ".txt"))
            {
                StreamReader reader = new StreamReader(Application.get_dataPath() + "/" + path + ".txt");
                str = reader.ReadToEnd();
                reader.Close();
            }
            return str;
        }
        return ((TextAsset) Resources.Load(path)).get_text();
    }

    private static Texture2D LoadTexture(string FilePath)
    {
        if (File.Exists(FilePath))
        {
            byte[] buffer = File.ReadAllBytes(FilePath);
            Texture2D textured = new Texture2D(2, 2);
            if (ImageConversion.LoadImage(textured, buffer))
            {
                return textured;
            }
        }
        return null;
    }

    public static bool ParseBool(string text, bool defaultValue = true)
    {
        bool flag;
        if (!bool.TryParse(text, out flag))
        {
            flag = defaultValue;
        }
        return flag;
    }

    public static float ParseFloat(string text, float defaultValue = -1f)
    {
        float num;
        if (!float.TryParse(text, out num))
        {
            num = defaultValue;
        }
        return num;
    }

    public static int ParseInt(string text, int defaultValue = -1)
    {
        int num;
        if (!int.TryParse(text, out num))
        {
            num = defaultValue;
        }
        return num;
    }

    public static string[][] ParseTSV(string tsvText)
    {
        string[] separator = new string[] { "\r\n", Environment.NewLine, "\n" };
        string[] strArray2 = tsvText.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        string[][] strArray3 = new string[strArray2.Length][];
        separator = new string[] { "\t" };
        for (int i = 0; i < strArray3.Length; i++)
        {
            strArray3[i] = strArray2[i].Split(separator, StringSplitOptions.None);
        }
        return strArray3;
    }
}

