using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StevieDev.Dialogue.Editor
{
    public class DialogueNode : BaseNode
    {
        public DialogueNode()
        {

        }

        public DialogueNode(Vector2 Position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            base._editorWindow = editorWindow;
            base._graphView = graphView;

            title = "Dialogue";
            SetPosition(new Rect(Position, _defaultNodeSize));
            NodeGUID = Guid.NewGuid().ToString();
            AddInputPort("Input", Port.Capacity.Multi);
        }
    }
}