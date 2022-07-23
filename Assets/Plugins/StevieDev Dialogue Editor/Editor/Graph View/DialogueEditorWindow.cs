using System.Collections;
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace StevieDev.DialogueEditor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueContainerSO _currentDialogueContainer;          // Current open dialogue container in dialogue editor window
        private DialogueGraphView _graphView;                           // Reference to GraphView Class
        private GraphSaveLoadUtility _saveAndLoad;                      // Reference to SaveAndLoad Class

        private LanguageType _selectedLanguage = LanguageType.English;  // Current selected language in the dialogue editor window
        private ToolbarMenu _languageDropDownMenu;                      // Languages toolbar menu in the top of the dialogue editor window
        private Label DialogueContainerName;                            // Name of the current open dialogue container
        private string _graphViewStyleSheet = "GraphViewStyleSheet";         // Name of the graph view style sheet

        /// <summary>
        /// Current selected language in the dialogue editor window
        /// </summary>
        public LanguageType SelectedLanguage { get => _selectedLanguage; set => _selectedLanguage = value; }

        // Callback attribute for opening an scriptable object asset in Unity
        // (e.g the callback is fired when double clicking an asset in the project browser)
        [OnOpenAsset(0)]
        private static bool ShowWindow(int instanceID, int line)
        {
            UnityEngine.Object item = EditorUtility.InstanceIDToObject(instanceID); // Find Unity object with this instandID and load it in

            if (item is DialogueContainerSO) // Check if item is DialgoueContainerSO Object
            {
                DialogueEditorWindow window = (DialogueEditorWindow)GetWindow(typeof(DialogueEditorWindow));   // Make a unity editor window of type DialogueEditorWindow
                window.titleContent = new GUIContent("Dialogue Editor");                                        // Name the Editor Window
                window._currentDialogueContainer = item as DialogueContainerSO;                                 // The DialogueContainerSO will be loaded into the editor window
                window.minSize = new Vector2(500, 250);                                                         // Starter size of the Editor Window
                window.Load();                                                                                  // Load in DialogueContainerSO in the editor window.
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
            _saveAndLoad = new GraphSaveLoadUtility(_graphView);
            StyleSheet styleSheet = Resources.Load<StyleSheet>(_graphViewStyleSheet);
            rootVisualElement.styleSheets.Add(styleSheet);
        }

        private void GenerateToolBar()
        {
            Toolbar toolbar = new Toolbar();

            Button saveBtn = new Button() { text = "Save" };
            saveBtn.clicked += Save;
            toolbar.Add(saveBtn);

            Button loadBtn = new Button() { text = "Load" };
            loadBtn.clicked += Load;
            toolbar.Add(loadBtn);

            //dropdown menu names
            _languageDropDownMenu = new ToolbarMenu();
            // Convert language enum into an array and create a button for each language
            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                _languageDropDownMenu.menu.AppendAction(
                    language.ToString(),  // language name 
                    new Action<DropdownMenuAction> // button action
                    (
                        x => Language(language)
                    )
                );
            }
            toolbar.Add(_languageDropDownMenu);

            // Nmae of current dialgoe container you have opened
            DialogueContainerName = new Label($"");
            toolbar.Add(DialogueContainerName);
            DialogueContainerName.AddToClassList("DialogueContainerName");

            rootVisualElement.Add(toolbar);
        }

        /// <summary>
        /// Will save the current changes to dialogue container
        /// </summary>
        private void Save()
        {
            if (_currentDialogueContainer == null)
            {
                EditorUtility.DisplayDialog("Dialogue Save Error", "No Dialogue Container to save with !", "OK");
                return;
            }
            else
            {
                Debug.Log($"Saved graph !");
                _saveAndLoad.Save(_currentDialogueContainer);
            }
        }

        /// <summary>
        /// Will load the current selected dialogue container
        /// </summary>
        private void Load()
        {
            Debug.Log($"Load");
            if (_currentDialogueContainer != null)
            {
                Language(LanguageType.English);
                DialogueContainerName.text = $"Name: {_currentDialogueContainer.name}";
                _saveAndLoad.Load(_currentDialogueContainer);
            }
        }

        /// <summary>
        /// Will change the language in the dialogue editor window
        /// </summary>
        /// <param name="language">Target language you want to change to</param> 
        private void Language(LanguageType language)
        {
            _languageDropDownMenu.text = $"Language : {language.ToString()}";
            _selectedLanguage = language;
            _graphView.ReloadLanguage();
        }
    }
}