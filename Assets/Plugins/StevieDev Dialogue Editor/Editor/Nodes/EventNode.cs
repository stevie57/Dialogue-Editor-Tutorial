using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StevieDev.DialogueEditor
{
    public class EventNode : BaseNode
    {
        private List<EventScriptableObjectData> _eventScriptableObjectDatas = new List<EventScriptableObjectData>();
        private List<EventStringIdData> _eventStringIdDatas = new List<EventStringIdData>();

        public List<EventScriptableObjectData> EventScriptableObjectDatas { get => _eventScriptableObjectDatas; set => _eventScriptableObjectDatas = value; }
        public List<EventStringIdData> EventStringIdDatas { get => _eventStringIdDatas; set => _eventStringIdDatas = value; }

        public EventNode()
        {

        }

        public EventNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;

            title = "Event";
            SetPosition(new Rect(position, _defaultNodeSize));

            NodeGUID = Guid.NewGuid().ToString();

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Output", Port.Capacity.Single);
            TopButton();
            
        }

        //override
        //public void LoadValueInToField()
        //{
        //}

        private void TopButton()
        {
            ToolbarMenu menu = new ToolbarMenu();
            menu.text = "Add Event";

            menu.menu.AppendAction("String ID", new Action<DropdownMenuAction>(x => AddStringEvent()));
            menu.menu.AppendAction("Scriptable Object", new Action<DropdownMenuAction>(x => AddScriptableEvent()));

            titleContainer.Add(menu);
        }
        public void AddStringEvent(EventStringIdData paramIdEventStringIdData = null)
        {
            EventStringIdData tmpEventStringData = new EventStringIdData();
            if(paramIdEventStringIdData != null)
            {
                tmpEventStringData.stringEvent = paramIdEventStringIdData.stringEvent;
                tmpEventStringData.idNumber = paramIdEventStringIdData.idNumber;
            }
            _eventStringIdDatas.Add(tmpEventStringData);

            // container of all objects
            Box boxContainer = new Box();
            boxContainer.AddToClassList("EventBox");

            // Text
            TextField textField = new TextField();
            textField.AddToClassList("EventText");
            boxContainer.Add(textField);
            textField.RegisterValueChangedCallback(value =>
            {
                tmpEventStringData.stringEvent = value.newValue;
            });
            textField.SetValueWithoutNotify(tmpEventStringData.stringEvent);

            // ID number
            IntegerField integerField = new IntegerField();
            integerField.AddToClassList("EventInt");
            boxContainer.Add(integerField);
            integerField.RegisterValueChangedCallback(value =>
            {
                tmpEventStringData.idNumber = value.newValue;
            });
            integerField.SetValueWithoutNotify(tmpEventStringData.idNumber);

            // Remove Button
            Button btn = new Button()
            {
                text = "X"
            };
            btn.clicked += () =>
            {
                DeleteBox(boxContainer);
                _eventStringIdDatas.Remove(tmpEventStringData);
            };
            btn.AddToClassList("EventBtn");
            boxContainer.Add(btn);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        public void AddScriptableEvent(EventScriptableObjectData paramEventSO = null)
        {
            EventScriptableObjectData tmpDialogueEventSO = new EventScriptableObjectData();
            if(paramEventSO != null)
            {
                tmpDialogueEventSO.DialogueEventSO = paramEventSO.DialogueEventSO;
            }
            _eventScriptableObjectDatas.Add(tmpDialogueEventSO);

            // container of all objects
            Box boxContainer = new Box();
            boxContainer.AddToClassList("EventBox");
            // scriptable object field
            ObjectField scriptableObjectField = new ObjectField()
            {
                objectType = typeof(DialogueEventSO),
                allowSceneObjects = false,
                value = null
            };
            scriptableObjectField.SetValueWithoutNotify(tmpDialogueEventSO.DialogueEventSO);
            scriptableObjectField.AddToClassList("EventObject");
            boxContainer.Add(scriptableObjectField);          
            scriptableObjectField.RegisterValueChangedCallback(value =>
            {
                tmpDialogueEventSO.DialogueEventSO = value.newValue as DialogueEventSO;
            });

            // Remove Button
            Button btn = new Button()
            {
                text = "X"
            };
            btn.clicked += () =>
            {
                DeleteBox(boxContainer);
                _eventScriptableObjectDatas.Remove(tmpDialogueEventSO);
            };
            btn.AddToClassList("EventBtn");
            boxContainer.Add(btn);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        private void DeleteBox(Box boxContainer)
        {
            mainContainer.Remove(boxContainer);
            RefreshExpandedState();
        }
    }
}