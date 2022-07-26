using UnityEngine;
using System;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace StevieDev.Dialogue.Editor
{
    public class EndNode : BaseNode
    {
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
        }

    }
}