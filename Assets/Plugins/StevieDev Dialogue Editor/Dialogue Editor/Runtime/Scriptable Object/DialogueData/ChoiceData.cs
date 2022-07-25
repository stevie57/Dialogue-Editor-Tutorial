using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine.UIElements;
//#endif 

namespace StevieDev.Dialogue
{
    [System.Serializable]
    public class ChoiceData : BaseData
    {
#if UNITY_EDITOR
        public TextField TextField { get; set; }
        public ObjectField ObjectField { get; set; }
#endif

        public Container_ChoiceStateType ChoiceStateType = new Container_ChoiceStateType();
        public List<LanguageGeneric<string>> Text = new List<LanguageGeneric<string>>();
        public List<LanguageGeneric<AudioClip>> AudioClips = new List<LanguageGeneric<AudioClip>>();
        public List<EventData_StringCondition> ConditionData_StringEvents = new List<EventData_StringCondition>();
    }
}
