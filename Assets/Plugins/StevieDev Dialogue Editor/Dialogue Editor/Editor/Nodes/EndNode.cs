using UnityEngine;
using System;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace StevieDev.Dialogue.Editor
{
    public class EndNode : BaseNode
    {
        private EndData endData = new EndData();
        public EndData EndData { get => endData; set => endData = value; }

        public EndNode()
        {

        }

        public EndNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;

            StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/EndNodeStyleSheet");
            styleSheets.Add(styleSheet);

            title = "End";
            SetPosition(new Rect(position, _defaultNodeSize));
            NodeGUID = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);

            MakeMainContainer();
        }
        private void MakeMainContainer()
        {
            EnumField enumField = GetEnumField_EndNodeType(endData.EndNodeType);

            mainContainer.Add(enumField);
        }

        public override void LoadValueInToField()
        {
            if (EndData.EndNodeType.EnumField != null)
                EndData.EndNodeType.EnumField.SetValueWithoutNotify(EndData.EndNodeType.Value);
        }
    }
}