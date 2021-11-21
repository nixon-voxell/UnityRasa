using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Voxell.Rasa;

namespace Voxell.UI
{
  public class InspectorView : VisualElement
  {
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {}
    private Editor _editor;

    public void UpdateSelection(RasaNodeView nodeView)
    {
      Clear();
      Object.DestroyImmediate(_editor);

      _editor = Editor.CreateEditor(nodeView.rasaNode);
      IMGUIContainer container = new IMGUIContainer(_editor.OnInspectorGUI);
      Add(container);
    }

    public void ClearSelection()
    {
      Clear();
      Object.DestroyImmediate(_editor);
    }
  }
}