using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

public class DialogueEditorWindow : EditorWindow
{
    private DialogueContainerSO _currentDialogueContainer;
    private DialogueGraphView _graphView;
    private ToolbarMenu _toolbarMenu;
    private Label DialogueContainerName;
    private LanguageType _languageType = LanguageType.English;
    public LanguageType LanguageType { get => _languageType; set => _languageType = value; }

    [OnOpenAsset(1)]
    private static bool ShowWindow(int _instanceID, int line)
    {
        UnityEngine.Object item = EditorUtility.InstanceIDToObject(_instanceID);

        if(item is DialogueContainerSO)
        {
            DialogueEditorWindow window = (DialogueEditorWindow) GetWindow(typeof(DialogueEditorWindow));
            window.titleContent = new GUIContent("Dialogue Editor");
            window._currentDialogueContainer = item as DialogueContainerSO;
            window.minSize = new Vector2(500, 250);
            window.Load();
        }

        return false;
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
        Load();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this);
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolBar()
    {
        StyleSheet styleSheet = Resources.Load<StyleSheet>($"GraphViewStyleSheet");
        rootVisualElement.styleSheets.Add(styleSheet);

        Toolbar toolbar = new Toolbar();

        Button saveBtn = new Button() { text = "Save" };
        saveBtn.clicked += Save;
        toolbar.Add(saveBtn);

        Button loadBtn = new Button() { text = "Load" };
        saveBtn.clicked += Load;
        toolbar.Add(loadBtn);

        //dropdown menu names
        _toolbarMenu = new ToolbarMenu();
        foreach(LanguageType language in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
        {
            _toolbarMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>
                (
                    x => Language(language, _toolbarMenu)
                )
            );
        }
        toolbar.Add(_toolbarMenu);

        DialogueContainerName = new Label($"");
        toolbar.Add(DialogueContainerName);
        DialogueContainerName.AddToClassList("DialogueContainerName");


        rootVisualElement.Add(toolbar);
    }

    private void Save()
    {
        Debug.Log($"Save");
    }

    private void Load()
    {
        Debug.Log($"Load");
        if(_currentDialogueContainer != null)
        {
            Language(LanguageType.English, _toolbarMenu);
            DialogueContainerName.text = $"Name: {_currentDialogueContainer.name}";
        }
    }

    private void Language(LanguageType language, ToolbarMenu toolbarMenu)
    {
        toolbarMenu.text = $"Language : {language.ToString()}";
    }
}
