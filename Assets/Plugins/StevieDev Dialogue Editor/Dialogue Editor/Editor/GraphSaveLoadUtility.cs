using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace StevieDev.Dialogue.Editor
{
    public class GraphSaveLoadUtility
    {
        private List<Edge> _edges => _graphView.edges.ToList();
        private List<BaseNode> _nodes => _graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
        private DialogueGraphView _graphView;

        public GraphSaveLoadUtility(DialogueGraphView graphView)
        {
            this._graphView = graphView;
        }

        public void Save(DialogueContainerSO dialogueContainerSO)
        {
            SaveEdges(dialogueContainerSO);
            SaveNodes(dialogueContainerSO);

            EditorUtility.SetDirty(dialogueContainerSO);
            AssetDatabase.SaveAssets();
        }

        public void Load(DialogueContainerSO dialogueContainerSO)
        {
            ClearGraph();
            GenerateNodes(dialogueContainerSO);
            ConnectNodes(dialogueContainerSO);
        }

        #region Save
        private void SaveEdges(DialogueContainerSO dialogueContainerSO)
        {
            dialogueContainerSO.NodeLinkDatas.Clear();

            Edge[] connectedEdges = _edges.Where(edge => edge.input.node != null).ToArray(); // If an edge input.node is not null then it is connected
            for (int i = 0; i < connectedEdges.Count(); i++)
            {
                BaseNode outputNode = (BaseNode)connectedEdges[i].output.node;
                BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

                dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData
                {
                    BaseNodeGuid = outputNode.NodeGUID,
                    BasePortName = connectedEdges[i].output.portName,
                    TargetNodeGuid = inputNode.NodeGUID,
                    TargetPortName = connectedEdges[i].input.portName,
                });
            }
        }

        private void SaveNodes(DialogueContainerSO dialogueContainerSO)
        {
            dialogueContainerSO.EventDatas.Clear();
            dialogueContainerSO.EndDatas.Clear();
            dialogueContainerSO.StartDatas.Clear();
            dialogueContainerSO.BranchDatas.Clear();
            dialogueContainerSO.DialogueDatas.Clear();
            dialogueContainerSO.ChoiceDatas.Clear();
            dialogueContainerSO.VRDialogueDatas.Clear();

            _nodes.ForEach(node =>
            {
                switch (node)
                {
                    case DialogueNode dialogueNode:
                        dialogueContainerSO.DialogueDatas.Add(SaveNodeData(dialogueNode));
                        break;
                    case StartNode startNode:
                        dialogueContainerSO.StartDatas.Add(SaveNodeData(startNode));
                        break;
                    case EndNode endNode:
                        dialogueContainerSO.EndDatas.Add(SaveNodeData(endNode));
                        break;
                    case EventNode eventNode:
                        dialogueContainerSO.EventDatas.Add(SaveNodeData(eventNode));
                        break;
                    case BranchNode branchNode:
                        dialogueContainerSO.BranchDatas.Add(SaveNodeData(branchNode));
                        break;
                    case ChoiceNode choiceNode:
                        dialogueContainerSO.ChoiceDatas.Add(SaveNodeData(choiceNode));
                        break;
                    case VRDialogueNode vrDialogueNode:
                        dialogueContainerSO.VRDialogueDatas.Add(SaveNodeData(vrDialogueNode));
                        break;
                    default:
                        break;
                }
            });
        }

        private DialogueData SaveNodeData(DialogueNode node)
        {
            DialogueData newDialogueData = new DialogueData
            {
                NodeGuid = node.NodeGUID,
                Position = node.GetPosition().position
            };

            // Set ID. Numbering them based on the master list
            for (int i = 0; i < node.DialogueData.Dialogue_BaseContainers.Count; i++)
            {
                node.DialogueData.Dialogue_BaseContainers[i].ID.Value = i;
            }

            // Going through all data lists
            // Using if statement to seperate out type of data as we go each base data
            foreach (DialogueData_BaseContainer baseContainer in node.DialogueData.Dialogue_BaseContainers)
            {
                // Name nodes saving
                if (baseContainer is DialogueData_Name)
                {
                    DialogueData_Name tmp = (baseContainer as DialogueData_Name);
                    DialogueData_Name tmpData = new DialogueData_Name();

                    tmpData.ID.Value = tmp.ID.Value;
                    tmpData.CharacterName.Value = tmp.CharacterName.Value;

                    newDialogueData.DialogueData_Names.Add(tmpData);
                }

                // Text nodes saving
                if (baseContainer is DialogueData_Text)
                {
                    DialogueData_Text tmp = (baseContainer as DialogueData_Text);
                    DialogueData_Text tmpData = new DialogueData_Text();

                    tmpData.ID = tmp.ID;
                    tmpData.GuidID = tmp.GuidID;
                    tmpData.Text = tmp.Text;
                    tmpData.AudioClips = tmp.AudioClips;

                    newDialogueData.DialogueData_Texts.Add(tmpData);
                }

                // Images nodes saving
                if (baseContainer is DialogueData_Images)
                {
                    DialogueData_Images tmp = (baseContainer as DialogueData_Images);
                    DialogueData_Images tmpData = new DialogueData_Images();

                    tmpData.ID.Value = tmp.ID.Value;
                    tmpData.Sprite_Left.Value = tmp.Sprite_Left.Value;
                    tmpData.Sprite_Right.Value = tmp.Sprite_Right.Value;

                    newDialogueData.DialogueData_Images.Add(tmpData);
                }
            }

            // Port
            foreach (DialogueData_Port port in node.DialogueData.DialogueData_Ports)
            {
                DialogueData_Port portData = new DialogueData_Port();

                portData.OutputGuid = string.Empty;
                portData.InputGuid = string.Empty;
                portData.PortGuid = port.PortGuid;

                foreach (Edge edge in _edges)
                {
                    if (edge.output.portName == port.PortGuid)
                    {
                        portData.OutputGuid = (edge.output.node as BaseNode).NodeGUID;
                        portData.InputGuid = (edge.input.node as BaseNode).NodeGUID;
                    }
                }

                newDialogueData.DialogueData_Ports.Add(portData);
            }

            return newDialogueData;
        }

        private StartData SaveNodeData(StartNode node)
        {
            StartData nodeData = new StartData()
            {
                NodeGuid = node.NodeGUID,
                Position = node.GetPosition().position,
            };

            return nodeData;
        }

        private EndData SaveNodeData(EndNode node)
        {
            EndData newEndNodeData = new EndData()
            {
                NodeGuid = node.NodeGUID,
                Position = node.GetPosition().position,
            };
            newEndNodeData.EndNodeType.Value = node.EndData.EndNodeType.Value;

            return newEndNodeData;
        }

        private EventData SaveNodeData(EventNode node)
        {
            EventData newNodeData = new EventData()
            {
                NodeGuid = node.NodeGUID,
                Position = node.GetPosition().position,
            };

            // Save Dialogue Event
            foreach (Container_DialogueEventSO dialogueEvent in node.EventData.Container_DialogueEventSOs)
            {
                newNodeData.Container_DialogueEventSOs.Add(dialogueEvent);
            }

            // Save String Event
            foreach (EventData_StringModifier stringEvents in node.EventData.EventData_StringModifiers)
            {
                EventData_StringModifier tmp = new EventData_StringModifier();
                tmp.Number.Value = stringEvents.Number.Value;
                tmp.StringEventText.Value = stringEvents.StringEventText.Value;
                tmp.StringEventModifierType.Value = stringEvents.StringEventModifierType.Value;

                newNodeData.EventData_StringModifiers.Add(tmp);
            }
            return newNodeData;
        }

        private BranchData SaveNodeData(BranchNode node)
        {
            List<Edge> tmpEdges = _edges.Where(x => x.output.node == node).Cast<Edge>().ToList();

            Edge trueOutput = _edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "True");
            Edge falseOutput = _edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "False");

            BranchData newBranchNodeData = new BranchData()
            {
                NodeGuid = node.NodeGUID,
                Position = node.GetPosition().position,
                TruiGuiNode = (trueOutput != null ? (trueOutput.input.node as BaseNode).NodeGUID : string.Empty),
                FalseGuiNode = (falseOutput != null ? (falseOutput.input.node as BaseNode).NodeGUID : string.Empty),
            };

            foreach (EventData_StringCondition stringEvents in node.BranchData.EventData_StringConditions)
            {
                EventData_StringCondition tmp = new EventData_StringCondition();
                tmp.Number.Value = stringEvents.Number.Value;
                tmp.StringEventText.Value = stringEvents.StringEventText.Value;
                tmp.StringEventConditionType.Value = stringEvents.StringEventConditionType.Value;

                newBranchNodeData.EventData_StringConditions.Add(tmp);
            }

            return newBranchNodeData;
        }

        private ChoiceData SaveNodeData(ChoiceNode node)
        {
            ChoiceData newChoiceNodeData = new ChoiceData()
            {
                NodeGuid = node.NodeGUID,
                Position = node.GetPosition().position,

                Text = node.ChoiceData.Text,
                AudioClips = node.ChoiceData.AudioClips,
            };
            newChoiceNodeData.ChoiceStateTypes.Value = node.ChoiceData.ChoiceStateTypes.Value;

            foreach (EventData_StringCondition stringEvents in node.ChoiceData.EventData_StringConditions)
            {
                EventData_StringCondition tmp = new EventData_StringCondition();
                tmp.StringEventText.Value = stringEvents.StringEventText.Value;
                tmp.Number.Value = stringEvents.Number.Value;
                tmp.StringEventConditionType.Value = stringEvents.StringEventConditionType.Value;

                newChoiceNodeData.EventData_StringConditions.Add(tmp);
            }

            return newChoiceNodeData;
        }

        private VRDialogueData SaveNodeData(VRDialogueNode node)
        {
            VRDialogueData newVRDialogueData = new VRDialogueData()
            {
                NodeGuid = node.NodeGUID,
                Position = node.GetPosition().position,
            };

            newVRDialogueData.DialogueTimeline = node.VRDialogueData.DialogueTimeline;
            newVRDialogueData.NodeTitle = node.VRDialogueData.NodeTitle;

            // Port
            foreach (VRChoiceData choice in node.VRDialogueData.ChoiceLists)
            {
                VRChoiceData newChoiceData = new VRChoiceData();
                newChoiceData.ChoiceName = choice.ChoiceName;
                newChoiceData.Keywords.AddRange(choice.Keywords);
                newChoiceData.ChoicePort.PortGuid = choice.ChoicePort.PortGuid;
                newChoiceData.ChoicePort.OutputGuid = String.Empty;
                newChoiceData.ChoicePort.InputGuid = String.Empty;

                foreach(Edge edge in _edges)
                {
                    if (edge.output.portName == choice.ChoicePort.PortGuid)
                    {
                        newChoiceData.ChoicePort.OutputGuid = (edge.output.node as BaseNode).NodeGUID;
                        newChoiceData.ChoicePort.InputGuid = (edge.input.node as BaseNode).NodeGUID;
                    }
                }

                //DialogueData_Port portData = new DialogueData_Port();

                //portData.OutputGuid = string.Empty;
                //portData.InputGuid = string.Empty;
                ////portData.PortGuid = port.PortGuid;
                //portData.PortGuid = choice.ChoicePort.PortGuid;

                //foreach (Edge edge in _edges)
                //{
                //    if (edge.output.portName == choice.ChoicePort.PortGuid)
                //    {
                //        portData.OutputGuid = (edge.output.node as BaseNode).NodeGUID;
                //        portData.InputGuid = (edge.input.node as BaseNode).NodeGUID;
                //    }
                //}
                newVRDialogueData.ChoiceLists.Add(newChoiceData);
            }
            return newVRDialogueData;
        }
        #endregion

        #region Load

        private void ClearGraph()
        {
            _edges.ForEach(edge => _graphView.RemoveElement(edge));

            foreach (BaseNode node in _nodes)
            {
                _graphView.RemoveElement(node);
            }
        }

        private void GenerateNodes(DialogueContainerSO dialogueContainer)
        {
            // Start
            foreach (StartData node in dialogueContainer.StartDatas)
            {
                StartNode tempNode = _graphView.CreateStartNode(node.Position);
                tempNode.NodeGUID = node.NodeGuid;

                _graphView.AddElement(tempNode);
            }

            // End Node 
            foreach (EndData node in dialogueContainer.EndDatas)
            {
                EndNode tempNode = _graphView.CreateEndNode(node.Position);
                tempNode.NodeGUID = node.NodeGuid;
                tempNode.EndData.EndNodeType.Value = node.EndNodeType.Value;

                tempNode.LoadValueInToField();
                _graphView.AddElement(tempNode);
            }

            // Event Node
            foreach (EventData node in dialogueContainer.EventDatas)
            {
                EventNode tempNode = _graphView.CreateEventNode(node.Position);
                tempNode.NodeGUID = node.NodeGuid;

                foreach (Container_DialogueEventSO item in node.Container_DialogueEventSOs)
                {
                    tempNode.AddScriptableEvent(item);
                }
                foreach (EventData_StringModifier item in node.EventData_StringModifiers)
                {
                    tempNode.AddStringEvent(item);
                }

                tempNode.LoadValueInToField();
                _graphView.AddElement(tempNode);
            }

            // Breach Node
            foreach (BranchData node in dialogueContainer.BranchDatas)
            {
                BranchNode tempNode = _graphView.CreateBranchNode(node.Position);
                tempNode.NodeGUID= node.NodeGuid;

                foreach (EventData_StringCondition item in node.EventData_StringConditions)
                {
                    tempNode.AddCondition(item);
                }

                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            }

            // Choice Node
            foreach (ChoiceData node in dialogueContainer.ChoiceDatas)
            {
                ChoiceNode tempNode = _graphView.CreateChoiceNode(node.Position);
                tempNode.NodeGUID= node.NodeGuid;

                tempNode.ChoiceData.ChoiceStateTypes.Value = node.ChoiceStateTypes.Value;

                foreach (LanguageGeneric<string> dataText in node.Text)
                {
                    foreach (LanguageGeneric<string> editorText in tempNode.ChoiceData.Text)
                    {
                        if (editorText.LanguageType == dataText.LanguageType)
                        {
                            editorText.LanguageGenericType = dataText.LanguageGenericType;
                        }
                    }
                }
                foreach (LanguageGeneric<AudioClip> dataAudioClip in node.AudioClips)
                {
                    foreach (LanguageGeneric<AudioClip> editorAudioClip in tempNode.ChoiceData.AudioClips)
                    {
                        if (editorAudioClip.LanguageType == dataAudioClip.LanguageType)
                        {
                            editorAudioClip.LanguageGenericType = dataAudioClip.LanguageGenericType;
                        }
                    }
                }

                foreach (EventData_StringCondition item in node.EventData_StringConditions)
                {
                    tempNode.AddCondition(item);
                }

                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            }

            // Dialogue Node
            foreach (DialogueData node in dialogueContainer.DialogueDatas)
            {
                DialogueNode tempNode = _graphView.CreateDialogueNode(node.Position);
                tempNode.NodeGUID = node.NodeGuid;

                List<DialogueData_BaseContainer> data_BaseContainer = new List<DialogueData_BaseContainer>();

                data_BaseContainer.AddRange(node.DialogueData_Images);
                data_BaseContainer.AddRange(node.DialogueData_Texts);
                data_BaseContainer.AddRange(node.DialogueData_Names);

                data_BaseContainer.Sort(delegate (DialogueData_BaseContainer x, DialogueData_BaseContainer y)
                {
                    return x.ID.Value.CompareTo(y.ID.Value);
                });

                foreach (DialogueData_BaseContainer data in data_BaseContainer)
                {
                    switch (data)
                    {
                        case DialogueData_Name Name:
                            tempNode.CharacterName(Name);
                            break;
                        case DialogueData_Text Text:
                            tempNode.TextLine(Text);
                            break;
                        case DialogueData_Images image:
                            tempNode.ImagePic(image);
                            break;
                        default:
                            break;
                    }
                }

                foreach (DialogueData_Port port in node.DialogueData_Ports)
                {
                    tempNode.AddChoicePort(tempNode, port);
                }

                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            }

            // VR dialogue node
            foreach(VRDialogueData node in dialogueContainer.VRDialogueDatas)
            {
                VRDialogueNode tempNode = _graphView.CreateVRDialogueNode(node.Position);
                tempNode.NodeGUID = node.NodeGuid;
                tempNode.VRDialogueData.NodeTitle = node.NodeTitle;
                tempNode.AddCustomDialogueNodeTitle(tempNode.VRDialogueData.NodeTitle);

                tempNode.VRDialogueData.DialogueTimeline = node.DialogueTimeline;
                tempNode.AddTimelinePlayableField(node.DialogueTimeline);

                foreach(VRChoiceData choice in node.ChoiceLists)
                {
                    tempNode.AddChoice(choice);
                }              
                _graphView.AddElement(tempNode);
            }
        }

        private void ConnectNodes(DialogueContainerSO dialogueContainer)
        {
            // Make connection for all node.
            for (int i = 0; i < _nodes.Count; i++)
            {
                List<NodeLinkData> connections = dialogueContainer.NodeLinkDatas.Where(edge => edge.BaseNodeGuid == _nodes[i].NodeGUID).ToList();

                List<Port> allOutputPorts = _nodes[i].outputContainer.Children().Where(x => x is Port).Cast<Port>().ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string targetNodeGuid = connections[j].TargetNodeGuid;
                    BaseNode targetNode = _nodes.First(node => node.NodeGUID == targetNodeGuid);

                    if (targetNode == null)
                        continue;

                    foreach (Port item in allOutputPorts)
                    {
                        if (item.portName == connections[j].BasePortName)
                        {
                            LinkNodesTogether(item, (Port)targetNode.inputContainer[0]);
                        }
                    }
                }
            }
        }

        private void LinkNodesTogether(Port outputPort, Port inputPort)
        {
            Edge tempEdge = new Edge()
            {
                output = outputPort,
                input = inputPort
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _graphView.Add(tempEdge);
        }

        #endregion
    }
    
}