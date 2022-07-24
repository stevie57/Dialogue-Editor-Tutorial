using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StevieDev.Dialogue.Example01
{
    public class DialogueTalk : DialogueGetData
    {
        [SerializeField] private DialogueController _dialogueController;
        [SerializeField] private AudioSource _audioSource;

        //private DialogueNodeData _currentDialogueNodeData;
        //private DialogueNodeData _lastDialogueNodeData;

        private void Start()
        {
            _dialogueController = FindObjectOfType<DialogueController>();
            _audioSource.GetComponent<AudioSource>();
        }

        public void StartDialogue()
        {
            //CheckNodeType(GetNextNode(_dialogueContainerSO.StartNodeDatas[0]));
            //_dialogueController.ShowDialogue(true);
        }

        //private void CheckNodeType(BaseNodeData baseNodeData)
        //{
        //    switch (basenodedata)
        //    {
        //        case startnodedata nodedata:
        //            runnode(nodedata);
        //            break;
        //        case dialoguenodedata nodedata:
        //            runnode(nodedata);
        //            break;
        //        case eventnodedata nodedata:
        //            runnode(nodedata);
        //            break;
        //        case endnodedata nodedata:
        //            runnode(nodedata);
        //            break;
        //    }
        //}
    }
}