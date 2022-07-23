using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace StevieDev.DialogueEditor
{
    public class StartNode : BaseNode
    {
        public StartNode()
        {

        }

        public StartNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;

            title = "Start";
            SetPosition(new Rect(position, _defaultNodeSize));
            NodeGUID = Guid.NewGuid().ToString();

            AddOutputPort("Output", Port.Capacity.Single);

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}