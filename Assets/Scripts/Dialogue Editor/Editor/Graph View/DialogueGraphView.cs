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
    private NodeSearchWindow _searchWindow;

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

        AddSearchWindow();
    }

    private void AddSearchWindow()
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Configure(_editorWindow, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    public void LanguageReload()
    {
        List<DialogueNode> dialogeNodes = nodes.ToList().Where(node => node is DialogueNode).Cast<DialogueNode>().ToList();
        foreach (DialogueNode node in dialogeNodes)
        {
            node.ReloadLanguage();
        }
    }

    override
    public List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        Port startPortView = startPort;

        ports.ForEach((port) =>
        {
            Port portView = port;
            if (startPort != portView && startPortView.node != portView.node && startPortView.direction != port.direction)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public StartNode CreateStart(Vector2 position)
    {
        return new StartNode(position, _editorWindow, this);
    }

    public DialogueNode CreateDialogue(Vector2 position)
    {
        return new DialogueNode(position, _editorWindow, this);
    }
    public EventNode CreateEvent(Vector2 position)
    {
        return new EventNode(position, _editorWindow, this);
    }
    public EndNode CreateEnd(Vector2 position)
    {
        return new EndNode(position, _editorWindow, this);
    }
}
