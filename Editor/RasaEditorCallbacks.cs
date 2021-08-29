using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Voxell.Rasa.UI
{
  public class RasaEditorCallbacks
  {
    internal static bool ShowRasaEditorWindow(string path)
    {
      string extension = System.IO.Path.GetExtension(path);
      if (extension != ".asset") return false;
      if (!(Selection.activeObject is RasaTree)) return false;
      RasaTree rasaTree = Selection.activeObject as RasaTree;
      string guid = AssetDatabase.AssetPathToGUID(path);

      RasaEditorWindow[] rasaWindows = Resources.FindObjectsOfTypeAll<RasaEditorWindow>();
      for (int w=0; w < rasaWindows.Length; w++)
      {
        if (rasaWindows[w].selectedGuid == guid)
        {
          rasaWindows[w].Focus();
          return true;
        }
      }

      RasaEditorWindow window = EditorWindow.CreateWindow<RasaEditorWindow>(rasaTree.name, typeof(RasaEditorWindow));
      window.Initialize(guid, rasaTree);
      window.Focus();
      return true;
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
      string path = AssetDatabase.GetAssetPath(instanceID);
      return ShowRasaEditorWindow(path);
    }
  }
}