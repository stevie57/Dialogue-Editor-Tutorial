using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StevieDev.DialogueEditor
{
    public class DialogueNode : BaseNode
    {
        private List<LanguageGeneric<string>> _textLanguages = new List<LanguageGeneric<string>>();
        private List<LanguageGeneric<AudioClip>> _audioClips = new List<LanguageGeneric<AudioClip>>();
        private Sprite _faceImage;
        private string _characterName = string.Empty;
        private DialogueFaceImageType _faceImageType;
        private List<DialogueNodePort> _dialogueNodePorts = new List<DialogueNodePort>();

        public List<LanguageGeneric<string>> TextLanguages { get => _textLanguages; set => _textLanguages = value; }
        public List<LanguageGeneric<AudioClip>> AudioClips { get => _audioClips; set => _audioClips = value; }
        public string CharacterName { get => _characterName; set => _characterName = value; }
        public DialogueFaceImageType FaceImageType { get => _faceImageType; set => _faceImageType = value; }
        public List<DialogueNodePort> DialogueNodePorts { get => _dialogueNodePorts; set => _dialogueNodePorts = value; }
        public Sprite FaceImage { get => _faceImage; set => _faceImage = value; }

        private TextField _textLanguages_Field;
        private ObjectField _audioClips_Field;
        private ObjectField _faceImage_Field;
        private Image _faceImagePreview;
        private TextField _characterName_Field;
        private EnumField _faceImageType_field;

        public DialogueNode()
        {

        }

        public DialogueNode(Vector2 Position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            base._editorWindow = editorWindow;
            base._graphView = graphView;

            title = "Dialogue";
            SetPosition(new Rect(Position, _defaultNodeSize));
            NodeGUID = Guid.NewGuid().ToString();
            AddInputPort("Input", Port.Capacity.Multi);

            foreach (LanguageType Language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                _textLanguages.Add(new LanguageGeneric<string>
                {
                    LanguageType = Language,
                    LanguageGenericType = string.Empty
                });

                _audioClips.Add(new LanguageGeneric<AudioClip>
                {
                    LanguageType = Language,
                    LanguageGenericType = null
                });
            }

            // face image field
            _faceImage_Field = new ObjectField
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = _faceImage
            };
            _faceImagePreview = new Image();
            _faceImagePreview.AddToClassList("FaceImagePreview");


            _faceImage_Field.RegisterValueChangedCallback(value =>
            {
                Sprite tmp = (Sprite)value.newValue;
                _faceImage = tmp;
                _faceImagePreview.image = (tmp != null ? tmp.texture : null);
            });
            _faceImage_Field.SetValueWithoutNotify(_faceImage);
            mainContainer.Add(_faceImagePreview);
            mainContainer.Add(_faceImage_Field);

            // face image enum field
            _faceImageType_field = new EnumField()
            {
                value = _faceImageType
            };
            _faceImageType_field.Init(_faceImageType);
            _faceImageType_field.RegisterValueChangedCallback(value =>
            {
                _faceImageType = (DialogueFaceImageType)value.newValue;
            });
            mainContainer.Add(_faceImageType_field);

            // audio field
            _audioClips_Field = new ObjectField()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = _audioClips.Find(audioClip => audioClip.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType,
            };
            _audioClips_Field.RegisterValueChangedCallback(value =>
            {
                _audioClips.Find(audioClip => audioClip.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue as AudioClip;
            });
            _audioClips_Field.SetValueWithoutNotify(_audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.SelectedLanguage).LanguageGenericType);
            mainContainer.Add(_audioClips_Field);

            // Text Name Label
            Label label_name = new Label("Name");
            label_name.AddToClassList("label_Name");
            label_name.AddToClassList("Label");
            mainContainer.Add(label_name);

            _characterName_Field = new TextField("");
            _characterName_Field.RegisterValueChangedCallback(value =>
            {
                _characterName = value.newValue;
            });
            _characterName_Field.SetValueWithoutNotify(_characterName);
            _characterName_Field.AddToClassList("TextName");
            mainContainer.Add(_characterName_Field);

            // Text Box
            Label lable_TextLanguages = new Label("Text Box");
            lable_TextLanguages.AddToClassList("label_TextLanguages");
            lable_TextLanguages.AddToClassList("Label");
            mainContainer.Add(lable_TextLanguages);

            // Text Field
            _textLanguages_Field = new TextField(string.Empty);
            _textLanguages_Field.RegisterValueChangedCallback(value =>
            {
                _textLanguages.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
            });
            _textLanguages_Field.SetValueWithoutNotify(
                _textLanguages.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType);
            _textLanguages_Field.multiline = true;
            _textLanguages_Field.AddToClassList("TextBox");
            mainContainer.Add(_textLanguages_Field);

            Button button = new Button() { text = "Add Choice" };
            button.clicked += () =>
            {
                AddChoicePort(this);
            };
            titleButtonContainer.Add(button);
        }

        override
        public void ReloadLanguage()
        {
            _textLanguages_Field.RegisterValueChangedCallback(value =>
            {
                _textLanguages.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
            });
            _textLanguages_Field.SetValueWithoutNotify(
                _textLanguages.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType);

            _audioClips_Field.RegisterValueChangedCallback(value =>
            {
                _audioClips.Find(audioClip => audioClip.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue as AudioClip;
            });

            foreach (DialogueNodePort nodePort in _dialogueNodePorts)
            {
                nodePort.TextField.RegisterValueChangedCallback(value =>
                {
                    nodePort.TextLanguages.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
                });
                nodePort.TextField.SetValueWithoutNotify(
                    nodePort.TextLanguages.Find(text => text.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType);
            }
        }

        override
        public void LoadValueInToField()
        {
            _textLanguages_Field.SetValueWithoutNotify(_textLanguages.Find(language => language.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType);
            _audioClips_Field.SetValueWithoutNotify(_audioClips.Find(language => language.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType);
            _faceImage_Field.SetValueWithoutNotify(_faceImage);
            _faceImageType_field.SetValueWithoutNotify(_faceImageType);
            _characterName_Field.SetValueWithoutNotify(_characterName);

            if(_faceImage != null) 
            {
                _faceImagePreview.image = ((Sprite)_faceImage_Field.value).texture;
            }
        }

        public Port AddChoicePort(BaseNode baseNode, DialogueNodePort dialogueNodePort = null)
        {
            Port newPort = GetPortInstance(Direction.Output);

            int outputPortCount = baseNode.outputContainer.Query("connector").ToList().Count;
            string outputPortName = $"Continue";

            DialogueNodePort newDialogueNodePort = new DialogueNodePort();
            newDialogueNodePort.PortGUID = Guid.NewGuid().ToString();
            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                newDialogueNodePort.TextLanguages.Add(new LanguageGeneric<string>
                {
                    LanguageType = language,
                    LanguageGenericType = outputPortName
                });
            }

            if (dialogueNodePort != null)
            {
                newDialogueNodePort.InputGUID = dialogueNodePort.InputGUID;
                newDialogueNodePort.OutputGUID = dialogueNodePort.OutputGUID;
                newDialogueNodePort.PortGUID = dialogueNodePort.PortGUID;

                foreach (LanguageGeneric<string> languageGeneric in dialogueNodePort.TextLanguages)
                {
                    newDialogueNodePort.TextLanguages.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
                }
            }

            // Text for the Port
            newDialogueNodePort.TextField = new TextField();
            newDialogueNodePort.TextField.RegisterValueChangedCallback(value =>
            {
                newDialogueNodePort.TextLanguages.Find(language => language.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType = value.newValue;
            });
            newDialogueNodePort.TextField.SetValueWithoutNotify(newDialogueNodePort.TextLanguages.Find(language => language.LanguageType == _editorWindow.SelectedLanguage).LanguageGenericType);
            newPort.contentContainer.Add(newDialogueNodePort.TextField);

            // Delete Button
            Button deleteButton = new Button(() => DeletePort(baseNode, newPort))
            {
                text = "X"
            };
            newPort.contentContainer.Add(deleteButton);

            //newDialogueNodePort.MyPort = newPort;
            newPort.portName = dialogueNodePort.PortGUID;                       // We use portName as PortID
            Label portNameLable = newPort.contentContainer.Q<Label>("type");    // Get Label in port aht is used to contain the port name
            portNameLable.AddToClassList("PortName");                           // Add a USS class to it so we can hide it in the editor window
            //baseNode.Remove(portNameLable);                                   


            DialogueNodePorts.Add(newDialogueNodePort);

            baseNode.outputContainer.Add(newPort);
            baseNode.RefreshExpandedState();
            baseNode.RefreshPorts();

            return newPort;
        }

        private void DeletePort(BaseNode node, Port port)
        {
            DialogueNodePort tmp = DialogueNodePorts.Find(tmpPort => tmpPort.PortGUID == port.portName);
            DialogueNodePorts.Remove(tmp);

            IEnumerable<Edge> portEdge = _graphView.edges.ToList().Where(edge => edge.output == port);

            if (portEdge.Any())
            {
                Edge edge = portEdge.First();
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                _graphView.RemoveElement(edge);
            }

            node.outputContainer.Remove(port);
            node.RefreshExpandedState();
            node.RefreshPorts();
        }
    }
}