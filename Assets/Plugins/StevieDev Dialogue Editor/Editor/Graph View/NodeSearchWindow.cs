using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DialogueEditorWindow _editorWindow;
    private DialogueGraphView _graphView;

    private Texture2D _iconImage;

    public void Configure(DialogueEditorWindow editor, DialogueGraphView graphView)
    {
        _editorWindow = editor;
        _graphView = graphView;
        
        // Icon image is not used
        // Used to create space for styling
        // TODO : Find a better way
        _iconImage = new Texture2D(1, 1);
        _iconImage.SetPixel(0, 0, new Color(0, 0, 0, 0));
        _iconImage.Apply();
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> tree = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Dialogue Editor"), 0), 
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),

            AddNodeSearch("Start Node", new StartNode()),
            AddNodeSearch("Dialogue Node", new DialogueNode()),
            AddNodeSearch("Event Node", new EventNode()),
            AddNodeSearch("End Node", new EndNode()),
        };
        return tree;
    }

    private SearchTreeEntry AddNodeSearch(string name, BaseNode baseNode)
    {
        SearchTreeEntry tmp = new SearchTreeEntry(new GUIContent(name, _iconImage))
        {
            level = 2,
            userData = baseNode
        };
        return tmp;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        // get mouse position on the screen
        Vector2 mousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo
            (_editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);
        
        // use mouse position to calculate position within the graph view
        Vector2 graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePosition);

        return CheckForNodeType(searchTreeEntry, graphMousePosition);
    }

    /// <summary>
    /// Create a node based on the selected node type
    /// </summary>
    /// <param name="searchTreeEntry"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool CheckForNodeType(SearchTreeEntry searchTreeEntry, Vector2 position)
    {
        switch (searchTreeEntry.userData)
        {
            case StartNode node:
                _graphView.AddElement(_graphView.CreateStart(position));
                return true;
            case DialogueNode node:
                _graphView.AddElement(_graphView.CreateDialogue(position));
                return true;
            case EventNode node:
                _graphView.AddElement(_graphView.CreateEvent(position));
                return true;
            case EndNode node:
                _graphView.AddElement(_graphView.CreateEnd(position));
                return true;
        }
        return false;
    }
}