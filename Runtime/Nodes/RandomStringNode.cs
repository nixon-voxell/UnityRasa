using System;
using System.Collections.Generic;

namespace Voxell.Rasa
{
  public class RandomStringNode : RandomNode<string>
  {
    new public static string pathName = "Random/Random String";

    public override List<PortInfo> CreateInputPorts()
    {
      List<PortInfo> portInfos = base.CreateInputPorts();
      portInfos.Add(new PortInfo(CapacityInfo.Single, typeof(List<string>), "array", EdgeColor.strList));
      return portInfos;
    }

    public override List<PortInfo> CreateOutputPorts()
    {
      List<PortInfo> portInfos = base.CreateOutputPorts();
      portInfos.Add(new PortInfo(CapacityInfo.Multi, typeof(string), "selection", EdgeColor.str));
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