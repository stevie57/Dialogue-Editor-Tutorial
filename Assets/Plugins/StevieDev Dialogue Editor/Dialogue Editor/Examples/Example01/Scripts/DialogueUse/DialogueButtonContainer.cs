using UnityEngine.Events;

namespace StevieDev.Dialogue.Example01
{
    public class DialogueButtonContainer
    {
        public UnityAction UnityAction { get; set; }
        public string Text { get; set; }
        public bool ConditionCheck { get; set; }
        public ChoiceStateType ChoiceState { get; set; }
    }
}