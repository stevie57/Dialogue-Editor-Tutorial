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
    //{
    //    private DialogueGraphView _graphView;
    //    private List<Edge> _edges => _graphView.edges.ToList();
    //    private List<BaseNode> _nodes => _graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

    //    public GraphSaveLoadUtility(DialogueGraphView graphView)
    //    {
    //        _graphView = graphView;
    //    }

    //    public void Save(DialogueContainerSO dialogueContainerSO)
    //    {
    //        SaveEdges(dialogueContainerSO);
    //        SaveNodes(dialogueContainerSO);

    //        EditorUtility.SetDirty(dialogueContainerSO);
    //        AssetDatabase.SaveAssets();
    //    }

    //    public void Load(DialogueContainerSO dialogueContainerSO)
    //    {
    //        ClearGraph();
    //        GenerateNodes(dialogueContainerSO);
    //        ConnectNodes(dialogueContainerSO);
    //    }

    //    #region Save
    //    private void SaveEdges(DialogueContainerSO dialogueContainerSO)
    //    {
    //        dialogueContainerSO.NodeLinkDatas.Clear();

    //        Edge[] connectedEdges = _edges.Where(edge => edge.input.node != null).ToArray();
    //        for (int i = 0; i < connectedEdges.Count(); i++)
    //        {
    //            BaseNode outputNode = connectedEdges[i].output.node as BaseNode;
    //            BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

    //            dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData()
    //            {
    //                BaseNodeGUID = outputNode.NodeGUID,
    //                BasePortName = connectedEdges[i].output.portName,
    //                TargetNodeGUID = inputNode.NodeGUID,
    //                TargetPortName = connectedEdges[i].input.portName
    //            });
    //        }
    //    }

    //    private void SaveNodes(DialogueContainerSO dialogeContainerSO)
    //    {
    //        dialogeContainerSO.StartNodeDatas.Clear();
    //        dialogeContainerSO.DialogueNodeDatas.Clear();
    //        dialogeContainerSO.EventNodeDatas.Clear();
    //        dialogeContainerSO.EndNodeDatas.Clear();
    //        dialogeContainerSO.BranchNodeDatas.Clear();

    //        foreach (BaseNode node in _nodes)
    //        {
    //            switch (node)
    //            {
    //                case StartNode startNode:
    //                    dialogeContainerSO.StartNodeDatas.Add(SaveNodeData(startNode));
    //                    break;
    //                case DialogueNode dialogueNode:
    //                    dialogeContainerSO.DialogueNodeDatas.Add(SaveNodeData(dialogueNode));
    //                    break;
    //                case EventNode eventNode:
    //                    dialogeContainerSO.EventNodeDatas.Add(SaveNodeData(eventNode));
    //                    break;
    //                case EndNode endNode:
    //                    dialogeContainerSO.EndNodeDatas.Add(SaveNodeData(endNode));
    //                    break;
    //                case BranchNode branchNode:
    //                    dialogeContainerSO.BranchNodeDatas.Add(SaveNodeData(branchNode));
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //    }

    //    private StartNodeData SaveNodeData(StartNode node)
    //    {
    //        StartNodeData nodeData = new StartNodeData()
    //        {
    //            SavedNodeGUID = node.NodeGUID,
    //            Position = node.GetPosition().position,
    //        };

    //        return nodeData;
    //    }

    //    private EndNodeData SaveNodeData(EndNode node)
    //    {
    //        EndNodeData nodeData = new EndNodeData()
    //        {
    //            SavedNodeGUID = node.NodeGUID,
    //            Position = node.GetPosition().position,
    //        };

    //        return nodeData;
    //    }

    //    private EventNodeData SaveNodeData(EventNode node)
    //    {
    //        EventNodeData nodeData = new EventNodeData()
    //        {
    //            SavedNodeGUID = node.NodeGUID,
    //            Position = node.GetPosition().position,
    //        };

    //        return nodeData;
    //    }

    //    private DialogueNodeData SaveNodeData(DialogueNode node)
    //    {
    //        DialogueNodeData dialogueNodeData = new DialogueNodeData
    //        {
    //            SavedNodeGUID = node.NodeGUID,
    //            Position = node.GetPosition().position,
    //        };
    //        return dialogueNodeData;
    //    }

    //    private BranchNodeData SaveNodeData(BranchNode node)
    //    {
    //        // Go through all edges and find the ones connected to this node and then turn it into a list
    //        //List<Edge> tmpEdges = _edges.Where(x => x.output.node == node).Cast<Edge>().ToList();
            
    //        Edge trueOutput = _edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "True");
    //        Edge falseOutput = _edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "False");

    //        BranchNodeData nodeData = new BranchNodeData()
    //        {
    //            SavedNodeGUID = node.NodeGUID,
    //            Position = node.GetPosition().position,
    //        };

    //        return nodeData;
    //    }

    //    #endregion

    //    #region Load

    //    private void ClearGraph()
    //    {
    //        _edges.ForEach(edge => { _graphView.RemoveElement(edge); });
    //        _nodes.ForEach(node => { _graphView.RemoveElement(node); });
    //    }

    //    private void GenerateNodes(DialogueContainerSO dialogueContainerSO)
    //    {
    //        // Create Start Node
    //        foreach (StartNodeData node in dialogueContainerSO.StartNodeDatas)
    //        {
    //            StartNode tempNode = _graphView.CreateStart(node.Position);
    //            tempNode.NodeGUID = node.SavedNodeGUID;

    //            _graphView.AddElement(tempNode);
    //        }

    //        // Create Dialogue Nodes
    //        foreach (DialogueNodeData node in dialogueContainerSO.DialogueNodeDatas)
    //        {
    //            DialogueNode tempNode = _graphView.CreateDialogue(node.Position);
    //            tempNode.NodeGUID = node.SavedNodeGUID;

    //            tempNode.LoadValueInToField();
    //            _graphView.AddElement(tempNode);
    //        }

    //        // Create Event Nodes
    //        foreach (EventNodeData node in dialogueContainerSO.EventNodeDatas)
    //        {
    //            EventNode tempNode = _graphView.CreateEvent(node.Position);
    //            tempNode.NodeGUID = node.SavedNodeGUID;
               
    //            tempNode.LoadValueInToField();
    //            _graphView.AddElement(tempNode);
    //        }

    //        //Create Branch Nodes
    //        foreach(BranchNodeData node in dialogueContainerSO.BranchNodeDatas)
    //        {
    //            BranchNode tempNode = _graphView.CreateBranch(node.Position);
    //            tempNode.NodeGUID = node.SavedNodeGUID;

    //            foreach(BranchStringIdData item in node.BranchStringIdDatas)
    //            {
    //                tempNode.AddCondition(item);
    //            }

    //            tempNode.LoadValueInToField();
    //            //tempNode.ReloadLanguage();
    //            _graphView.AddElement(tempNode);
    //        }

    //        // Create End Nodes
    //        foreach (EndNodeData node in dialogueContainerSO.EndNodeDatas)
    //        {
    //            EndNode tempNode = _graphView.CreateEnd(node.Position);
    //            tempNode.NodeGUID = node.SavedNodeGUID;

    //            tempNode.LoadValueInToField();
    //            _graphView.AddElement(tempNode);
    //        }
    //    }

    //    private void ConnectNodes(DialogueContainerSO dialogueContainerSO)
    //    {
    //        // make connection for all nodes except dialogue nodes
    //        for (int i = 0; i < _nodes.Count; i++)
    //        {
    //            List<NodeLinkData> connections = dialogueContainerSO.NodeLinkDatas.Where(Edge => Edge.BaseNodeGUID == _nodes[i].NodeGUID).ToList();

    //            // Check current node and look in its output container.
    //            // Look through all children visual elements to find Ports and cast back as a list.
    //            List<Port> allOutputPorts = _nodes[i].outputContainer.Children().Where(x => x is Port).Cast<Port>().ToList();

    //            for (int j = 0; j < connections.Count; j++)
    //            {
    //                string targetNodeGuid = connections[j].TargetNodeGUID;
    //                BaseNode targetNode = _nodes.First(node => node.NodeGUID == targetNodeGuid);

    //                if (targetNode == null) continue;

    //                foreach(Port item in allOutputPorts)
    //                {
    //                    if(item.portName == connections[j].BasePortName)
    //                    {
    //                        LinkNodesTogether(item, (Port)targetNode.inputContainer[0]);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    private void LinkNodesTogether(Port outputPort, Port inputPort)
    //    {
    //        Edge tempEdge = new Edge()
    //        {
    //            output = outputPort,
    //            input = inputPort
    //        };

    //        tempEdge.input.Connect(tempEdge);
    //        tempEdge.output.Connect(tempEdge);

    //        _graphView.Add(tempEdge);
    //    }
    //    #endregion
    }
}