using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveLoadUtility 
{
    private DialogueGraphView _graphView;
    private List<Edge> _edes => _graphView.edges.ToList();
    private List<BaseNode> _nodes => _graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

    public GraphSaveLoadUtility(DialogueGraphView graphView)
    {
        _graphView = graphView;
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

        Edge[] connectedEdges = _edes.Where(edge => edge.input.node != null).ToArray();
        for (int i = 0; i < connectedEdges.Count(); i++)
        {
            BaseNode outputNode = connectedEdges[i].output.node as BaseNode;
            BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

            dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData()
            {
                BaseNodeGUID = outputNode.NodeGUID,
                TargetNodeGUID = inputNode.NodeGUID,
            });
        }
    }

    private void SaveNodes(DialogueContainerSO dialogeContainerSO)
    {
        dialogeContainerSO.StartNodeDatas.Clear();
        dialogeContainerSO.DialogueNodeDatas.Clear();
        dialogeContainerSO.EventNodeDatas.Clear();
        dialogeContainerSO.EndNodeDatas.Clear();

        foreach(BaseNode node in _nodes)
        {
            switch (node)
            {
                case StartNode startNode:
                    dialogeContainerSO.StartNodeDatas.Add(SaveNodeData(startNode));
                    break;
                case DialogueNode dialogueNode:
                    dialogeContainerSO.DialogueNodeDatas.Add(SaveNodeData(dialogueNode));
                    break;
                case EventNode eventNode:
                    dialogeContainerSO.EventNodeDatas.Add(SaveNodeData(eventNode));
                    break;
                case EndNode endNode:
                    dialogeContainerSO.EndNodeDatas.Add(SaveNodeData(endNode));
                    break;
            }
        }
    }

    private StartNodeData SaveNodeData(StartNode node) 
    {
        StartNodeData nodeData = new StartNodeData()
        {
            SavedNodeGUID = node.NodeGUID,
            Position = node.GetPosition().position,
        };

        return nodeData;
    }

    private EndNodeData SaveNodeData(EndNode node)
    {
        EndNodeData nodeData = new EndNodeData()
        {
            SavedNodeGUID = node.NodeGUID,
            Position = node.GetPosition().position,
            EndNodeType = node.EndNodeType
        };

        return nodeData;
    }

    private EventNodeData SaveNodeData(EventNode node)
    {
        EventNodeData nodeData = new EventNodeData()
        {
            SavedNodeGUID = node.NodeGUID,
            Position = node.GetPosition().position,
            DialogueEventSO = node.DialogueEvent
        };

        return nodeData;
    }

    private DialogueNodeData SaveNodeData(DialogueNode node)
    {
        DialogueNodeData dialogueNodeData = new DialogueNodeData
        {
            SavedNodeGUID = node.NodeGUID,
            Position = node.GetPosition().position,
            
            TextType = node.Texts,
            Name = node.Name,
            AudioClips = node.AudioClips,
            DialogueFaceImageType = node.FaceImageType,
            Sprite = node.FaceImage,
            DialogueNodePorts = new List<DialogueNodePort>(node.DialogueNodePorts),            
        };

        // going through each dialogue node port and checking connected edges
        foreach(DialogueNodePort nodePort in dialogueNodeData.DialogueNodePorts)
        {
            nodePort.OutputGUID = string.Empty;
            nodePort.InputGUID = string.Empty;
            foreach(Edge edge in _edes)
            {
                if(edge.output == nodePort.MyPort)
                {
                    nodePort.OutputGUID = (edge.output.node as BaseNode).NodeGUID;
                    nodePort.InputGUID = (edge.input.node as BaseNode).NodeGUID;
                }
            }
        }
        return dialogueNodeData;
    }
    #endregion

    #region Load

    private void ClearGraph()
    {
        _edes.ForEach(edge => { _graphView.RemoveElement(edge); });
        _nodes.ForEach(node => { _graphView.RemoveElement(node); });
    }

    private void GenerateNodes(DialogueContainerSO dialogueContainerSO)
    {
        // Create Start Node
        foreach(StartNodeData node in dialogueContainerSO.StartNodeDatas)
        {
            StartNode tempNode = _graphView.CreateStart(node.Position);
            tempNode.NodeGUID = node.SavedNodeGUID;
            
            _graphView.AddElement(tempNode);
        }

        // Create Dialogue Nodes
        foreach(DialogueNodeData node in dialogueContainerSO.DialogueNodeDatas)
        {
            DialogueNode tempNode = _graphView.CreateDialogue(node.Position);
            tempNode.NodeGUID = node.SavedNodeGUID;
            tempNode.Name = node.Name;
            tempNode.FaceImageType = node.DialogueFaceImageType;
            tempNode.FaceImage = node.Sprite;

            foreach(LanguageGeneric<string> languageGeneric in node.TextType)
            {
                tempNode.Texts.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType; ;
            }

            foreach (LanguageGeneric<AudioClip> languageGeneric in node.AudioClips)
            {
                tempNode.AudioClips.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType; ;
            }

            foreach (DialogueNodePort port in node.DialogueNodePorts)
            {
                tempNode.AddChoicePort(tempNode, port);
            }

            tempNode.LoadValueInToField();
            _graphView.AddElement(tempNode);
        }

        foreach(EventNodeData node in dialogueContainerSO.EventNodeDatas)
        {
            EventNode tempNode = _graphView.CreateEvent(node.Position);
            tempNode.NodeGUID = node.SavedNodeGUID;
            tempNode.DialogueEvent = node.DialogueEventSO;

            tempNode.LoadValueInToField();
            _graphView.AddElement(tempNode);
        }

        foreach(EndNodeData node in dialogueContainerSO.EndNodeDatas)
        {
            EndNode tempNode = _graphView.CreateEnd(node.Position);
            tempNode.NodeGUID = node.SavedNodeGUID;
            tempNode.EndNodeType = node.EndNodeType;

            tempNode.LoadValueInToField();
            _graphView.AddElement(tempNode);
        }
    }

    private void ConnectNodes(DialogueContainerSO dialogueContainerSO)
    {
        for (int i = 0; i < _nodes.Count; i++)
        {
            List<NodeLinkData> connections = dialogueContainerSO.NodeLinkDatas.Where(Edge => Edge.BaseNodeGUID == _nodes[i].NodeGUID).ToList();

            for (int j = 0; j < connections.Count; j++)
            {
                string targetNodeGuid = connections[j].TargetNodeGUID;
                BaseNode targetNode = _nodes.First(node => node.NodeGUID == targetNodeGuid);

                if((_nodes[i] is DialogueNode) == false)
                {
                    LinkNodesTogether(_nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);
                }
            }
        }

        List<DialogueNode> dialogueNodes = _nodes.FindAll(node => node is DialogueNode).Cast<DialogueNode>().ToList();

        foreach(DialogueNode dialogueNode in dialogueNodes)
        {
            foreach(DialogueNodePort nodePort in dialogueNode.DialogueNodePorts)
            {
                if(nodePort.InputGUID != string.Empty)
                {
                    BaseNode targetNode = _nodes.Find(node => node.NodeGUID == nodePort.InputGUID);
                    LinkNodesTogether(nodePort.MyPort, (Port)targetNode.inputContainer[0]);
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
