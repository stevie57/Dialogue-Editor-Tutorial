using UnityEngine;
using System;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class EndNode : BaseNode
{
    private EndNodeType _endNodeType = EndNodeType.End;
    private EnumField _enumField;

    public EndNodeType EndNodeType { get => _endNodeType; set => _endNodeType = value; }
    
    public EndNode()
    {

    }

    public EndNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        _editorWindow = editorWindow;
        _graphView = graphView;

        title = "End";
        SetPosition(new Rect(position, _defaultNodeSize));
        NodeGUID = Guid.NewGuid().ToString();

        AddInputPort("Input", Port.Capacity.Multi);

        //Creating enum fields
        _enumField = new EnumField()
        {
            value = _endNodeType
        };
        _enumField.Init(_endNodeType);
        _enumField.RegisterValueChangedCallback((value) =>
        {
            _endNodeType = (EndNodeType) value.newValue;
        });
        _enumField.SetValueWithoutNotify(_endNodeType);

        mainContainer.Add(_enumField);
    }

    override
    public void LoadValueInToField()
    {
        _enumField.SetValueWithoutNotify(_endNodeType);
    }
}
