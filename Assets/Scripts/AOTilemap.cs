using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AOTilemap : MonoBehaviour
{
    [SerializeField] public Tilemap tilemapLayer0;
    [SerializeField] public Tilemap tilemapLayer1;
    [SerializeField] public Tilemap tilemapLayer2;
    [SerializeField] public Tilemap tilemapLayer3;

    private AOGameManager GM;

    void Awake()
    {
        GM = AOGameManager.Instance;
    }

    public void LoadMap(int MapNumber)
    {
        //clear the map
        tilemapLayer0.ClearAllTiles();
        tilemapLayer1.ClearAllTiles();
        tilemapLayer2.ClearAllTiles();
        tilemapLayer3.ClearAllTiles();

        GM.mapData = AoFileIO.LoadMaps(MapNumber);

        foreach (var pair in GM.mapData)
        {
            /*tilemapLayer0.SetTile(new Vector3Int((int)pair.Key.x, 100 - (int)pair.Key.y, 0), SetGraphicOnTile(pair, 0));

            if (pair.Value.graphic[1].grhIndex != 0)
            {
                tilemapLayer1.SetTile(new Vector3Int((int)pair.Key.x, 100 - (int)pair.Key.y, 0), SetGraphicOnTile(pair, 1));
            }

            if (pair.Value.graphic[2].grhIndex != 0)
            {
                tilemapLayer2.SetTile(new Vector3Int((int)pair.Key.x, 100 - (int)pair.Key.y, 0), SetGraphicOnTile(pair, 2));
            }

            if (pair.Value.graphic[3].grhIndex != 0)
            {
                tilemapLayer3.SetTile(new Vector3Int((int)pair.Key.x, 100 - (int)pair.Key.y, 0), SetGraphicOnTile(pair, 3));
            }*/

            Vector3Int[] location = { pair.Key.MapPositionToVector3() };

            tilemapLayer0.SetTiles(location, new TileBase[] { SetGraphicOnTile(pair, 0) });

            if (pair.Value.graphic[1].grhIndex != 0)
            {
                tilemapLayer1.SetTiles(location, new TileBase[] { SetGraphicOnTile(pair, 1) });
               
            }

            if (pair.Value.graphic[2].grhIndex != 0)
            {
                tilemapLayer2.SetTiles(location, new TileBase[] { SetGraphicOnTile(pair, 2) });
                
            }

            if (pair.Value.graphic[3].grhIndex != 0)
            {
                tilemapLayer2.SetTiles(location, new TileBase[] { SetGraphicOnTile(pair, 3) });
               
            }
        }
    }

    private TileBase SetGraphicOnTile(System.Collections.Generic.KeyValuePair<AOPosition, MapData> pair, int layer)
    {
        if (AOGameManager.Instance.grhData[pair.Value.graphic[layer].grhIndex].NumFrames > 1)
        {
           return Resources.Load<AnimatedTile>("Animatedtiles/animTile_" + pair.Value.graphic[layer].grhIndex);
        }

        Tile tempTile = ScriptableObject.CreateInstance<Tile>();

        tempTile.sprite = AOGameManager.Instance.spriteCache.GetSprite(pair.Value.graphic[layer].grhIndex);

        return tempTile;
    }
}

