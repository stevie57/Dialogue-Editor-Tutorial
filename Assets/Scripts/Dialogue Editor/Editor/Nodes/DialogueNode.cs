using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    private List<LanguageGeneric<string>> _texts = new List<LanguageGeneric<string>>();
    private List<LanguageGeneric<AudioClip>> _audioClips = new List<LanguageGeneric<AudioClip>>();
    private Sprite _faceImage;
    private string _name = string.Empty;
    private DialogueFaceImageType _faceImageType;
    private List<DialogueNodePort> _dialogueNodePorts = new List<DialogueNodePort>();

    public List<LanguageGeneric<string>> Texts { get => _texts; set => _texts = value; }
    public List<LanguageGeneric<AudioClip>> AudioClips { get => _audioClips; set => _audioClips = value; }
    public string Name { get => _name; set => _name = value; }
    public DialogueFaceImageType FaceImageType { get => _faceImageType; set => _faceImageType = value; }
    public List<DialogueNodePort> DialogueNodePorts { get => _dialogueNodePorts; set => _dialogueNodePorts = value; }
    public Sprite FaceImage { get => _faceImage; set => _faceImage = value; }

    private TextField texts_Field;
    private ObjectField audioClips_Field;
    private ObjectField faceImage_Field;
    private TextField name_Field;
    private EnumField faceImageType_field;

    public DialogueNode()
    {

    }

    public DialogueNode(Vector2 Position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        _editorWindow = editorWindow;
        _graphView = graphView;

        title = "Dialogue";
        SetPosition(new Rect(Position, _defaultNodeSize));
        NodeGUID = Guid.NewGuid().ToString();
        AddInputPort("Input", Port.Capacity.Multi);

        foreach (LanguageType Language in (LanguageType[])Enum.GetValues(typeof(LanguageType))) 
        {
            _texts.Add(new LanguageGeneric<string>
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
        faceImage_Field = new ObjectField
        {
            objectType = typeof(Sprite),
            allowSceneObjects = false,
            value = _faceImage
        };
        faceImage_Field.RegisterValueChangedCallback(value =>
        {
            _faceImage = (Sprite)value.newValue;
        });
        faceImage_Field.SetValueWithoutNotify(_faceImage);
        mainContainer.Add(faceImage_Field);

        // face image enum field
        faceImageType_field = new EnumField()
        {
            value = _faceImageType
        };
        faceImageType_field.Init(_faceImageType);
        faceImageType_field.RegisterValueChangedCallback(value =>
        {
            _faceImageType = (DialogueFaceImageType) value.newValue ;
        });
        mainContainer.Add(faceImageType_field);

        // audio field
        audioClips_Field = new ObjectField()
        {
            objectType = typeof(AudioClip),
            allowSceneObjects = false,
            value = _audioClips.Find(audioClip => audioClip.LanguageType == _editorWindow.LanguageType).LanguageGenericType,
        };
        audioClips_Field.RegisterValueChangedCallback(value =>
        {
            _audioClips.Find(audioClip => audioClip.LanguageType == _editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip;
        });
        audioClips_Field.SetValueWithoutNotify(_audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        mainContainer.Add(audioClips_Field);
        
        // Text Name Label
        Label label_name = new Label("Name");
        label_name.AddToClassList("label_Name");
        label_name.AddToClassList("Label");
        mainContainer.Add(label_name);

        name_Field = new TextField("");
        name_Field.RegisterValueChangedCallback(value =>
        {
            _name = value.newValue;
        });
        name_Field.SetValueWithoutNotify(_name);
        name_Field.AddToClassList("TextName");
        mainContainer.Add(name_Field);

        //name_Field = new TextField("Name");
        //name_Field.RegisterValueChangedCallback(value =>
        //{
        //    _name = value.newValue;
        //});
        //name_Field.SetValueWithoutNotify(_name);
        //name_Field.AddToClassList("TextName");
        //mainContainer.Add(name_Field);

        // Text Box
        Label lable_texts = new Label("Text Box");
        lable_texts.AddToClassList("label_texts");
        lable_texts.AddToClassList("Label");
        mainContainer.Add(lable_texts);

        // Text Field
        texts_Field = new TextField(string.Empty);
        texts_Field.RegisterValueChangedCallback(value => 
        {
            _texts.Find(text => text.LanguageType == _editorWindow.LanguageType).LanguageGenericType = value.newValue;
        });
        texts_Field.SetValueWithoutNotify(
            _texts.Find(text => text.LanguageType == _editorWindow.LanguageType).LanguageGenericType);
        texts_Field.multiline = true;
        texts_Field.AddToClassList("TextBox");
        mainContainer.Add(texts_Field);

        Button button = new Button()
        {
            text = "Add Choice"
        };
        button.clicked += () =>
        {
            AddChoicePort(this);
        };
        titleButtonContainer.Add(button);
    }

    public void ReloadLanguage()
    {
        texts_Field.RegisterValueChangedCallback(value =>
        {
            _texts.Find(text => text.LanguageType == _editorWindow.LanguageType).LanguageGenericType = value.newValue;
        });
        texts_Field.SetValueWithoutNotify(
            _texts.Find(text => text.LanguageType == _editorWindow.LanguageType).LanguageGenericType);

        audioClips_Field.RegisterValueChangedCallback(value =>
        {
            _audioClips.Find(audioClip => audioClip.LanguageType == _editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip;
        });

        foreach(DialogueNodePort nodePort in _dialogueNodePorts)
        {
            nodePort.TextField.RegisterValueChangedCallback(value =>
            {
                nodePort.TextLanguages.Find(text => text.LanguageType == _editorWindow.LanguageType).LanguageGenericType = value.newValue;
            });
            nodePort.TextField.SetValueWithoutNotify(
                nodePort.TextLanguages.Find(text => text.LanguageType == _editorWindow.LanguageType).LanguageGenericType);
        }
    }

    override
    public void LoadValueInToField()
    {
        texts_Field.SetValueWithoutNotify(_texts.Find(language => language.LanguageType == _editorWindow.LanguageType).LanguageGenericType);

        audioClips_Field.SetValueWithoutNotify(_audioClips.Find(language => language.LanguageType == _editorWindow.LanguageType).LanguageGenericType);

        faceImage_Field.SetValueWithoutNotify(_faceImage);

        faceImageType_field.SetValueWithoutNotify(_faceImageType);

        name_Field.SetValueWithoutNotify(_name);
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

        if(dialogueNodePort != null)
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
            newDialogueNodePort.TextLanguages.Find(language => language.LanguageType == _editorWindow.LanguageType).LanguageGenericType = value.newValue;
        });
        newDialogueNodePort.TextField.SetValueWithoutNotify(newDialogueNodePort.TextLanguages.Find(language => language.LanguageType == _editorWindow.LanguageType).LanguageGenericType);
        newPort.contentContainer.Add(newDialogueNodePort.TextField);

        // Delete Button
        Button deleteButton = new Button( () => DeletePort(baseNode, newPort)) 
        { 
            text = "X" 
        };
        newPort.contentContainer.Add(deleteButton);

        newDialogueNodePort.MyPort = newPort;
        newPort.portName = String.Empty;

        DialogueNodePorts.Add(newDialogueNodePort);

        baseNode.outputContainer.Add(newPort);
        baseNode.RefreshExpandedState();
        baseNode.RefreshPorts();

        return newPort;
    }

    private void DeletePort(BaseNode node, Port port)
    {
        DialogueNodePort tmp = DialogueNodePorts.Find(tmpPort => tmpPort.MyPort == port);
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