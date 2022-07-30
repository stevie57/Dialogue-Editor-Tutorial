using System.Collections.Generic;
using UnityEngine.Playables;

namespace StevieDev.Dialogue
{
    [System.Serializable]
    public class VRDialogueData : BaseData
    {
        public Container_String NodeTitle = new Container_String();
        public PlayableAsset DialogueTimeline;
        public List<VRChoiceData> ChoiceLists = new List<VRChoiceData>();
    }

    [System.Serializable]
    public class VRChoiceData
    {
        public Container_String ChoiceName = new Container_String();
        public List<KeywordData> Keywords = new List<KeywordData>();
        public DialogueData_Port ChoicePort = new DialogueData_Port();
    }

    [System.Serializable]
    public class KeywordData
    {
        public Container_String KeywordEntry = new Container_String();
    }
}