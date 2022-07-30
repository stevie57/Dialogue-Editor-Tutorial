using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace StevieDev.Dialogue.Editor
{
    public class VRDialogueNode : BaseNode
    {
        private VRDialogueData _VRDialogueData = new VRDialogueData();
        private string _newKeyWordText = string.Empty;

        public VRDialogueNode() 
        { 
        
        }

        public VRDialogueNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            base._editorWindow = editorWindow;
            base._graphView = graphView;

            StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/DialogueNodeStyleSheet");
            styleSheets.Add(styleSheet);

            title = "Dialogue Node";
            SetPosition(new Rect(position, _defaultNodeSize));
            _nodeGUID = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Single);           
            CreateChoiceButton();
        }
        private void CreateChoiceButton()
        {     
            Button btn = new Button()
            {
                text = "Add Choice",
            };
            btn.AddToClassList("TopBtn");

            btn.clicked += () =>
            {
                AddChoice(this._VRDialogueData);
            };

            titleButtonContainer.Add(btn);
        }

        private void AddChoice(VRDialogueData dialogueData)
        {
            Box choiceBox = new Box();
            
            // Choice Data
            VRChoiceData newVRChoiceData = new VRChoiceData();
            dialogueData.ChoiceLists.Add(newVRChoiceData);
            newVRChoiceData.ChoiceName.Value = "Choice Test";

            //TitleNameBox.AddToClassList("TitleNameBox");
            Label TitleFieldLabel = new Label() { text = "Choice Title" };
            choiceBox.Add(TitleFieldLabel);

            TextField choiceTitleTextField = new TextField();
            choiceTitleTextField.RegisterValueChangedCallback(value =>
            {
                newVRChoiceData.ChoiceName.Value = value.newValue;
            });
            choiceTitleTextField.SetValueWithoutNotify(newVRChoiceData.ChoiceName.Value);
            choiceBox.Add(choiceTitleTextField);

            // Create Choice Port Data
            DialogueData_Port newChoicePortData = new DialogueData_Port();
            newChoicePortData.OutputGuid = _nodeGUID;
            _VRDialogueData.DialogueData_Ports.Add(newChoicePortData);

            // Create Port
            Port newChoicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputContainer.Add(newChoicePort);

            // removal button
            Button DeleteButton = new Button();
            DeleteButton.text = "X";

            DeleteButton.clicked += () =>
            {
                // Remove Port Data
                DialogueData_Port targetData = dialogueData.DialogueData_Ports.Find(data => data.OutputGuid == newChoicePortData.OutputGuid);
                dialogueData.DialogueData_Ports.Remove(targetData);
                
                // Remove Choice Data
                dialogueData.ChoiceLists.Remove(newVRChoiceData);

                // Remove edges connected the port
                IEnumerable<Edge> portEdge = _graphView.edges.ToList().Where(edge => edge.output == newChoicePort);
                if (portEdge.Any())
                {
                    Edge edge = portEdge.First();
                    edge.input.Disconnect(edge);
                    edge.output.Disconnect(edge);
                    _graphView.RemoveElement(edge);
                }

                // remove output container port
                this.outputContainer.Remove(newChoicePort);
                DeleteBox(choiceBox);
            };
            choiceBox.Add(DeleteButton);

            Box KeywordsBox = new Box();
            Label KeywordsLabel = new Label();
            KeywordsLabel.text = "Current KeyWords:";
            KeywordsBox.Add(KeywordsLabel);

            Box KeywordsBoxList = new Box();

            // test
            //KeywordData testKeyword = new KeywordData();
            //testKeyword.KeywordEntry.Value = "test";
            //newVRChoiceData.Keywords.Add(testKeyword);

            RefreshKeywordsBox(KeywordsBoxList, newVRChoiceData.Keywords);
            KeywordsBox.Add(KeywordsBoxList);

            Label EnterKeywordFieldLabel = new Label();
            EnterKeywordFieldLabel.text = "Enter New KeyWord";
            KeywordsBox.Add(EnterKeywordFieldLabel);
            TextField newKeywordField = new TextField();
            newKeywordField.RegisterValueChangedCallback(value =>
            {
                _newKeyWordText = value.newValue;
            });
            newKeywordField.SetValueWithoutNotify(_newKeyWordText);
            KeywordsBox.Add(newKeywordField);

            Button AddKeyWordButton = new Button();
            AddKeyWordButton.text = "Add KeyWord";
            AddKeyWordButton.clicked += () =>
            {
                KeywordData newKeyword = new KeywordData();
                newKeyword.KeywordEntry.Value = _newKeyWordText;
                newVRChoiceData.Keywords.Add(newKeyword);
                _newKeyWordText = String.Empty;
                RefreshKeywordsBox(KeywordsBoxList, newVRChoiceData.Keywords);
            };
            KeywordsBox.Add(AddKeyWordButton);

            choiceBox.Add(KeywordsBox);
            mainContainer.Add(choiceBox);
            RefreshExpandedState();
        }
        
        private void RefreshKeywordsBox(Box keywordBoxList, List<KeywordData> keywordsList)
        {
            keywordBoxList.Clear();
            if(keywordsList.Count == 0)
            {
                Label keywordLabel = new Label();
                keywordLabel.text = "No Saved Keywords";
                keywordBoxList.Add(keywordLabel);
            }
            else
            {
                foreach (KeywordData keyword in keywordsList)
                {
                    Box keywordBox = new Box();
                    
                    Label keywordLabel = new Label();
                    keywordLabel.text = keyword.KeywordEntry.Value;
                    keywordBox.Add(keywordLabel);

                    Button deleteKeywordButton = new Button();
                    deleteKeywordButton.text = "X";
                    deleteKeywordButton.clicked += () =>
                    {
                        KeywordData removeData = keywordsList.Find(data => data.KeywordEntry.Value == keyword.KeywordEntry.Value);
                        keywordsList.Remove(removeData);
                        keywordBoxList.Remove(keywordBox);
                    };
                    keywordBox.Add(deleteKeywordButton);

                    keywordBoxList.Add(keywordBox);
                }
            }
        }
    }

    public class VRDialogueData : BaseData
    {
        public PlayableAsset DialogueTimeline;
        public List<DialogueData_Port> DialogueData_Ports = new List<DialogueData_Port>();
        public List<VRChoiceData> ChoiceLists = new List<VRChoiceData>();
    }

    public class VRChoiceData: BaseData
    {
        public Container_String ChoiceName = new Container_String();
        public List<KeywordData> Keywords = new List<KeywordData>();
    }

    public class KeywordData
    {
        public Container_String KeywordEntry = new Container_String();
    }
}