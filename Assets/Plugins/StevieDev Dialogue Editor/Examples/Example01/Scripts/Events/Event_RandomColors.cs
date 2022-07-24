using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StevieDev.Dialogue.Example01
{
    [CreateAssetMenu(menuName = "Dialogue/Random Color Event")]
    [System.Serializable]
    public class Event_RandomColors : DialogueEventSO
    {
        public override void RunEvent()
        {
            GameEvents.Instance.CallRandomColorModel();
            base.RunEvent();
        }
    }
}