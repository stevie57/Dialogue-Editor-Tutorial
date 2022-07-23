using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// https://youtu.be/R4B8JeiKt5M?t=593 custom tool
public class CustomTools 
{
    [MenuItem("Custom Tools/Dialogue/Load From CSV")]
    public static void LoadFromCSV()
    {
        LoadCSV loadCSV = new LoadCSV();
        loadCSV.Load();

        EditorApplication.Beep();
        Debug.Log($"<color=green> Loading CSV File Successful! </color>");
    }

    [MenuItem("Custom Tools/Dialogue/Save to CSV")]
    public static void SaveToCSV()
    {
        SaveCSV saveCSV = new SaveCSV();
        saveCSV.Save();

        EditorApplication.Beep();
        Debug.Log($"<color=green> Save CSV File Successful! </color>");
    }

    [MenuItem("Custom Tools/Dialogue/Update Dialoge Languages")]
    public static void UpdateDialogueLanguage()
    {
        UpdateLanguageType updateLanguageType = new UpdateLanguageType();
        updateLanguageType.UpdateLanguage();

        EditorApplication.Beep();
        Debug.Log($"<color=green> Dialogue Language Updated </color>");
    }
}
