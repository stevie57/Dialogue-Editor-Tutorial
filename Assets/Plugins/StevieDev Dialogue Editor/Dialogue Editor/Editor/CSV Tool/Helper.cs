using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace StevieDev.Dialogue.Editor
{
    public static class Helper
    {
        public static List<T> FindAllObjectFromResources<T>()
        {
            List<T> tmp = new List<T>();
            string resourcesPath = Application.dataPath + "/Resources";
            string[] directories = Directory.GetDirectories(resourcesPath, "*", SearchOption.AllDirectories);

            foreach (string directory in directories)
            {
                string directoryPath = directory.Substring(resourcesPath.Length + 1); // get the directory name only by skipping the resourcePath portion
                T[] result = Resources.LoadAll(directoryPath, typeof(T)).Cast<T>().ToArray();   // Find all objects of that type. Cast it to that type. Turn it into an Array

                foreach (T item in result)
                {
                    if (!tmp.Contains(item))
                        tmp.Add(item);
                }
            }

            return tmp;
        }

        // allows you to find dialogue container in other folders than resource folder
        public static List<DialogueContainerSO> FindAllDialogueContainerSO()
        {
            // Find all the DialogueContainerSO in Assets and get its GUID ID
            string[] guids = AssetDatabase.FindAssets("t:DialogueContainerSO");

            // Make an array as long as as the guids array
            DialogueContainerSO[] items = new DialogueContainerSO[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);                      // Use the guid ID to find the asset path
                items[i] = AssetDatabase.LoadAssetAtPath<DialogueContainerSO>(path) ;       // Use the path to find and load DialogueContainerSO
            }

            return items.ToList();
        }
    }
}

