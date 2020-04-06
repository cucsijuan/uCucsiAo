using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AOGraphicsConverser : ScriptableWizard
{
    public string searchTag = "Your tag here";

    private GrhData[] grhData;

    [MenuItem ("CAO Tools/AO Graphics Converser")]
    static void SelectAllofTagwizard()
    {
        ScriptableWizard.DisplayWizard<AOGraphicsConverser>("AO Graphics Converser", "Create Animations", "Load Graphics file");
    }


    private void OnWizardCreate()
    {
        
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
}
