using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StevieDev.Dialogue.Editor
{
    public class BaseNode : Node
    {
        private string nodeGUID;
        protected DialogueGraphView _graphView;
        protected DialogueEditorWindow _editorWindow;
        protected Vector2 _defaultNodeSize = new Vector2(200, 250);
        public string NodeGUID { get => nodeGUID; set => nodeGUID = value; }


        private List<LanguageGenericHolder_Text> _languageGenericsList_Texts = new List<LanguageGenericHolder_Text>();
        private List<LanguageGenericHolder_AudioClip> _languageGenericsList_AudioClips = new List<LanguageGenericHolder_AudioClip>();

        public BaseNode()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
            styleSheets.Add(styleSheet);
        }

        #region Get New Field ---
        /// <summary>
        /// Get a new label
        /// </summary>
        /// <param name="labelName">Text in the label</param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected Label GetNewLabel(string labelName, string USS01 = "", string USS02 = "")
        {
            Label label_texts = new Label(labelName);

            // Set USS class for style sheet
            label_texts.AddToClassList(USS01);
            label_texts.AddToClassList(USS02);

            return label_texts;
        }

        /// <summary>
        /// Get a new Button
        /// </summary>
        /// <param name="btnText"> Text in the Button</param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected Button GetNewButton(string btnText, string USS01 = "", string USS02 = "")
        {
            Button btn = new Button()
            {
                text = btnText
            };

            btn.AddToClassList(USS01);
            btn.AddToClassList(USS02);

            return btn;
        }

        // Values --------------

        /// <summary>
        /// Get a new integerField
        /// </summary>
        /// <param name="inputValue">Container_Int that needs to be set in to the integerField</param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected IntegerField GetNewIntegerField(Container_Int inputValue, string USS01 = "", string USS02 = "")
        {
            IntegerField integerField = new IntegerField();

            integerField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = value.newValue;
            });
            integerField.SetValueWithoutNotify(inputValue.Value);


            integerField.AddToClassList(USS01);
            integerField.AddToClassList(USS02);

            return integerField;
        }

        /// <summary>
        /// Get a new float field
        /// </summary>
        /// <param name="inputValue">Container_Float that need to be set in the float field</param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected FloatField GetNewFloatField(Container_Float inputValue, string USS01 = "", string USS02 = "")
        {
            FloatField floatField = new FloatField();

            floatField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = value.newValue;
            });
            floatField.SetValueWithoutNotify(inputValue.Value);


            floatField.AddToClassList(USS01);
            floatField.AddToClassList(USS02);

            return floatField;
        }

        /// <summary>
        /// Get a new TextField
        /// </summary>
        /// <param name="inputValue">Container string that needs ot bet set in the Textfield</param>
        /// <param name="placeHolderText"></param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected TextField GetNewTextField(Container_String inputValue, string placeHolderText, string USS01 = "", string USS02 = "")
        {
            TextField textField = new TextField();

            textField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = value.newValue;
            });
            textField.SetValueWithoutNotify(inputValue.Value);


            textField.AddToClassList(USS01);
            textField.AddToClassList(USS02);

            SetPlaceHolderText(textField, placeHolderText);

            return textField;
        }

        /// <summary>
        /// Get a new image
        /// </summary>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected Image GetNewImage(string USS01 = "", string USS02 = "")
        {
            Image imagePreview = new Image();

            imagePreview.AddToClassList(USS01);
            imagePreview.AddToClassList(USS02);

            return imagePreview;
        }

        /// <summary>
        /// Get a new objectField with a sprite as an object
        /// </summary>
        /// <param name="inputSprite">Container_Sprite that needs to be set in the object field</param>
        /// <param name="imagePreview">Image that need to be set as preview image</param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected ObjectField GetNewObjectField_Sprite(Container_Sprite inputSprite, Image imagePreview, string USS01 = "", string USS02 = "")
        {
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = inputSprite.Value
            };

            objectField.RegisterValueChangedCallback(value =>
            {
                inputSprite.Value = value.newValue as Sprite;
                imagePreview.image = (inputSprite.Value != null ? inputSprite.Value.texture : null);
            });
            imagePreview.image = (inputSprite.Value != null ? inputSprite.Value.texture : null); // initial check

            // set uss class for stylsheet
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);

            return objectField;
        }

        /// <summary>
        /// Get a new ObjectField with a Container_DialogueEventSO as the object
        /// </summary>
        /// <param name="inputDialogueEventSO">Container_DialogueEventSO  that need to be set in object field</param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected ObjectField GetNewObjectField_DialogueEvent(Container_DialogueEventSO inputDialogueEventSO, string USS01 = "", string USS02 = "")
        {
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(Container_DialogueEventSO),
                allowSceneObjects = false,
                value = inputDialogueEventSO.DialogueEventSO
            };

            objectField.RegisterValueChangedCallback(value =>
            {
                inputDialogueEventSO.DialogueEventSO = value.newValue as DialogueEventSO;
            });

            // set uss class for stylsheet
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);

            return objectField;

        }

        // EnumField ---------------

        /// <summary>
        /// Get a new EnumField where the enum is a ChoiceStateType
        /// </summary>
        /// <param name="enumType">Container_ChoiceStateType that needs to be set in the EnumField</param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected EnumField GetEnumField_ChoiceStateType(Container_ChoiceStateType enumType, string USS01 = "", string USS02 = "")
        {
            EnumField enumField = new EnumField()
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);

            enumField.RegisterValueChangedCallback(value =>
            {
                enumType.Value = (ChoiceStateType)value.newValue;
            });
            enumField.SetValueWithoutNotify(enumType.Value);

            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);

            enumType.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField where the enum is a EndNodeType
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected EnumField GetEnumField_EndNodeType(Container_EndNodeType enumType, string USS01 = "", string USS02 = "")
        {
            EnumField enumField = new EnumField()
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);

            enumField.RegisterValueChangedCallback(value =>
            {
                enumType.Value = (EndNodeType)value.newValue;
            });
            enumField.SetValueWithoutNotify(enumType.Value);

            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);

            enumType.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField where the enum is a StringEventModifierType
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected EnumField GetEnumField_StringEventModifierType(Container_StringEventModifierType enumType, Action action, string USS01 = "", string USS02 = "")
        {
            EnumField enumField = new EnumField()
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);

            enumField.RegisterValueChangedCallback(value =>
            {
                enumType.Value = (StringEventModifierType)value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(enumType.Value);

            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);

            enumType.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField where the enum is a StringEventConditionType
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected EnumField GetEnumField_StringEventConditionType(Container_StringEventConditionType enumType, Action action, string USS01 = "", string USS02 = "")
        {
            EnumField enumField = new EnumField()
            {
                value = enumType.Value
            };
            enumField.Init(enumType.Value);

            enumField.RegisterValueChangedCallback(value =>
            {
                enumType.Value = (StringEventConditionType)value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(enumType.Value);

            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);

            enumType.EnumField = enumField;
            return enumField;
        }

        // Custom-Made -------------

        /// <summary>
        /// Get a new TextField taht use a List<LanguageGeneric<string>> Text 
        /// </summary>
        /// <param name="Text">List of languageGeneric Text</param>
        /// <param name="placeHolderText"> The text that will be displayed if the text field is empty</param>
        /// <param name="USS01">USS class add to the UI Element</param>
        /// <param name="USS02">USS class add to the UI Element</param>
        /// <returns></returns>
        protected TextField GetNewTextField_TextLanguage(List<LanguageGeneric<string>> Text, string placeHolderText = "", string USS01 = "", string USS02 = "")
        {
            // Add languages
            foreach(LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                Text.Add(new LanguageGeneric<string> 
                { 
                    LanguageType = language,
                    LanguageGenericType = ""
                });
            }

            // Make Text Field
            TextField textField = new TextField();

            // Add it to the reloaded current language list
            _languageGenericsList_Texts.Add(new LanguageGenericHolder_Text(Text, textField, placeHolderText));

            // When we change the variable from graph view
            textField.RegisterValueChangedCallback(value =>
            {
                Text.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
            });
            textField.SetValueWithoutNotify(Text.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType);

            //Text field is set to multiline
            textField.multiline = true;

            textField.AddToClassList(USS01);
            textField.AddToClassList(USS02);

            return textField;
        }


        protected ObjectField GetNewObjectField_AudioClipsLanguage(List<LanguageGeneric<AudioClip>> audioClips, string USS01 = "", string USS02 = "")
        {
            // Add Languages
            foreach(LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                audioClips.Add(new LanguageGeneric<AudioClip>
                    {
                        LanguageType = language,
                        LanguageGenericType = null
                    });
            }

            // Make Object Field
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = audioClips.Find(audioclip => audioclip.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType
            };

            // Add it to the reload current language list
            _languageGenericsList_AudioClips.Add(new LanguageGenericHolder_AudioClip(audioClips, objectField));

            objectField.RegisterValueChangedCallback(value =>
            {
                audioClips.Find(audioclip => audioclip.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue as AudioClip;
            });

            // Set uss class for stylesheet
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);

            return objectField;
        }

        #endregion

        #region Methods ------------

        /// <summary>
        /// Add a port to the outputContainer
        /// </summary>
        /// <param name="portName"> The name of the port</param>
        /// <param name="capacity"> Can it accept multiple or a single edge</param>
        ///<returns> Get the port that was just added to the outputContainer</returns>
        public void AddOutputPort(string portName, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port outputPort = GetPortInstance(Direction.Output, capacity);
            outputPort.portName = portName;
            outputContainer.Add(outputPort);
        }

        /// <summary>
        /// Add a port to the inputContainer
        /// </summary>
        /// <param name="portName">The name of the port</param>
        /// <param name="capacity">Can it accept multiple or a single edge</param>
        /// <returns> Get the port that was just added to the inputContainer</returns>
        public void AddInputPort(string portName, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port inputPort = GetPortInstance(Direction.Input, capacity);
            inputPort.portName = portName;
            inputContainer.Add(inputPort);
        }

        /// <summary>
        /// Make a new port and return it
        /// </summary>
        /// <param name="nodeDirection">What direction the port is. Whether it is Input or Output</param>
        /// <param name="capacity">Can it accept multiple or a single</param>
        /// <returns>Get new port</returns>
        public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }

        public virtual void LoadValueInToField()
        {

        }

        /// <summary>
        /// Reload Languages to the current selected language
        /// </summary>

        public virtual void ReloadLanguage()
        {
            foreach (LanguageGenericHolder_Text textHolder in _languageGenericsList_Texts)
            {
                Reload_TextLanguage(textHolder.InputText, textHolder.TextField, textHolder.PlaceHolderText);
            }
            foreach (LanguageGenericHolder_AudioClip audioHolder in _languageGenericsList_AudioClips)
            {
                Reload_AudioClipLanguage(audioHolder.InputAudioClips, audioHolder.AudioObjectField);
            }
        }

        /// <summary>
        /// Add String Modifier Event to UI element.
        /// </summary>
        /// <param name="stringEventModifier">The List<EventData_StringModifier> that EventData_StringModifier should be added to.</param>
        /// <param name="stringEvent">EventData_StringModifier that should be use.</param>
        protected void AddStringModifierEventBuild(List<EventData_StringModifier> stringEventModifier, EventData_StringModifier stringEvent = null)
        {
            EventData_StringModifier tmpStringEventModifier = new EventData_StringModifier();

            // If we param value is not null we load in values.
            if (stringEvent != null)
            {
                tmpStringEventModifier.StringEventText.Value = stringEvent.StringEventText.Value;
                tmpStringEventModifier.Number.Value = stringEvent.Number.Value;
                tmpStringEventModifier.StringEventModifierType.Value = stringEvent.StringEventModifierType.Value;
            }

            stringEventModifier.Add(tmpStringEventModifier);

            // Container of all object.
            Box boxContainer = new Box();
            Box boxfloatField = new Box();
            boxContainer.AddToClassList("StringEventBox");
            boxfloatField.AddToClassList("StringEventBoxfloatField");

            // Text.
            TextField textField = GetNewTextField(tmpStringEventModifier.StringEventText, "String Event", "StringEventText");

            // ID number.
            FloatField floatField = GetNewFloatField(tmpStringEventModifier.Number, "StringEventInt");

            // TODO: Delete maby?
            // Check for StringEventType and add the proper one.
            //EnumField enumField = null;

            // String Event Modifier
            Action tmp = () => ShowHide_StringEventModifierType(tmpStringEventModifier.StringEventModifierType.Value, boxfloatField);
            // EnumField String Event Modifier
            EnumField enumField = GetEnumField_StringEventModifierType(tmpStringEventModifier.StringEventModifierType, tmp, "StringEventEnum");
            // Run the show and hide.
            ShowHide_StringEventModifierType(tmpStringEventModifier.StringEventModifierType.Value, boxfloatField);

            // Remove button.
            Button btn = GetNewButton("X", "removeBtn");
            btn.clicked += () =>
            {
                stringEventModifier.Remove(tmpStringEventModifier);
                DeleteBox(boxContainer);
            };

            // Add it to the box
            boxContainer.Add(textField);
            boxContainer.Add(enumField);
            boxfloatField.Add(floatField);
            boxContainer.Add(boxfloatField);
            boxContainer.Add(btn);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        /// <summary>
        /// Add String Condition Event to UI element.
        /// </summary>
        /// <param name="stringEventCondition">The List<EventData_StringCondition> that EventData_StringCondition should be added to.</param>
        /// <param name="stringEvent">EventData_StringCondition that should be use.</param>
        protected void AddStringConditionEventBuild(List<EventData_StringCondition> stringEventCondition, EventData_StringCondition stringEvent = null)
        {
            EventData_StringCondition tmpStringEventCondition = new EventData_StringCondition();

            // If we paramida value is not null we load in values.
            if (stringEvent != null)
            {
                tmpStringEventCondition.StringEventText.Value = stringEvent.StringEventText.Value;
                tmpStringEventCondition.Number.Value = stringEvent.Number.Value;
                tmpStringEventCondition.StringEventConditionType.Value = stringEvent.StringEventConditionType.Value;
            }

            stringEventCondition.Add(tmpStringEventCondition);

            // Container of all object.
            Box boxContainer = new Box();
            Box boxfloatField = new Box();
            boxContainer.AddToClassList("StringEventBox");
            boxfloatField.AddToClassList("StringEventBoxfloatField");

            // Text.
            TextField textField = GetNewTextField(tmpStringEventCondition.StringEventText, "String Event", "StringEventText");

            // ID number.
            FloatField floatField = GetNewFloatField(tmpStringEventCondition.Number, "StringEventInt");

            // Check for StringEventType and add the proper one.
            EnumField enumField = null;
            // String Event Condition
            Action tmp = () => ShowHide_StringEventConditionType(tmpStringEventCondition.StringEventConditionType.Value, boxfloatField);
            // EnumField String Event Condition
            enumField = GetEnumField_StringEventConditionType(tmpStringEventCondition.StringEventConditionType, tmp, "StringEventEnum");
            // Run the show and hide.
            ShowHide_StringEventConditionType(tmpStringEventCondition.StringEventConditionType.Value, boxfloatField);

            // Remove button.
            Button btn = GetNewButton("X", "removeBtn");
            btn.clicked += () =>
            {
                stringEventCondition.Remove(tmpStringEventCondition);
                DeleteBox(boxContainer);
            };

            // Add it to the box
            boxContainer.Add(textField);
            boxContainer.Add(enumField);
            boxfloatField.Add(floatField);
            boxContainer.Add(boxfloatField);
            boxContainer.Add(btn);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        /// <summary>
        /// hid and show the UI element
        /// </summary>
        /// <param name="value">StringEventModifierType</param>
        /// <param name="boxContainer">The Box that will be hidden or shown</param>
        private void ShowHide_StringEventModifierType(StringEventModifierType value, Box boxContainer)
        {
            if (value == StringEventModifierType.SetTrue || value == StringEventModifierType.SetFalse)
            {
                ShowHide(false, boxContainer);
            }
            else
            {
                ShowHide(true, boxContainer);
            }
        }

        /// <summary>
        /// hid and show the UI element
        /// </summary>
        /// <param name="value">StringEventConditionType</param>
        /// <param name="boxContainer">The Box that will be hidden or shown</param>
        private void ShowHide_StringEventConditionType(StringEventConditionType value, Box boxContainer)
        {
            if (value == StringEventConditionType.True || value == StringEventConditionType.False)
            {
                ShowHide(false, boxContainer);
            }
            else
            {
                ShowHide(true, boxContainer);
            }
        }


        /// <summary>
        /// Relaod all the text in the Textfield to the current selected Language
        /// </summary>
        /// <param name="inputText"> List of Language Generic<string</param>
        /// <param name="textField"> The Textfield that is to be reloaded</param>
        /// <param name="placeHolderText">The text that will be displayed if the text field is empty </param>
        private void Reload_TextLanguage(List<LanguageGeneric<string>> inputText, TextField textField, string placeHolderText)
        {
            textField.RegisterValueChangedCallback(value =>
            {
                inputText.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
            });

            SetPlaceHolderText(textField, placeHolderText);
        }

        /// <summary>
        /// Relaod all the text in the Textfield to the current selected Language
        /// </summary>
        /// <param name="inputAudioClips"> List of Language Generic<AudioCLip></param>
        /// <param name="audioObjectField"> The audio object feild to be reloaded</param>
        private void Reload_AudioClipLanguage(List<LanguageGeneric<AudioClip>> inputAudioClips, ObjectField audioObjectField)
        {
            audioObjectField.RegisterValueChangedCallback(value =>
            {
                inputAudioClips.Find(audiocClip => audiocClip.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue as AudioClip;
            });
            audioObjectField.SetValueWithoutNotify(inputAudioClips.Find(audioClip => audioClip.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType);
        }

        /// <summary>
        /// Set a placeholder text on a textfield
        /// </summary>
        /// <param name="textField">Textfield that needs a placeholder</param>
        /// <param name="placeHolder">The text that will be displayed if the text field is empty</param>
        /// <exception cref="NotImplementedException"></exception>
        private void SetPlaceHolderText(TextField textField, string placeHolder)
        {
            string placeHolderClass = TextField.ussClassName + "__placeHolder";

            CheckForText();
            OnFocusOut();
            textField.RegisterCallback<FocusInEvent>(evt => OnFocusIn());
            textField.RegisterCallback<FocusOutEvent>(evt => OnFocusOut());

            void CheckForText()
            {
                if (!string.IsNullOrEmpty(textField.text))
                {
                    textField.RemoveFromClassList(placeHolderClass);
                }
            }

            void OnFocusOut()
            {
                if (!string.IsNullOrEmpty(textField.text))
                {
                    textField.SetValueWithoutNotify(placeHolder);
                    textField.AddToClassList(placeHolderClass);
                }
            }

            void OnFocusIn()
            {
                if (textField.ClassListContains(placeHolderClass))
                {
                    textField.value = String.Empty;
                    textField.RemoveFromClassList(placeHolderClass);
                }
            }
        }

        /// <summary>
        /// Add or remove the USS Hide tag.
        /// </summary>
        /// <param name="show">true = show - flase = hide</param>
        /// <param name="boxContainer">which container box to add the desired USS tag to</param>
        protected void ShowHide(bool show, Box boxContainer)
        {
            string hideUssClass = "Hide";
            if (show == true)
            {
                boxContainer.RemoveFromClassList(hideUssClass);
            }
            else
            {
                boxContainer.AddToClassList(hideUssClass);
            }
        }

        /// <summary>
        /// Remove box container.
        /// </summary>
        /// <param name="boxContainer">desired box to delete and remove</param>
        protected virtual void DeleteBox(Box boxContainer)
        {
            mainContainer.Remove(boxContainer);
            RefreshExpandedState();
        }

        #endregion

        #region LanguageGenericHolder Class -------
        public class LanguageGenericHolder_Text
        {
            public List<LanguageGeneric<string>> InputText;
            public TextField TextField;
            public string PlaceHolderText;

            public LanguageGenericHolder_Text(List<LanguageGeneric<string>> inputText, TextField textField, string placeholderText = "place holder text")
            {
                this.InputText = inputText;
                this.TextField = textField;
                this.PlaceHolderText = placeholderText;
            }
        }

        public class LanguageGenericHolder_AudioClip
        {
            public List<LanguageGeneric<AudioClip>> InputAudioClips;
            public ObjectField AudioObjectField;

            public LanguageGenericHolder_AudioClip(List<LanguageGeneric<AudioClip>> inputAudioClips, ObjectField objectField)
            {
                this.InputAudioClips = inputAudioClips;
                this.AudioObjectField = objectField;
            }
        }
        #endregion
    }
}