using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace StevieDev.Dialogue.Editor
{
    public class BranchNode : BaseNode
    {
        //private List<BranchStringIdData> _branchStringIdData= new List<BranchStringIdData>();

        //public List<BranchStringIdData> BranchStringIdData { get => _branchStringIdData; set => _branchStringIdData = value; }

        public BranchNode()
        {

        }

        public BranchNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            base._editorWindow = editorWindow;
            base._graphView = graphView;

            title = "Branch";                                   // Set Name
            SetPosition(new Rect(position, _defaultNodeSize));  // Set Position
            NodeGUID = Guid.NewGuid().ToString();               // Set ID

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("True", Port.Capacity.Single);
            AddOutputPort("False", Port.Capacity.Single);

            TopButton();
        }

        private void TopButton()
        {
            ToolbarMenu menu = new ToolbarMenu();
            menu.text = "Add Condition";

            //menu.menu.AppendAction("String Condition", new Action<DropdownMenuAction>(x => AddCondition()));

            titleButtonContainer.Add(menu);
        }

    }
}
