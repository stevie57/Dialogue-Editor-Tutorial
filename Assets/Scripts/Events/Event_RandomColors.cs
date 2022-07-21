using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_RandomColors : DialogueEventSO
{
    public override void RunEvent()
    {
        GameEvents.Instance.CallRandomColorModel();
        base.RunEvent();
    }
}
