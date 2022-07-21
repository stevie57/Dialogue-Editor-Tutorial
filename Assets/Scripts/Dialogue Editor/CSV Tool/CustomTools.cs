using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// https://youtu.be/R4B8JeiKt5M?t=593 custom tool
public class CustomTools : MonoBehaviour
{
    [MenuItem("Custom Tools/Dialogue/Update Dialoge Languages")]
    public static void UpdateDialogueLanguage()
    {
        UpdateLanguageType updateLanguageType = new UpdateLanguageType();
        updateLanguageType.UpdateLanguage();

        EditorApplication.Beep();
        Debug.Log($"<color=green> Dialogue Language Updated </color>");
    }
}
