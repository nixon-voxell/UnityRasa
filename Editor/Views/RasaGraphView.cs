/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies.
All rights reserved.
*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Searcher;
using UnityEditor.Experimental.GraphView;
using Voxell.Rasa;

namespace Voxell.UI
{
  public class RasaGraphView : GraphView
  {
    public new class UxmlFactory : UxmlFactory<RasaGraphView, GraphView.UxmlTraits> {}
    public Action<RasaNodeView> OnNodeSelected;
    public Action OnNodeUnSelected;
    public RasaTree rasaTree;

    private EditorWindow _editorWindow;
    private Vector2 _mousePosition;
    private List<Type> _nodeTypes;
    private List<SearcherItem> _searcherItems;
    private Dictionary<string, int> _nodeMap;

    public RasaGraphView()
    {
      Insert(0, new GridBackground());

      this.AddManipulator(new ContentZoomer());
      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new SelectionDropper());
      this.AddManipulator(new RectangleSelector());

      StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/voxell.rasa/Editor/RasaEditorWindow.uss");
      styleSheets.Add(styleSheet);

      // add undo redo for adding and removing nodes
      Undo.undoRedoPerformed += PopulateView;
      // when space bar is pressed
      nodeCreationRequest = NodeCreationRequest;
    }

    internal void Initialize(RasaTree rasaTree, EditorWindow editorWindow)
    {
      this.rasaTree = rasaTree;
      if (rasaTree.rootNode == null)
      {
        RootNode rootNode = ScriptableObject.CreateInstance<RootNode>();
        rootNode.Initialize("Entry Point", GUID.Generate().ToString(), new Vector2(20.0f, 50.0f));
        rasaTree.rootNode = rootNode;
        AssetDatabase.AddObjectToAsset(rasaTree.rootNode, rasaTree);
        AssetDatabase.SaveAssets();
      }
      _editorWindow = editorWindow;
    }

    internal void PopulateView()
    {
      if (rasaTree != null)
      {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        // create node views
        CreateNodeView(rasaTree.rootNode);
        for (int n=0; n < rasaTree.rasaNodes?.Count; n++)
          CreateNodeView(rasaTree.rasaNodes[n]);

        // create edges
        for (int n=0; n < rasaTree.rasaNodes?.Count; n++)
        {
          RasaNode childNode = rasaTree.rasaNodes[n];
          RasaNodeView childNodeView = GetNodeByGuid(childNode.guid) as RasaNodeView;

          if (childNode is ActionNode)
          {
            ActionNode actionNode = childNode as ActionNode;
            if (actionNode.parentNode != null)
            {
              RasaNodeView parentNodeView = GetNodeByGuid(actionNode.parentNode.guid) as RasaNodeView;
              Edge edge = parentNodeView.outputPorts[0].ConnectTo(childNodeView.inputPorts[0]);
              AddElement(edge);
            }
          }

          // get all field names in current child node
          Dictionary<string, int> chlidFieldMap = childNode.GenerateInputPortLocations();

          for (int f=0; f < childNode.fieldNames.Count; f++)
          {
            // get output port's field name
            string outputPortName = childNode.fieldNames[f];
            // get all ports that are connected to the output port
            Connection connection = childNode.connections[f];
            for (int c=0; c < connection.fieldNames.Count; c++)
            {
              string inputPortName = connection.fieldNames[c];
              RasaNode parentNode = connection.rasaNodes[c];
              Dictionary<string, int> parentFieldMap = parentNode.GenerateOutputPortLocations();
              RasaNodeView parentNodeView = GetNodeByGuid(parentNode.guid) as RasaNodeView;

              Edge edge = parentNodeView.outputPorts[parentFieldMap[inputPortName]].ConnectTo(
                childNodeView.inputPorts[chlidFieldMap[outputPortName]]);
              AddElement(edge);
            }
          }
        }
      }

      ClearSelection();
    }

    /// <summary>
    /// Recursively generate menu based on path name
    /// </summary>
    private SearcherItem RecursiveMenuGeneration(SearcherItem parentItem, int pathIdx, ref string[] paths)
    {
      SearcherItem item = new SearcherItem(paths[pathIdx]);
      int parentIdx;
      if (parentItem.Children.Contains(item)) parentIdx = parentItem.Children.IndexOf(item);
      else
      {
        parentIdx = parentItem.Children.Count;
        parentItem.AddChild(item);
      }

      if (++pathIdx < paths.Length)
        parentItem.Children[parentIdx] = RecursiveMenuGeneration(parentItem.Children[parentIdx], pathIdx, ref paths);
      return parentItem;
    }

