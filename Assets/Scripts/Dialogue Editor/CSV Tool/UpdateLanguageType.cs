using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLanguageType
{
    public void UpdateLanguage()
    {
        List<DialogueContainerSO> dialogueContainers = Helper.FindAllObjectFromResources<DialogueContainerSO>();
        foreach(DialogueContainerSO dialogueContainer in dialogueContainers)
        {
            foreach (DialogueNodeData nodeData in dialogueContainer.DialogueNodeDatas) 
            {
                nodeData.TextType = UpdateLanguageGenerics(nodeData.TextType);
                nodeData.AudioClips = UpdateLanguageGenerics(nodeData.AudioClips);

                foreach(DialogueNodePort nodePort in nodeData.DialogueNodePorts)
                {
                    nodePort.TextLanguages = UpdateLanguageGenerics(nodePort.TextLanguages);
                }
            }
        }
    }

    private List<LanguageGeneric<T>> UpdateLanguageGenerics<T>(List<LanguageGeneric<T>> languageGenerics)
    {
        List<LanguageGeneric<T>> tmp = new List<LanguageGeneric<T>>();
        foreach(LanguageType languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            tmp.Add(new LanguageGeneric<T>
            {
                LanguageType = languageType
            });
        }

        foreach(LanguageGeneric<T> languageGeneric in languageGenerics)
        {
            if(tmp.Find(language => language.LanguageType == languageGeneric.LanguageType) != null)
            {
                tmp.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
            }
        }

        return tmp;
    }
}
