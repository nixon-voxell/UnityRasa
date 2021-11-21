using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voxell.Rasa
{
  public sealed class LogNode : ActionNode
  {
    new public static string pathName = "Debug/Log";

    public override void OnEnable()
    {
      base.OnEnable();
      if (!fieldNames.Contains("object"))
      {
        fieldNames.Add("object");
        connections.Add(new Connection());
      }
    }

    protected override void OnStart(ref RasaNLP rasaNLP)
    {
      if (connections[0].fieldNames.Count == 1) Debug.Log(connections[0].GetValue(0));
    }
    protected override RasaState OnUpdate() => RasaState.Success;
    protected override void OnStop() {}

    public override List<PortInfo> CreateInputPorts()
    {
      List<PortInfo> portInfos = base.CreateInputPorts();
      portInfos.Add(new PortInfo(CapacityInfo.Single, typeof(object), "object", EdgeColor.obj));
      return portInfos;
    }

    public override bool OnAddInputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (base.OnAddInputPort(rasaNode, portType, portName)) return true;
      else
      {
        connections[0].Add(ref rasaNode, portName);
        return true;
      }
    }
    public override bool OnRemoveInputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (base.OnRemoveInputPort(rasaNode, portType, portName)) return true;
      else 
      {
        connections[0].RemoveAt(0);
        return true;
      }
    }
  }
}