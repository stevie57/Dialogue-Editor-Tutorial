using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StevieDev.DialogueEditor
{
    public class DialogueGetData : MonoBehaviour
    {
        [SerializeField] protected DialogueContainerSO _dialogueContainerSO;

        protected BaseNodeData GetNodeByGUID(string targetNodeGUID)
        {
            return _dialogueContainerSO.AllNodes.Find(node => node.SavedNodeGUID == targetNodeGUID);
        }

        protected BaseNodeData GetNodeByNodePort(DialogueNodePort nodePort)
        {
            return _dialogueContainerSO.AllNodes.Find(node => node.SavedNodeGUID == nodePort.InputGUID);
        }

        protected BaseNodeData GetNextNode(BaseNodeData baseNodeData)
        {
            NodeLinkData nodeLinkData = _dialogueContainerSO.NodeLinkDatas.Find(edge => edge.BaseNodeGUID == baseNodeData.SavedNodeGUID);

            return GetNodeByGUID(nodeLinkData.TargetNodeGUID);
        }
    }
}