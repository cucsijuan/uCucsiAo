using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class PaperdollLoader : EditorWindow
{
    public static PaperdollLoader win;

    string _Statuslabel = "GrhData not loaded.";

    GrhData[] grhData;

    [MenuItem("CAO Tools/Paperdoll Loader")]
    static void Init()
    {
        win = ScriptableObject.CreateInstance(typeof(PaperdollLoader)) as PaperdollLoader;
        win.minSize = new Vector2(300, 350);
        win.ShowUtility();


    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Label(_Statuslabel, GUILayout.MinWidth(128), GUILayout.MinHeight(32), GUILayout.MaxWidth(128), GUILayout.MaxHeight(32));

        if (GUILayout.Button("Load heads", GUILayout.MinWidth(128), GUILayout.MinHeight(32), GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
            LoadHeads();

        if (GUILayout.Button("Create idle body anims", GUILayout.MinWidth(128), GUILayout.MinHeight(32), GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
            CreateIdleBodyAnimations();

        if (GUILayout.Button("Create idle Weapon anims", GUILayout.MinWidth(128), GUILayout.MinHeight(32), GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
            CreateIdleAWeaponAnimations();

        if (GUILayout.Button("Create heading anims", GUILayout.MinWidth(128), GUILayout.MinHeight(32), GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
            CreateHeadingAnimations();

        if (GUILayout.Button("Create helmet anims", GUILayout.MinWidth(128), GUILayout.MinHeight(32), GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
            CreateHelmetAnimations();

        GUILayout.EndVertical();
    }

    private void LoadHeads()
    {
        var LoadedHeads = AoFileIO.LoadHeads();

        Debug.Log("Loaded: " + LoadedHeads.Length + " Heads");
    }

    private void LoadGrhData()
    {
        try
        {
            grhData = AoFileIO.LoadGrhs();

            if (grhData == null || grhData.Length == 0)
            {
                _Statuslabel = "Error: No graphics has been loaded.";
                return;
            }

            _Statuslabel = grhData.Length + "loaded graphics in memory.";
            Debug.Log("Loaded " + grhData.Length + " graphics.");
        }
        catch (System.InvalidOperationException ex)
        {
            Debug.LogError(ex.StackTrace);
            throw;
        }
    }

    private void CreateIdleBodyAnimations()
    {
        if (grhData == null || grhData.Length == 0)
        {
            LoadGrhData();
        }
       
        var bodies = AoFileIO.LoadBodies();

        if (grhData == null || grhData.Length == 0)
            return;

        for (int i = 0; i < bodies.Length; i++)
        {
            int[] frames = { grhData[bodies[i].Bodies[0].grhIndex].Frames[0] };
            CreateAnims(frames, bodies[i].Bodies[0].grhIndex, "IDLE_");

            frames[0] = grhData[bodies[i].Bodies[1].grhIndex].Frames[0];
            CreateAnims(frames, bodies[i].Bodies[1].grhIndex, "IDLE_");

            frames[0] = grhData[bodies[i].Bodies[2].grhIndex].Frames[0];
            CreateAnims(frames, bodies[i].Bodies[2].grhIndex, "IDLE_");

            frames[0] = grhData[bodies[i].Bodies[3].grhIndex].Frames[0];
            CreateAnims(frames, bodies[i].Bodies[3].grhIndex, "IDLE_");
        }
    }

    private void CreateIdleAWeaponAnimations()
    {
        if (grhData == null || grhData.Length == 0)
        {
            LoadGrhData();
        }

        var weapons = AoFileIO.LoadWeaponAnims();

        if (grhData == null || grhData.Length == 0)
            return;

        for (int i = 0; i < weapons.Length; i++)
        {
            int[] frames = { grhData[weapons[i].WeaponAnims[0].grhIndex].Frames[0] };
            CreateAnims(frames, weapons[i].WeaponAnims[0].grhIndex, "IDLEWEAP_");

            frames[0] = grhData[weapons[i].WeaponAnims[1].grhIndex].Frames[0];
            CreateAnims(frames, weapons[i].WeaponAnims[1].grhIndex, "IDLEWEAP_");

            frames[0] = grhData[weapons[i].WeaponAnims[2].grhIndex].Frames[0];
            CreateAnims(frames, weapons[i].WeaponAnims[2].grhIndex, "IDLEWEAP_");

            frames[0] = grhData[weapons[i].WeaponAnims[3].grhIndex].Frames[0];
            CreateAnims(frames, weapons[i].WeaponAnims[3].grhIndex, "IDLEWEAP_");
        }
    }

    private void CreateHeadingAnimations()
    {
        if (grhData == null || grhData.Length == 0)
        {
            LoadGrhData();
        }

        if (grhData == null || grhData.Length == 0)
            return;

        var heads= AoFileIO.LoadHeads();

        int i = 0;

        try
        {
            for (i = 0; i < heads.Length; i++)
            {
                int[] frames = { heads[i].Heads[0].grhIndex };
                CreateAnims(frames, heads[i].Heads[0].grhIndex, "HEAD_");

                frames[0] = heads[i].Heads[1].grhIndex;
                CreateAnims(frames, heads[i].Heads[1].grhIndex, "HEAD_");

                frames[0] = heads[i].Heads[2].grhIndex;
                CreateAnims(frames, heads[i].Heads[2].grhIndex, "HEAD_");

                frames[0] = heads[i].Heads[3].grhIndex;
                CreateAnims(frames, heads[i].Heads[3].grhIndex, "HEAD_");
            }
        }
        catch (Exception)
        {
            Debug.LogError("Failed loading heads at head number: " + i);
            throw;
        }

        _Statuslabel = "Created " + i + " head anims.";
    }

    private void CreateHelmetAnimations()
    {
        if (grhData == null || grhData.Length == 0)
        {
            LoadGrhData();
        }

        if (grhData == null || grhData.Length == 0)
            return;

        var helmets = AoFileIO.LoadHelmets();

        int i = 0;

        try
        {
            for (i = 0; i < helmets.Length; i++)
            {
                int[] frames = { helmets[i].Heads[0].grhIndex };
                CreateAnims(frames, helmets[i].Heads[0].grhIndex, "HELMET_");

                frames[0] = helmets[i].Heads[1].grhIndex;
                CreateAnims(frames, helmets[i].Heads[1].grhIndex, "HELMET_");

                frames[0] = helmets[i].Heads[2].grhIndex;
                CreateAnims(frames, helmets[i].Heads[2].grhIndex, "HELMET_");

                frames[0] = helmets[i].Heads[3].grhIndex;
                CreateAnims(frames, helmets[i].Heads[3].grhIndex, "HELMET_");
            }
        }
        catch (Exception)
        {
            Debug.LogError("Failed loading heads at head number: " + i);
            throw;
        }

        _Statuslabel = "Created " + i + " helmet anims.";
    }

    private void CreateAnims(int[] frames, int index, string namePrefix = "")
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
            UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(texture));
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
        AssetDatabase.CreateAsset(animClip, "Assets/Resources/Animations/" + namePrefix + index + ".anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
}
