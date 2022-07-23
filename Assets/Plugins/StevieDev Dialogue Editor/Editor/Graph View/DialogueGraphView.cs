using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    private string _graphViewStyleSheet = "GraphViewStyleSheet";
    private DialogueEditorWindow _editorWindow;
    private NodeSearchWindow _searchWindow;

    public DialogueGraphView(DialogueEditorWindow editor)
    {
        _editorWindow = editor;

        StyleSheet tmpStylesheet = Resources.Load<StyleSheet>(_graphViewStyleSheet);
        styleSheets.Add(tmpStylesheet);

        // Adding the ability to zoom in and out on the graph view
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());      // The ability to drag nodes around
        this.AddManipulator(new SelectionDragger());    // The abilty to drag all selected nodes around
        this.AddManipulator(new RectangleSelector());   // The abilty to drag select a rectangle area
        this.AddManipulator(new FreehandSelector());    // the abilty to select a single node

        // Add grid background
        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddSearchWindow();
    }

    /// <summary>
    /// Allow the abilty to search and create a selection of nodes
    /// </summary>
    private void AddSearchWindow()
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Configure(_editorWindow, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    /// <summary>
    /// Reload the current selected language.
    /// Normally used when changing language.
    /// </summary>
    public void ReloadLanguage()
    {
        List<DialogueNode> dialogeNodes = nodes.ToList().Where(node => node is DialogueNode).Cast<DialogueNode>().ToList();
        foreach (DialogueNode node in dialogeNodes)
        {
            node.ReloadLanguage();
        }
    }

    override  // this is a graph view method that where we inform which nodes can be connected to one another
    public List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        Port startPortView = startPort;

        ports.ForEach((port) =>
        {
            Port portView = port;

            // First a port cannot connect to itself
            // Then it cannot connect to a port on the same node
            // Lastly a input node cannot connect to another input node and an output node cannot connect output node
            if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != port.direction)
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