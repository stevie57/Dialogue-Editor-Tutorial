using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTalk : DialogueGetData
{
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private AudioSource _audioSource;

    private DialogueNodeData _currentDialogueNodeData;
    private DialogueNodeData _lastDialogueNodeData;

    private void Start()
    {
        _dialogueController = FindObjectOfType<DialogueController>();
        _audioSource.GetComponent<AudioSource>();
    }

    public void StartDialogue()
    {
        CheckNodeType(GetNextNode(_dialogueContainerSO.StartNodeDatas[0]));
        _dialogueController.ShowDialogue(true);
    }

    private void CheckNodeType(BaseNodeData baseNodeData)
    {
        switch (baseNodeData)
        {
            case StartNodeData nodeData:
                RunNode(nodeData);
                break;
            case DialogueNodeData nodeData:
                RunNode(nodeData);
                break;
            case EventNodeData nodeData:
                RunNode(nodeData);
                break;
            case EndNodeData nodeData:
                RunNode(nodeData);
                break;
        }
    }

    private void RunNode(StartNodeData nodeDAta)
    {
        CheckNodeType(GetNextNode(_dialogueContainerSO.StartNodeDatas[0]));
    }

    private void RunNode(DialogueNodeData nodeData)
    {
        _lastDialogueNodeData = _currentDialogueNodeData;
        _currentDialogueNodeData = nodeData;

        _dialogueController.SetText(
            _currentDialogueNodeData.Name,
            _currentDialogueNodeData.TextType.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
        _dialogueController.SetImage(_currentDialogueNodeData.Sprite, _currentDialogueNodeData.DialogueFaceImageType);
        
        MakeButtons(_currentDialogueNodeData.DialogueNodePorts);
    }

    private void RunNode(EventNodeData nodeData)
    {
        if(nodeData.DialogueEventSO != null)
        {
            nodeData.DialogueEventSO.RunEvent();
        }

        CheckNodeType(GetNextNode(nodeData));
    }

    private void RunNode(EndNodeData nodeData)
    {
        switch (nodeData.EndNodeType)
        {
            case EndNodeType.End:
                _dialogueController.ShowDialogue(false);
                break;
            case EndNodeType.Repeat:
                CheckNodeType(GetNodeByGUID(_currentDialogueNodeData.SavedNodeGUID));
                break;
            case EndNodeType.GoBack:
                CheckNodeType(GetNodeByGUID(_lastDialogueNodeData.SavedNodeGUID));
                break;
            case EndNodeType.ReturnToStart:
                CheckNodeType(GetNextNode(_dialogueContainerSO.StartNodeDatas[0]));
                break;
            default:
                break;
        }
    }

    private void MakeButtons(List<DialogueNodePort> nodePorts)
    {
        List<string> buttonTexts = new List<string>();
        List<UnityAction> unityActions = new List<UnityAction>();

        foreach(DialogueNodePort nodePort in nodePorts)
        {
            buttonTexts.Add(nodePort.TextLanguages.Find
                (text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
            UnityAction TempAction = null;
            TempAction += () =>
            {
                CheckNodeType(GetNodeByGUID(nodePort.InputGUID));
                _audioSource.Stop();
            };
            unityActions.Add(TempAction);
        }

        _dialogueController.SetButtons(buttonTexts, unityActions);
    }
}
