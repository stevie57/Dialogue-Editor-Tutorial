using System.Collections.Generic;
using UnityEngine.Playables;

namespace StevieDev.Dialogue.Editor
{
    public class VRDialogueNode : BaseNode
    {
        private VRDialogueData _VRDialogueData = new VRDialogueData();
    }

    public class VRDialogueData : BaseData
    {
        public PlayableAsset DialogueTimeline;
        public List<DialogueData_Port> DialogueData_Ports = new List<DialogueData_Port>();
    }
}