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
        private string _defaultEmptyChoiceTitle = "Name this Choice";

        public VRDialogueNode() 
        { 
        
        }

        public VRDialogueNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            base._editorWindow = editorWindow;
            base._graphView = graphView;

            StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/VRDialogueNodeStyleSheet");
            styleSheets.Add(styleSheet);

            SetPosition(new Rect(position, _defaultNodeSize));
            _nodeGUID = Guid.NewGuid().ToString();
            
            AddInputPort("Input", Port.Capacity.Single);

            Label nodeTitle = new Label();
            nodeTitle.AddToClassList("Label");
            nodeTitle.text = "Enter Node Title";
            mainContainer.Add(nodeTitle);
            title = _VRDialogueData.NodeTitle.Value;
            TextField nodeTitleTextField = new TextField();
            nodeTitleTextField.RegisterValueChangedCallback(value =>
            {
                _VRDialogueData.NodeTitle.Value = value.newValue;
                title = value.newValue;
            });
            nodeTitleTextField.SetValueWithoutNotify(_VRDialogueData.NodeTitle.Value);
            mainContainer.Add(nodeTitleTextField);
            AddTimelinePlayableField();
            CreateChoiceButton();
            AddChoice(this._VRDialogueData);
        }

        private void AddTimelinePlayableField()
        {
            Label TimelineLabel = new Label();
            TimelineLabel.AddToClassList("Label");
            TimelineLabel.text = "Timeline Asset";
            mainContainer.Add(TimelineLabel);

            ObjectField timelinePlayableField = new ObjectField()
            {
                objectType = typeof(PlayableAsset),
                allowSceneObjects = false,
                value = _VRDialogueData.DialogueTimeline
            };
            timelinePlayableField.RegisterValueChangedCallback(value =>
            {
                _VRDialogueData.DialogueTimeline = value.newValue as PlayableAsset;
            });

            timelinePlayableField.SetValueWithoutNotify(_VRDialogueData.DialogueTimeline);
            mainContainer.Add(timelinePlayableField);
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

            newVRChoiceData.ChoiceName.Value = _defaultEmptyChoiceTitle;

            // Create Choice Port Data
            DialogueData_Port newChoicePortData = new DialogueData_Port();
            newChoicePortData.OutputGuid = _nodeGUID;
            _VRDialogueData.DialogueData_Ports.Add(newChoicePortData);

            // Create Port
            Port newChoicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            Label choiceTitleLable = new Label();
            choiceTitleLable.text = newVRChoiceData.ChoiceName.Value;            
            newChoicePort.contentContainer.Add(choiceTitleLable);
            Label portNameLabel = newChoicePort.contentContainer.Q<Label>("type");
            portNameLabel.AddToClassList("PortName");


            Label TitleFieldLabel = new Label() { text = "Choice Title" };
            TitleFieldLabel.AddToClassList("ChoiceTitleBox");
            choiceBox.Add(TitleFieldLabel);
            TextField choiceTitleTextField = new TextField();

            choiceTitleTextField.RegisterValueChangedCallback(value =>
            {
                newVRChoiceData.ChoiceName.Value = value.newValue;
                choiceTitleLable.text = value.newValue;
            });
            choiceTitleTextField.SetValueWithoutNotify(newVRChoiceData.ChoiceName.Value);
            choiceBox.Add(choiceTitleTextField);

            outputContainer.Add(newChoicePort);

            Box KeywordsBox = new Box();
            Label KeywordsLabel = new Label();
            KeywordsLabel.AddToClassList("Label");
            KeywordsLabel.text = "Current KeyWords:";
            KeywordsBox.Add(KeywordsLabel);

            Box KeywordsBoxList = new Box();
            RefreshKeywordsBox(KeywordsBoxList, newVRChoiceData.Keywords);
            KeywordsBox.Add(KeywordsBoxList);

            Label EnterKeywordFieldLabel = new Label();
            EnterKeywordFieldLabel.AddToClassList("Label");
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
                if(_newKeyWordText != String.Empty)
                {
                    KeywordData newKeyword = new KeywordData();
                    newKeyword.KeywordEntry.Value = _newKeyWordText;
                    newVRChoiceData.Keywords.Add(newKeyword);
                    _newKeyWordText = String.Empty;
                    newKeywordField.SetValueWithoutNotify(_newKeyWordText);
                    RefreshKeywordsBox(KeywordsBoxList, newVRChoiceData.Keywords);
                }
            };
            KeywordsBox.Add(AddKeyWordButton);

            choiceBox.Add(KeywordsBox);

            // Delete Choice button
            Button DeleteButton = new Button();
            DeleteButton.AddToClassList("ChoiceDeleteButton");
            DeleteButton.text = "Delete Choice";
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

            mainContainer.Add(choiceBox);
            RefreshExpandedState();
        }
        
        private void RefreshKeywordsBox(Box keywordBoxList, List<KeywordData> keywordsList)
        {
            keywordBoxList.Clear();
            if(keywordsList.Count == 0)
            {
                Label keywordLabel = new Label();
                keywordLabel.text = "No Keywords Saved for Choice !!!";
                keywordLabel.AddToClassList("NoKeyWordsWarning");
                keywordBoxList.Add(keywordLabel);
            }
            else
            {
                foreach (KeywordData keyword in keywordsList)
                {
                    Box keywordBox = new Box();
                    keywordBox.AddToClassList("KeywordEntryBox");
                    Label keywordLabel = new Label();
                    keywordLabel.text = keyword.KeywordEntry.Value;
                    keywordLabel.AddToClassList("KeywordEntryText");
                    keywordBox.Add(keywordLabel);

                    Button deleteKeywordButton = new Button();
                    deleteKeywordButton.AddToClassList("KeywordDeleteButton");
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
        public Container_String NodeTitle = new Container_String();
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