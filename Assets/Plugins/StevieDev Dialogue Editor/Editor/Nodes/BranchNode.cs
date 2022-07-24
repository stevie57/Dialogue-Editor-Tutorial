using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StevieDev.DialogueEditor
{
    public class BranchNode : BaseNode
    {
        private List<BranchStringIdData> _branchStringIdData= new List<BranchStringIdData>();

        public List<BranchStringIdData> BranchStringIdData { get => _branchStringIdData; set => _branchStringIdData = value; }

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

            menu.menu.AppendAction("String Condition", new Action<DropdownMenuAction>(x => AddCondition()));

            titleButtonContainer.Add(menu);
        }

        public void AddCondition(BranchStringIdData paramBranchString = null)
        {
            BranchStringIdData tmpBranchStringIdData = new BranchStringIdData();
            if(paramBranchString != null)
            {
                tmpBranchStringIdData.StringEvent = paramBranchString.StringEvent;
                tmpBranchStringIdData.IdNumber = paramBranchString.IdNumber;
            }
            _branchStringIdData.Add(tmpBranchStringIdData);

            // Container for all objects
            Box boxContainer = new Box();
            boxContainer.AddToClassList("BranchBox");

            // Text field
            TextField textField = new TextField();
            textField.AddToClassList("EventText");
            boxContainer.Add(textField);
            textField.RegisterValueChangedCallback(value =>
            {
                tmpBranchStringIdData.StringEvent = value.newValue;
            });
            textField.SetValueWithoutNotify(tmpBranchStringIdData.StringEvent);

            // ID Number
            IntegerField integerField = new IntegerField();
            integerField.AddToClassList("BranchID");
            boxContainer.Add(integerField);
            integerField.RegisterValueChangedCallback(value =>
           {
               tmpBranchStringIdData.IdNumber = value.newValue;
           });
            integerField.SetValueWithoutNotify(tmpBranchStringIdData.IdNumber);

            Button btn = new Button() { text = "X" };
            btn.clicked += () =>
            {
                DeleteBox(boxContainer);
                _branchStringIdData.Remove(tmpBranchStringIdData);
            };
            boxContainer.Add(btn);

            mainContainer.Add(boxContainer);
        }

        private void DeleteBox(Box boxContainer)
        {
            mainContainer.Remove(boxContainer);
            RefreshExpandedState();
        }
    }
}
