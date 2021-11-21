using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voxell.Rasa
{
  public abstract class ActionNode : RasaNode
  {
    [HideInInspector] public ActionNode parentNode;
    [HideInInspector] public ActionNode childNode;

    public RasaState UpdateNode(ref RasaNLP rasaNLP)
    {
      if (rasaState == RasaState.Idle)
      {
        OnStart(ref rasaNLP);
        rasaState = RasaState.Running;
      }

      rasaState = OnUpdate();

      if (rasaState == RasaState.Failure || rasaState == RasaState.Success)
        OnStop();

      return rasaState;
    }

    protected abstract void OnStart(ref RasaNLP rasaNLP);
    protected abstract void OnStop();
    protected abstract RasaState OnUpdate();

    public override List<PortInfo> CreateInputPorts()
      => new List<PortInfo> { new PortInfo(CapacityInfo.Single, typeof(bool), "flow", EdgeColor.flow) };

    public override List<PortInfo> CreateOutputPorts()
      => new List<PortInfo> { new PortInfo(CapacityInfo.Single, typeof(bool), "flow", EdgeColor.flow) };

    public override bool OnAddInputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (portType == typeof(bool))
      { parentNode = rasaNode as ActionNode; return true; }
      return false;
    }
    public override bool OnRemoveInputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (portType == typeof(bool))
      { parentNode = null; return true; }
      return false;
    }
    public override bool OnAddOutputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (portType == typeof(bool))
      { childNode = rasaNode as ActionNode; return true; }
      return false;
    }
    public override bool OnRemoveOutputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (portType == typeof(bool))
      { childNode = null; return true; }
      return false;
    }
  }
}
