using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StevieDev.Dialogue.Editor
{
    public class UpdateLanguageType
    {
        public void UpdateLanguage()
        {
            //List<DialogueContainerSO> dialogueContainers = Helper.FindAllDialogueContainerSO();
            //foreach (DialogueContainerSO dialogueContainer in dialogueContainers)
            //{
            //    foreach (DialogueNodeData nodeData in dialogueContainer.DialogueNodeDatas)
            //    {
            //        nodeData.TextLanguages = UpdateLanguageGenerics(nodeData.TextLanguages);
            //        nodeData.AudioClips = UpdateLanguageGenerics(nodeData.AudioClips);

            //        foreach (DialogueNodePort nodePort in nodeData.DialogueNodePorts)
            //        {
            //            nodePort.TextLanguages = UpdateLanguageGenerics(nodePort.TextLanguages);
            //        }
            //    }
            //}
        }

        //private List<LanguageGeneric<T>> UpdateLanguageGenerics<T>(List<LanguageGeneric<T>> languageGenerics)
        //{
            //List<LanguageGeneric<T>> tmp = new List<LanguageGeneric<T>>();
            //foreach (languagetype languagetype in (languagetype[])enum.getvalues(typeof(languagetype)))
            //{
            //    tmp.add(new languagegeneric<t>
            //    {
            //        languagetype = languagetype
            //    });
            //}

            //foreach (languagegeneric<t> languagegeneric in languagegenerics)
            //{
            //    if (tmp.find(language => language.languagetype == languagegeneric.languagetype) != null)
            //    {
            //        tmp.find(language => language.languagetype == languagegeneric.languagetype).languagegenerictype = languagegeneric.languagegenerictype;
            //    }
            //}

            //return tmp;
        //}
    }
}