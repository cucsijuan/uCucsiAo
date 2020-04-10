using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AOGraphicsConverser : ScriptableWizard
{
    public int fromIndex = 1;
    public int MaxIndexToRead = 1;
    public bool CreateAnimations = true;

    private GrhData[] grhData;

    [MenuItem ("CAO Tools/AO Graphics Converser")]
    static void SelectAllofTagwizard()
    {
        ScriptableWizard.DisplayWizard<AOGraphicsConverser>("AO Graphics Converser", "Create Animations", "Load Graphics file");
    }


    private void OnWizardCreate()
    {
        Createanimations();
    }
  
    private void OnWizardOtherButton()
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

    private void OnWizardUpdate()
    {
        if (grhData != null)
        {
            helpString = grhData.Length + " graphics in memory.";
        }
        else
        {
            helpString = "No graphics has been loaded.";
        }
    }

    private void Createanimations()
    {
        int currentTextureName = 0;
        List<SpriteMetaData> newData = new List<SpriteMetaData>();
        TextureImporter ti = new TextureImporter();
        string path = "";
        Texture2D myTexture = null;

        MaxIndexToRead = MaxIndexToRead > 0 ? fromIndex + MaxIndexToRead + 1 : grhData.Length;

        for (int i = fromIndex; i < MaxIndexToRead; i++)
        {
            helpString = "Loading index: " + i;

            if (grhData[i].fileNum == 0 && grhData[i].NumFrames <= 1)
            {
                continue;
            }


            if (grhData[i].NumFrames > 1)
            {

                if (CreateAnimations == true) CreateAnims(grhData[i].Frames, i);
                continue;
                
            }
            else if (currentTextureName != grhData[i].fileNum)
            {
                //add slices to previous texture
                if (currentTextureName != 0)
                {
                    ti.spritesheet = newData.ToArray();
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }

                //Load new texture
                currentTextureName = grhData[i].fileNum;

                myTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Sprites/" + currentTextureName + ".png");
                path = AssetDatabase.GetAssetPath(myTexture);

                if(myTexture == null)
                {
                    continue;
                }

                try
                {
                    ti = AssetImporter.GetAtPath(path) as TextureImporter;
                    ti.isReadable = true;
                }
                catch (System.NullReferenceException ex)
                {
                    Debug.Log("Null reference while slicing " + currentTextureName + " Index:" + i + " Path: " + path +". " + ex.StackTrace);
                    throw;
                }
                

                newData = new List<SpriteMetaData>();
            }
            
            int SliceWidth = grhData[i].pixelWidth;
            int SliceHeight = grhData[i].pixelHeight;

            
            SpriteMetaData smd = new SpriteMetaData();

            float alignY = 0.5f;
            float alignX = 0.5f;

            if (grhData[i].TileHeight> 1)
            {
                alignY = 16/(float)grhData[i].pixelHeight;
            }

            if (grhData[i].tileWidth > 1)
            {
                alignX = 0.5f;
            }
            
            smd.pivot = new Vector2(alignX, alignY);
            smd.alignment = 9;
            smd.name = i.ToString();

            int sliceX = grhData[i].sX;
            int sliceY = myTexture.height - SliceHeight - grhData[i].sY;

            smd.rect = new Rect(sliceX, sliceY, SliceWidth, SliceHeight);

             newData.Add(smd);

        }


        try
        {
            //add slices and save last texture
            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        catch (System.NullReferenceException ex)
        {
            Debug.Log("Null reference while slicing " + currentTextureName + " Path: " + path + ". " + ex.StackTrace);
            throw;
        }

        

        helpString = "Slicing Done";
    }

    private void CreateAnims(int[] frames, int index)
    {
        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[frames.Length];
        AnimationClip animClip = new AnimationClip();
        EditorCurveBinding spriteBinding = new EditorCurveBinding();

        animClip.frameRate = 1000 * grhData[index].NumFrames / grhData[index].speed;   // FPS      
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";
        
        int currentKeyframe = 0;

        for (int b = 0; b < frames.Length; b++)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Sprites/" + grhData[frames[b]].fileNum + ".png");

            /*Sprite[] sprites = Resources.LoadAll<Sprite>(texture.name);*/
            Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(texture));
            for (int i = 0; i < (sprites.Length); i++)
            {
                if (sprites[i].name.Equals(frames[b].ToString()))
                {
                    spriteKeyFrames[currentKeyframe] = new ObjectReferenceKeyframe();
                    spriteKeyFrames[currentKeyframe].time = currentKeyframe / animClip.frameRate;
                    spriteKeyFrames[currentKeyframe].value = (Sprite)sprites[i];
                    
                    currentKeyframe++;
                    break;
                }
 
            }
        }

        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);
        AssetDatabase.CreateAsset(animClip, "assets/Animations/" + index + ".anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

}
