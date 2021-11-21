using System;
using System.Collections.Generic;

namespace Voxell.Rasa
{
  public class MaximumEntropyTokenizerNode : ArrayNode<string>
  {
    new public static string pathName = "NLP/Maximum Entropy Tokenizer";

    public override List<PortInfo> CreateInputPorts()
    {
      List<PortInfo> portInfos = base.CreateInputPorts();
      portInfos.Add(new PortInfo(CapacityInfo.Single, typeof(string), "data", EdgeColor.str));
      return portInfos;
    }

    public override List<PortInfo> CreateOutputPorts()
    {
      List<PortInfo> portInfos = base.CreateOutputPorts();
      portInfos.Add(new PortInfo(CapacityInfo.Multi, typeof(List<string>), "array", EdgeColor.strList));
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

    protected override void OnStart(ref RasaNLP rasaNLP)
    {
      rasaState = RasaState.Running;
      Connection connection = connections[0];
      string text = (string)connection.GetValue(0);
      array = rasaNLP.maximumEntropyTokenizer.Tokenize(text);
    }
  }
}