using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StevieDev.Dialogue.Example01
{
    public class DialogueGetData : MonoBehaviour
    {
        [SerializeField] protected DialogueContainerSO _dialogueContainer;
        protected BaseData GetNodeByGuid(string targetNodeGuid)
        {
            return _dialogueContainer.AllDatas.Find(node => node.NodeGuid == targetNodeGuid);
        }

        protected BaseData GetNodeByNodePort(DialogueData_Port nodePort)
        {
            return _dialogueContainer.AllDatas.Find(node => node.NodeGuid == nodePort.InputGuid);
        }

        protected BaseData GetNextNode(BaseData baseNodeData)
        {
            NodeLinkData nodeLinkData = _dialogueContainer.NodeLinkDatas.Find(egde => egde.BaseNodeGuid == baseNodeData.NodeGuid);

            return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
        }
    }
}