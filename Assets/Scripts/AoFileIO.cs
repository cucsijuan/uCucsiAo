using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AoFileIO
{
    public static GrhData[] LoadGrhs()
    {
        var readTimeWatch = System.Diagnostics.Stopwatch.StartNew();

        string fileName = Application.dataPath + "/Resources/Data/graficos.ind"; //TODO: hardcoded for more pleasure. need error handling

        GrhData[] grhData = new GrhData[0];

        if (File.Exists(fileName))
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                int fileVersion = reader.ReadInt32();
                int grhCount = reader.ReadInt32();

                grhData = new GrhData[grhCount + 1];

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    int grh = reader.ReadInt32();

                    grhData[grh].NumFrames = reader.ReadInt16();

                    if (grhData[grh].NumFrames <= 0)  throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                    if (grhData[grh].NumFrames > 1)
                    {
                        grhData[grh].Frames = new int[grhData[grh].NumFrames];

                        for (int frame = 0; frame < grhData[grh].NumFrames; frame++)
                        {
                            grhData[grh].Frames[frame] = reader.ReadInt32();

                            if (grhData[grh].Frames[frame] <= 0 || grhData[grh].Frames[frame] > grhCount) throw new System.InvalidOperationException("Tried to read invalid data " + grh);
                        }

                        grhData[grh].speed = reader.ReadSingle();
                        if (grhData[grh].speed <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].pixelHeight = grhData[grhData[grh].Frames[0]].pixelHeight;
                        if (grhData[grh].pixelHeight <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].pixelWidth = grhData[grhData[grh].Frames[0]].pixelWidth;
                        if (grhData[grh].pixelWidth <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].tileWidth = grhData[grhData[grh].Frames[0]].tileWidth;
                        if (grhData[grh].tileWidth <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].TileHeight = grhData[grhData[grh].Frames[0]].TileHeight;
                        if (grhData[grh].TileHeight <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);
                    }
                    else
                    {
                        grhData[grh].fileNum = reader.ReadInt32();
                        if (grhData[grh].fileNum <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].sX = reader.ReadInt16();
                        if (grhData[grh].sX < 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].sY = reader.ReadInt16();
                        if (grhData[grh].sY < 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].pixelWidth = reader.ReadInt16();
                        if (grhData[grh].pixelWidth <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].pixelHeight = reader.ReadInt16();
                        if (grhData[grh].pixelHeight <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

                        grhData[grh].tileWidth = (float)System.Math.Ceiling((double)grhData[grh].pixelWidth / 32); //TODO: make global constant
                        grhData[grh].TileHeight = (float)System.Math.Ceiling((double)grhData[grh].pixelHeight / 32);
                    }
                }

            }
        }

        readTimeWatch.Stop();
        Debug.Log("Finished loading graphics. Took:" + readTimeWatch.ElapsedMilliseconds + " ms");

        return grhData;
    }

    public static Dictionary<AOPosition, MapData> LoadMaps(int map)
    {
        var readTimeWatch = System.Diagnostics.Stopwatch.StartNew();
        byte byFlags;

        string fileName = Application.dataPath + "/Resources/Data/Maps/Mapa" + map + ".map"; //TODO: hardcoded for more pleasure.

        Dictionary<AOPosition, MapData> mapData = new Dictionary<AOPosition, MapData>();

        if (File.Exists(fileName))
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                int mapVersion = reader.ReadInt16();

                string desc = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(255));
                int CRC = reader.ReadInt32();
                int MagicWord = reader.ReadInt32();

                reader.ReadDouble();

                for (int y = 0; y < 100; y++)
                {
                    for (int x = 0; x < 100; x++)
                    {
                        byFlags = reader.ReadByte();

                        AOPosition pos = new AOPosition(x, y);
                        MapData tempData = new MapData();

                        tempData.graphic = new Grh[4];

                        tempData.blocked = (byte)(byFlags & 1);
                        tempData.graphic[0].grhIndex = reader.ReadInt32();
                        //TODO: llama a InitGrh aca

                        if ((byFlags & 2) != 0)
                        {
                            tempData.graphic[1].grhIndex = reader.ReadInt32();
                            //TODO: llama a InitGrh aca
                        }
                        else
                        {
                            tempData.graphic[1].grhIndex = 0;
                        }

                        if ((byFlags & 4) != 0)
                        {
                            tempData.graphic[2].grhIndex = reader.ReadInt32();
                            //TODO: llama a InitGrh aca
                        }
                        else
                        {
                            tempData.graphic[2].grhIndex = 0;
                        }

                        if ((byFlags & 8) != 0)
                        {
                            tempData.graphic[3].grhIndex = reader.ReadInt32();
                            //TODO: llama a InitGrh aca
                        }
                        else
                        {
                            tempData.graphic[3].grhIndex = 0;
                        }

                        if ((byFlags & 16) != 0)
                        {
                            tempData.trigger = reader.ReadInt16();
                            //TODO: llama a InitGrh aca
                        }

                        if (tempData.charIndex > 0)
                        {
                            tempData.charIndex = 0;
                        }

                        //TODO: descomentar cuando implemente objetos
                        /*if (tempData.objIndex > 0)
                        {
                            tempData.objIndex = 0;
                        }*/

                        mapData.Add(pos, tempData);


                    }
                }
            }
        }

        return mapData;
    }

}
