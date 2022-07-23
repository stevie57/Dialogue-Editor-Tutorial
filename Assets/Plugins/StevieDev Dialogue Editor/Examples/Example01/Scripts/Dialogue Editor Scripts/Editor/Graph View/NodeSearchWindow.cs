using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DialogueEditorWindow _editorWindow;
    private DialogueGraphView _graphView;

    private Texture2D _pic;

    public void Configure(DialogueEditorWindow editor, DialogueGraphView graphView)
    {
        _editorWindow = editor;
        _graphView = graphView;

        _pic = new Texture2D(1, 1);
        _pic.SetPixel(0, 0, new Color(0, 0, 0, 0));
        _pic.Apply();
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> tree = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),

            AddNodeSearch("Start Node", new StartNode()),
            AddNodeSearch("Dialogue Node", new DialogueNode()),
            AddNodeSearch("Event Node", new EventNode()),
            AddNodeSearch("End Node", new EndNode()),
        };


        return tree;
    }

    private SearchTreeEntry AddNodeSearch(string name, BaseNode baseNode)
    {
        SearchTreeEntry tmp = new SearchTreeEntry(new GUIContent(name, _pic))
        {
            level = 2,
            userData = baseNode
        };
        return tmp;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        Vector2 mousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo
            (
                _editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position
            );
        Vector2 graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePosition);

        return CheckForNodeType(searchTreeEntry, graphMousePosition);
    }

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