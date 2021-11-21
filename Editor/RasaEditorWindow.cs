using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Voxell.UI;

namespace Voxell.Rasa.UI
{
  public class RasaEditorWindow : EditorWindow
  {
    [SerializeField] public string selectedGuid;
    [SerializeField] public RasaTree rasaTree;

    private RasaGraphView _graphView;
    private InspectorView _inspectorView;
    private Button _saveButton, _showButton;

    public void Initialize(string guid, RasaTree rasaTree)
    {
      this.selectedGuid = guid;
      this.rasaTree = rasaTree;
      _graphView.Initialize(rasaTree, this);
      _graphView.PopulateView();
    }

    void CreateGUI()
    {
      // Each editor window contains a root VisualElement object
      VisualElement root = rootVisualElement;

      // Import UXML
      VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/voxell.rasa/Editor/RasaEditorWindow.uxml");
      visualTree.CloneTree(root);

      _graphView = root.Q<RasaGraphView>();
      _inspectorView = root.Q<InspectorView>();

      _saveButton = root.Q<Button>("save-asset");
      _saveButton.clickable = new Clickable(SaveAsset);

      _showButton = root.Q<Button>("show-in-project");
      _showButton.clickable = new Clickable(() => EditorGUIUtility.PingObject(rasaTree));

      _graphView.OnNodeSelected = (nodeView) => _inspectorView.UpdateSelection(nodeView);
      _graphView.OnNodeUnSelected = _inspectorView.ClearSelection;
      if (rasaTree != null) _graphView.Initialize(rasaTree, this);
      _graphView.PopulateView();
    }

    private void SaveAsset()
    {
      // remove all previous cached nodes
      for (int n=0; n < rasaTree.cachedNodes?.Length; n++)
      {
        RasaNode rasaNode = rasaTree.cachedNodes[n];
        if (AssetDatabase.Contains(rasaNode))
          AssetDatabase.RemoveObjectFromAsset(rasaTree.cachedNodes[n]);
      }

      for (int n=0; n < rasaTree.rasaNodes.Count; n++)
      {
        RasaNode rasaNode = rasaTree.rasaNodes[n];
        if (!AssetDatabase.Contains(rasaNode))
          AssetDatabase.AddObjectToAsset(rasaNode, rasaTree);
      }

      AssetDatabase.SaveAssets();
      rasaTree.Cache();
    }

    void OnGUI()
    {}
  }
}