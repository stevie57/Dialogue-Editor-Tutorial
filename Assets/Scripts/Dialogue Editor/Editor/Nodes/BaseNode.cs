using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseNode : Node
{
    private string nodeGUID;
    protected string NodeGUID { get => nodeGUID; set => nodeGUID = value; }
    protected DialogueGraphView _graphView;
    protected DialogueEditorWindow _editorWindow;
    protected Vector2 _defaultNodeSize = new Vector2(200, 250);

    public BaseNode()
    {
        StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
        styleSheets.Add(styleSheet);
    }

    public void AddOutputPort(string portName, Port.Capacity capacity = Port.Capacity.Single)
    {
        Port outputPort = GetPortInstance(Direction.Output, capacity);
        outputPort.name = portName;
        outputContainer.Add(outputPort);
    }

    public void AddInputPort(string portName, Port.Capacity capacity = Port.Capacity.Multi)
    {
        Port inputPort = GetPortInstance(Direction.Input, capacity);
        inputPort.name = portName;
        inputContainer.Add(inputPort);
    }

    public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }
}
