using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StevieDev.Dialogue
{
    [System.Serializable]
    public class BranchData : BaseData
    {
        public string TruiGuiNode;
        public string FalseGuiNode;
        public List<EventData_StringCondition> EventData_StringConditions = new List<EventData_StringCondition>();
    }
}