    private void NodeCreationRequest(NodeCreationContext c)
    {
      _mousePosition = c.screenMousePosition - _editorWindow.position.position;
      _nodeTypes = TypeCache.GetTypesDerivedFrom<RasaNode>().ToList();

      // remove root node and all abstract nodes
      List<Type> nodeTypesToRemove = _nodeTypes.FindAll((t) => t.IsAbstract || t.IsGenericType);
      nodeTypesToRemove.ForEach((t) => _nodeTypes.Remove(t));
      _nodeTypes.Remove(typeof(RootNode));

      // create search menu
      _searcherItems = new List<SearcherItem>();
      _nodeMap = new Dictionary<string, int>();
      string[] pathNames = _nodeTypes.Select((t) => t.GetField("pathName").GetValue(null) as string).ToArray();

      for (int p=0; p < pathNames.Length; p++)
      {
        // split path names into individual section names
        string[] paths = pathNames[p].Split('/');
        _nodeMap.Add(paths.Last(), p);

        SearcherItem parentItem = new SearcherItem(paths[0]);
        int parentIdx;
        if (_searcherItems.Contains(parentItem)) parentIdx = _searcherItems.IndexOf(parentItem);
        else
        {
          parentIdx = _searcherItems.Count;
          _searcherItems.Add(parentItem);
        }

        if (paths.Length > 1)
          _searcherItems[parentIdx] = RecursiveMenuGeneration(_searcherItems[parentIdx], 1, ref paths);
      }

      // only display the search window when current graph view is focused
      if (EditorWindow.focusedWindow == _editorWindow)
      {
        SearcherWindow.Show(
          _editorWindow,
          _searcherItems, "Create Rasa Node",
          OnSearcherSelectEntry,
          _mousePosition
        );
      }
    }

    private bool OnSearcherSelectEntry(SearcherItem item)
    {
      if (item != null)
      {
        RasaNode rasaNode = ScriptableObject.CreateInstance(_nodeTypes[_nodeMap[item.Name]]) as RasaNode;
        Undo.RecordObject(rasaTree, "Rasa Tree (Add Node)");
        EditorUtility.SetDirty(rasaTree);
        CreateNode(rasaNode);
      }

      return true;
    }

    private void CreateNode(RasaNode rasaNode)
    {
      rasaNode.Initialize(
        ObjectNames.NicifyVariableName(rasaNode.GetType().Name),
        GUID.Generate().ToString(),
        contentViewContainer.WorldToLocal(_mousePosition)
      );

      rasaTree.rasaNodes.Add(rasaNode);
      CreateNodeView(rasaNode);
    }

    private void CreateNodeView(RasaNode node)
    {
      RasaNodeView nodeView = new RasaNodeView(node);
      nodeView.OnNodeSelected = OnNodeSelected;
      nodeView.OnNodeUnSelected = OnNodeUnSelected;
      AddElement(nodeView);
      // select the new node so that the inspector shows the content of the node
      ClearSelection();
      AddToSelection(nodeView);
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
      if (graphViewChange.elementsToRemove != null)
      {
        Undo.RecordObject(rasaTree, "Rasa Tree (Remove Element)");
        EditorUtility.SetDirty(rasaTree);
        graphViewChange.elementsToRemove.ForEach(elem =>
        {
          RasaNodeView nodeView = elem as RasaNodeView;
          if (nodeView != null) rasaTree.rasaNodes.Remove(nodeView.rasaNode);

          Edge edge = elem as Edge;
          if (edge != null)
          {
            // output port information
            Port outputPort = edge.output;
            RasaNodeView outputNodeView = outputPort.node as RasaNodeView;
            Type outputType = outputPort.portType;
            string outputName = outputPort.portName;

            // input port information
            Port inputPort = edge.input;
            RasaNodeView inputNodeView = inputPort.node as RasaNodeView;
            Type inputType = inputPort.portType;
            string inputName = inputPort.portName;

            inputNodeView.rasaNode.OnRemoveInputPort(outputNodeView.rasaNode, outputType, outputName);
            outputNodeView.rasaNode.OnRemoveOutputPort(inputNodeView.rasaNode, inputType, inputName);
          }
        });
      }

      if (graphViewChange.edgesToCreate != null)
      {
        Undo.RecordObject(rasaTree, "Rasa Tree (Create Edge)");
        EditorUtility.SetDirty(rasaTree);
        graphViewChange.edgesToCreate.ForEach(edge =>
        {
          // output port information
          Port outputPort = edge.output;
          RasaNodeView outputNodeView = outputPort.node as RasaNodeView;
          Type outputType = outputPort.portType;
          string outputName = outputPort.portName;

          // input port information
          Port inputPort = edge.input;
          RasaNodeView inputNodeView = edge.input.node as RasaNodeView;
          Type inputType = inputPort.portType;
          string inputName = inputPort.portName;

          inputNodeView.rasaNode.OnAddInputPort(outputNodeView.rasaNode, outputType, outputName);
          outputNodeView.rasaNode.OnAddOutputPort(inputNodeView.rasaNode, inputType, inputName);
        });
      }
      return graphViewChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
      return ports.Where((endPort) =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node &&
        (endPort.portType == startPort.portType ||
          (endPort.portType == typeof(object) && startPort.portType != typeof(bool)))).ToList();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
      Vector2 mousePosition = evt.mousePosition + _editorWindow.position.position;
      evt.menu.InsertAction(0, "Create Node", (a) =>
      {
        NodeCreationRequest(new NodeCreationContext
        {
          screenMousePosition = mousePosition,
          target = this,
          index = -1
        });
      });
    }
  }
}