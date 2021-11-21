using System;
using System.Collections.Generic;

namespace Voxell.Rasa
{
  public abstract class DataNode<T> : RasaNode
  {
    new public static string pathName = "Data";
    public T data;

    public override List<PortInfo> CreateInputPorts() => new List<PortInfo>();

    public override bool OnAddInputPort(RasaNode rasaNode, Type portType, string portName) => true;
    public override bool OnRemoveInputPort(RasaNode rasaNode, Type portType, string portName) => true;
    public override bool OnAddOutputPort(RasaNode rasaNode, Type portType, string portName) => true;
    public override bool OnRemoveOutputPort(RasaNode rasaNode, Type portType, string portName) => true;
  }
}
