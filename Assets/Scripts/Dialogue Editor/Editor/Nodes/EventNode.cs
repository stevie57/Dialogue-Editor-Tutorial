﻿using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EventNode : BaseNode
{
    private DialogueEventSO _dialogueEvent;
    private ObjectField _objectField;

    public DialogueEventSO DialogueEvent { get => _dialogueEvent; set => _dialogueEvent = value; }

    public EventNode()
    {

    }
    public EventNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView )
    {
        _editorWindow = editorWindow;
        _graphView = graphView;

        title = "Event";
        SetPosition(new Rect(position, _defaultNodeSize));

        NodeGUID = Guid.NewGuid().ToString();

        AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("Output", Port.Capacity.Single);

        _objectField = new ObjectField()
        {
            objectType = typeof(DialogueEventSO),
            allowSceneObjects = false,
            value = _dialogueEvent
        };
        _objectField.RegisterValueChangedCallback(value =>
        {
            _dialogueEvent = _objectField.value as DialogueEventSO;
        });
        _objectField.SetValueWithoutNotify(_dialogueEvent);

        

        mainContainer.Add(_objectField);
    }
}
