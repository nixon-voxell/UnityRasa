using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voxell.Rasa
{
  [CreateAssetMenu(fileName = "NewRasaTree", menuName = "Rasa/Tree")]
  public class RasaTree : ScriptableObject
  {
    [TextArea(1, 5), Tooltip("Sample intents that could trigger this Rasa Tree Graph. (this is for training purposes)")]
    public string[] sampleIntents;

    // properties (external data input) for the rasa tree graph
    [HideInInspector] public List<string> propertyNames;
    [HideInInspector] public List<DataNode<object>> inputDataNodes;

    [HideInInspector] public RootNode rootNode;
    [HideInInspector] public List<RasaNode> rasaNodes = new List<RasaNode>();
    [HideInInspector] public RasaNode[] cachedNodes;

    private ActionNode runningNode;

    /// <summary>
    /// Generate property ID from property name
    /// </summary>
    /// <returns>property ID if property name is found, -1 if property name is not found</returns>
    public int PropertyToID(string propertyName)
    {
      if (propertyNames.Contains(propertyName)) return propertyNames.IndexOf(propertyName);
      else
      {
        Debug.LogError($"Property named `{propertyName}` not found.");
        return -1;
      }
    }

    public RasaState UpdateTree(ref RasaNLP rasaNLP)
    {
      if (rootNode == null)
      {
        Debug.LogError("Root Node not initialized yet. Open up the Rasa Tree Graph at least once to initialize the Root Node.");
        return RasaState.Failure;
      }

      if (runningNode.rasaState == RasaState.Idle) runningNode.UpdateNode(ref rasaNLP);
      else if (runningNode.rasaState == RasaState.Success)
      {
        if (runningNode.childNode != null)
          runningNode = runningNode.childNode;
        else return RasaState.Success;
      } else if (runningNode.rasaState == RasaState.Failure)
      {
        Debug.LogError($"Rasa Tree failed at node: {runningNode.name}");
        return RasaState.Failure;
      }

      return RasaState.Running;
    }

    public void ResetTree()
    {
      rootNode.rasaState = RasaState.Idle;
      for (int n=0; n < rasaNodes.Count; n++)
        rasaNodes[n].rasaState = RasaState.Idle;

      runningNode = rootNode;
    }

    public void Cache() => cachedNodes = rasaNodes.ToArray();
    public void ClearCache() => Array.Clear(cachedNodes, 0, cachedNodes.Length);
  }
}