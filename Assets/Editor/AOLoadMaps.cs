using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AOLoadMaps : ScriptableWizard
{
    [SerializeField] AOTilemap tilemap = null;
    [SerializeField] int map = 1;

    private GrhData[] grhData;

    [MenuItem("CAO Tools/AO Map Reader")]
    static void SelectAllofTagwizard()
    {
        ScriptableWizard.DisplayWizard<AOLoadMaps>("AO Map Reader", "Load Grhs", "Load Map");
    }

    private void OnWizardCreate()
    {
        LoadGrhs();
    }

    private void OnWizardOtherButton()
    {
        LoadGrhs();

        //clear the map
        tilemap.tilemapLayer0.ClearAllTiles();
        tilemap.tilemapLayer1.ClearAllTiles();
        tilemap.tilemapLayer2.ClearAllTiles();
        tilemap.tilemapLayer3.ClearAllTiles();

        Dictionary<AOPosition, MapData> mapData = AoFileIO.LoadMaps(map);
        
        foreach (var pair in mapData)
        {
            int layerCounter = 0;
            foreach (var layer in pair.Value.graphic)
            {
                if (grhData[layer.grhIndex].NumFrames > 1 && !File.Exists("assets/Resources/AnimatedTiles/animTile_" + layer.grhIndex + ".asset"))
                {   
                    List<Sprite> sprites = new List<Sprite>();

                    for (int i = 0; i < grhData[layer.grhIndex].NumFrames; i++)
                    {
                        sprites.Add(GetSprite(grhData[layer.grhIndex].Frames[i], layerCounter));
                    }

                    AnimatedTile animatedTile = new AnimatedTile();
                    animatedTile.m_AnimatedSprites = sprites.ToArray();
                    animatedTile.m_MaxSpeed = 1000 * grhData[layer.grhIndex].NumFrames / grhData[layer.grhIndex].speed;

                    AssetDatabase.CreateAsset(animatedTile, "assets/Resources/AnimatedTiles/animTile_" + layer.grhIndex + ".asset");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            Vector3Int[] location = { pair.Key.MapPositionToVector3() };

            tilemap.tilemapLayer0.SetTiles(location, new TileBase[] { SetGraphicOnTile(pair, 0) });
            //tilemap.tilemapLayer0.SetTile(new Vector3Int((int)pair.Key.x, 100 - (int)pair.Key.y, 0), SetGraphicOnTile(pair, 0));

            if (pair.Value.graphic[1].grhIndex != 0)
            {
                tilemap.tilemapLayer1.SetTiles(location, new TileBase[] { SetGraphicOnTile(pair, 1) });
                //tilemap.tilemapLayer1.SetTile(new Vector3Int((int)pair.Key.x, 100 - (int)pair.Key.y, 10), SetGraphicOnTile(pair, 1));
            }

            if (pair.Value.graphic[2].grhIndex != 0)
            {
                tilemap.tilemapLayer2.SetTiles(location, new TileBase[] { SetGraphicOnTile(pair, 2) });
                //tilemap.tilemapLayer2.SetTile(new Vector3Int((int)pair.Key.x, 100 - (int)pair.Key.y, 20), SetGraphicOnTile(pair, 2));
            }

            if (pair.Value.graphic[3].grhIndex != 0)
            {
                tilemap.tilemapLayer3.SetTiles(location, new TileBase[] { SetGraphicOnTile(pair, 3) });
                //tilemap.tilemapLayer3.SetTile(new Vector3Int((int)pair.Key.x, 100 - (int)pair.Key.y, 30), SetGraphicOnTile(pair, 3));
            }
            


        }
    }

    private void LoadGrhs()
    {
        try
        {
            grhData = AoFileIO.LoadGrhs();

            if (grhData.Length == 0)
            {
                helpString = "No graphics has been loaded.";
                return;
            }

            helpString = grhData.Length + " graphics in memory.";
            isValid = true;
            Debug.Log("Loaded " + grhData.Length + " graphics.");
        }
        catch (System.InvalidOperationException ex)
        {
            Debug.LogError(ex.StackTrace);
            throw;
        }
    }

    private Sprite GetSprite(int index, int layer)
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Sprites/" + grhData[index].fileNum + ".png");

        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(texture));

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name.Equals(index.ToString()))
            {
                return (Sprite)sprites[i];

            }
        }

        Debug.LogWarning("Tried to get sprite " + index + " and failed.");
        return null;
    }

    private TileBase SetGraphicOnTile(System.Collections.Generic.KeyValuePair<AOPosition, MapData> pair, int layer)
    {
        Tile tempTile = ScriptableObject.CreateInstance<Tile>();

        if (grhData[pair.Value.graphic[layer].grhIndex].NumFrames > 1)
        {
            AnimatedTile animTile = AssetDatabase.LoadAssetAtPath<AnimatedTile>("assets/Resources/AnimatedTiles/animTile_" + pair.Value.graphic[layer].grhIndex + ".asset");
            return animTile;
        }
        else
        {
            tempTile.sprite = GetSprite(pair.Value.graphic[layer].grhIndex, layer);
        }

        return tempTile;
    }
}
