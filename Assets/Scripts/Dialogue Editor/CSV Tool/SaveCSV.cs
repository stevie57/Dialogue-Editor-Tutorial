using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveCSV
{
    public string csvDirectoryName = "";
    private string csvFileName = "";
    private string csvSeperator = ",";
    private string[] csvHeader;
    private string idName = "GUID ID";

    public void Save()
    {

    }

    private void MakeHeader()
    {
        List<string> headerText = new List<string>();
        headerText.Add(idName);
    }

    private void VerifyDirectory()
    {
        string directory = GetDirectoryPath();
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private string GetDirectoryPath()
    {
        return $"{Application.dataPath}/{csvDirectoryName}";
    }

    private string GetFilePath()
    {
        return $"{GetDirectoryPath()}/{csvFileName}";
    }
}
