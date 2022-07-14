using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    private string styleSheetName = "GraphViewStyleSheet";
    private DialogueEditorWindow _editorWindow;

    public DialogueGraphView(DialogueEditorWindow editor)
    {
        _editorWindow = editor;

        StyleSheet tmpStylesheet = Resources.Load<StyleSheet>(styleSheetName);
        styleSheets.Add(tmpStylesheet);

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();        
    }

    public void LanguageReload()
    {
        List<DialogueNode> dialogeNodes = nodes.ToList().Where(node => node is DialogueNode).Cast<DialogueNode>().ToList();
        foreach(DialogueNode node in dialogeNodes)
        {
            node.ReloadLanguage();
        }
    }
}
