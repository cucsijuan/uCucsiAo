using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AoFileIO
{
    public static GrhData[] LoadGrhs()
    {
        var readTimeWatch = System.Diagnostics.Stopwatch.StartNew();

        string fileName = Application.dataPath + "/Data/graficos.ind"; //TODO: hardcoded for more pleasure.

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
}
