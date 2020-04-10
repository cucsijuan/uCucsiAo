using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteProcessor : AssetPostprocessor
{
    void OnPostprocessTexture(Texture2D texture)
    {
        string lowerCaseAssetPath = assetPath.ToLower();
        bool isInSpritesdirectory = lowerCaseAssetPath.IndexOf("/resources/sprites/") != -1;

        if (isInSpritesdirectory)
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            textureImporter.filterMode = FilterMode.Point;
            textureImporter.spritePixelsPerUnit = 32;



            UnityEngine.Object[] texArr = { texture };

            //RemoveColor(Color.black, texArr);
        }
    }

    // for multiple images
    void RemoveColor(Color c, UnityEngine.Object[] imgs, float tolerance = 0.06f)
    {
        if (!Directory.Exists("Assets/AlphaImages/"))
        {
            Directory.CreateDirectory("Assets/AlphaImages/");
        }

        float inc = 0f;
        foreach (Texture2D i in imgs)
        {
            inc++;
            if (inc % 512 == 0 && EditorUtility.DisplayCancelableProgressBar("Playin' With Pixels", "Seaching for Color Matches", ((float)inc / (float)imgs.Length)))
            {
                Debug.LogError("Cancelled..");
                break;
            }

            Color[] pixels = i.GetPixels(0, 0, i.width, i.height, 0);
            var clear = new Color(0, 0, 0, 0);

            for (int p = 0; p < pixels.Length; p++)
            {

                if (Mathf.Abs(pixels[p].r - c.r) < tolerance && Mathf.Abs(pixels[p].g - c.g) < tolerance && Mathf.Abs(pixels[p].b - c.b) < tolerance && Mathf.Abs(pixels[p].a - c.a) < tolerance)
                {
                    pixels[p] = clear;
                }
            }

            Texture2D n = new Texture2D(i.width, i.height);
            n.SetPixels(0, 0, i.width, i.height, pixels, 0);
            n.Apply();

            byte[] bytes = n.EncodeToPNG();
            File.WriteAllBytes("Assets/AlphaImages/" + i.name + ".png", bytes);
        }

        EditorUtility.ClearProgressBar();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    Texture2D RemoveColor(Color c, Texture2D i, float tolerance = 0.06f)
    {

        Color[] pixels = i.GetPixels(0, 0, i.width, i.height, 0);

        var clear = new Color(0, 0, 0, 0);

        for (int p = 0; p < pixels.Length; p++)
        {
            if (p % 512 == 0 && EditorUtility.DisplayCancelableProgressBar("Playin' With Pixels", "Seaching for Color Matches", ((float)p / pixels.Length)))
            {
                Debug.LogError("Cancelled..");
                break;
            }

            if (Mathf.Abs(pixels[p].r - c.r) < tolerance && Mathf.Abs(pixels[p].g - c.g) < tolerance && Mathf.Abs(pixels[p].b - c.b) < tolerance && Mathf.Abs(pixels[p].a - c.a) < tolerance)
            {
                pixels[p] = clear;
            }

        }

        Texture2D n = new Texture2D(i.width, i.height);
        n.SetPixels(0, 0, i.width, i.height, pixels, 0);
        n.Apply();
        EditorUtility.ClearProgressBar();
        return (n);
    }
}



