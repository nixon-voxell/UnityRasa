using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voxell.Rasa
{
  public enum RasaState { Idle = 0, Running = 1, Success = 2, Failure = 3 }
  public enum CapacityInfo { Single = 0, Multi = 1 }

  public struct PortInfo
  {
    public CapacityInfo capacityInfo;
    public Type portType;
    public string portName;
    public Color color;

    public PortInfo(CapacityInfo capacityInfo, Type portType, string portName, Color color)
    {
      this.capacityInfo = capacityInfo;
      this.portType = portType;
      this.portName = portName;
      this.color = color;
    }
  }

  [Serializable]
  public class Connection
  {
    public List<RasaNode> rasaNodes;
    public List<string> fieldNames;

    public Connection()
    {
      this.rasaNodes = new List<RasaNode>();
      this.fieldNames = new List<string>();
    }

    public void Add(ref RasaNode rasaNode, string fieldName)
    {
      rasaNodes.Add(rasaNode);
      fieldNames.Add(fieldName);
    }

    public void RemoveAt(int idx)
    {
      rasaNodes.RemoveAt(idx);
      fieldNames.RemoveAt(idx);
    }

    public object GetValue(int idx)
      => rasaNodes[idx].GetType().GetField(fieldNames[idx]).GetValue(rasaNodes[idx]);
  }
}
