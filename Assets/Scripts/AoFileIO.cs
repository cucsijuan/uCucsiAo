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

                    if (grhData[grh].NumFrames <= 0) throw new System.InvalidOperationException("Tried to read invalid data " + grh);

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

    public static HeadData[] LoadHeads()
    {
        string fileName = Application.dataPath + "/Resources/Data/Cabezas.ind"; //TODO: hardcoded for more pleasure.

        return LoadHeadData(fileName);
    }

    public static HeadData[] LoadHelmets()
    {
        string fileName = Application.dataPath + "/Resources/Data/Cascos.ind"; //TODO: hardcoded for more pleasure.

        return LoadHeadData(fileName);
    }

    public static HeadData[] LoadHeadData(string path)
    {
        

        List<HeadData> headsData = new List<HeadData>();

        if (File.Exists(path))
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                string desc = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(255));
                int CRC = reader.ReadInt32();
                int MagicWord = reader.ReadInt32();

                int numHeads = reader.ReadInt16();

               
                for (int i = 0; i < numHeads; i++)
                {
                    int[] heads = new int[4];
                    for (int a = 0; a < heads.Length; a++)
                    {
                        heads[a] = reader.ReadInt32();
                    }

                    HeadData tempData;
                    tempData.Heads = new Grh[4];
                    tempData.Heads[0].grhIndex = heads[0];
                    tempData.Heads[1].grhIndex = heads[1];
                    tempData.Heads[2].grhIndex = heads[2];
                    tempData.Heads[3].grhIndex = heads[3];
                    headsData.Add(tempData);
                }
            }

        }

        return headsData.ToArray();
    }

    public static BodyData[] LoadBodies()
    {
        string fileName = Application.dataPath + "/Resources/Data/Personajes.ind"; //TODO: hardcoded for more pleasure.

        List<BodyData> headsData = new List<BodyData>();

        if (!File.Exists(fileName))
        {
            Debug.LogError("LoadBodies: " + fileName + " not found.");
            return headsData.ToArray();
        }

        int currentIndex = 0;

        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                string desc = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(255));
                int CRC = reader.ReadInt32();
                int MagicWord = reader.ReadInt32();

                int numHeads = reader.ReadInt16();

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    int[] bodies = new int[4];

                    for (int a = 0; a < bodies.Length; a++)
                    {
                        bodies[a] = reader.ReadInt32();
                    }

                    short HeadOffsetX = reader.ReadInt16();
                    short HeadOffsetY = reader.ReadInt16();

                    BodyData tempData;
                    tempData.Bodies = new Grh[4];
                    tempData.Bodies[0].grhIndex = bodies[0];
                    tempData.Bodies[1].grhIndex = bodies[1];
                    tempData.Bodies[2].grhIndex = bodies[2];
                    tempData.Bodies[3].grhIndex = bodies[3];

                    tempData.HeadOffset = new Vector2(HeadOffsetX, HeadOffsetY);

                    headsData.Add(tempData);

                    currentIndex++;
                }
            }
        }
        catch (EndOfStreamException ex)
        {
            Debug.LogError(" LoadBodies: EOF reach when reading index: " + currentIndex + " Stack: " + ex.StackTrace );
            throw;
        }

        Debug.Log(" LoadBodies: Loaded: " + (currentIndex + 1) + " Bodies ");
        return headsData.ToArray();
    }

    public static WeaponAnimData[] LoadWeaponAnims()
    {
        string fileName = Application.dataPath + "/Resources/Data/armas.dat"; //TODO: hardcoded for more pleasure.

        List<WeaponAnimData> weaponsAnimData = new List<WeaponAnimData>();

        if (File.Exists(fileName))
        {
            // Read file using StreamReader. Reads file line by line  
            using (StreamReader reader = new StreamReader(fileName))
            {
                int weaponsNum = 0;

                string str = reader.ReadLine();

                if (str[0] == '[')
                {
                    char[] trimChars = { '[', ']' };
                    str = str.Trim(trimChars);

                    if (str.ToUpper() == "INIT")
                    {
                        str = reader.ReadLine();
                        str = str.Remove(0, str.IndexOf('=')+1);
                        weaponsNum = int.Parse(str);
                    }

                    

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        str = reader.ReadLine();

                        if (str.Length > 0 && str[0] != '[' && str[0] != '\'')
                        {
                            WeaponAnimData tempData;
                            tempData.WeaponAnims = new Grh[4];

                            for (int i = 0; i < 4; i++)
                            {
                                char[] splitChar = { '=' };
                                string[] strSplit = str.Split(splitChar);

                                strSplit[1] = strSplit[1].Split(' ')[0];

                                switch (strSplit[0])
                                {
                                    case "Dir1":
                                        tempData.WeaponAnims[0].grhIndex = int.Parse(strSplit[1]);
                                        break;
                                    case "Dir2":
                                        tempData.WeaponAnims[1].grhIndex = int.Parse(strSplit[1]);
                                        break;
                                    case "Dir3":
                                        tempData.WeaponAnims[2].grhIndex = int.Parse(strSplit[1]);
                                        break;
                                    case "Dir4":
                                        tempData.WeaponAnims[3].grhIndex = int.Parse(strSplit[1]);
                                        break;
                                    default:
                                        break;
                                }

                                str = reader.ReadLine();
                            }
           
                            weaponsAnimData.Add(tempData);
                            
                        }

                    }

                }
            }
        }

        return weaponsAnimData.ToArray();
    }

    public static ShieldAnimData[] LoadShieldAnims()
    {
        string fileName = Application.dataPath + "/Resources/Data/escudos.dat"; //TODO: hardcoded for more pleasure.

        List<ShieldAnimData> shieldAnimData = new List<ShieldAnimData>();

        if (!File.Exists(fileName))
        {
            Debug.LogError("LoadShieldAnims: " + fileName + " not found.");
            return shieldAnimData.ToArray();
        }

        // Read file using StreamReader. Reads file line by line  
        using (StreamReader reader = new StreamReader(fileName))
        {
            int shieldsNum = 0;

            string str = reader.ReadLine();

            if (str[0] == '[')
            {
                char[] trimChars = { '[', ']' };
                str = str.Trim(trimChars);

                if (str.ToUpper() == "INIT")
                {
                    str = reader.ReadLine();
                    str = str.Remove(0, str.IndexOf('=') + 1);
                    shieldsNum = int.Parse(str);
                }



                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    str = reader.ReadLine();

                    if (str.Length > 0 && str[0] != '[' && str[0] != '\'')
                    {
                        ShieldAnimData tempData;
                        tempData.ShieldAnims = new Grh[4];

                        for (int i = 0; i < 4; i++)
                        {
                            char[] splitChar = { '=' };
                            string[] strSplit = str.Split(splitChar);

                            strSplit[1] = strSplit[1].Split(' ')[0];

                            switch (strSplit[0])
                            {
                                case "Dir1":
                                    tempData.ShieldAnims[0].grhIndex = int.Parse(strSplit[1]);
                                    break;
                                case "Dir2":
                                    tempData.ShieldAnims[1].grhIndex = int.Parse(strSplit[1]);
                                    break;
                                case "Dir3":
                                    tempData.ShieldAnims[2].grhIndex = int.Parse(strSplit[1]);
                                    break;
                                case "Dir4":
                                    tempData.ShieldAnims[3].grhIndex = int.Parse(strSplit[1]);
                                    break;
                                default:
                                    break;
                            }

                            str = reader.ReadLine();
                        }

                        shieldAnimData.Add(tempData);

                    }

                }

            }
            
        }

        return shieldAnimData.ToArray();
    }

}



