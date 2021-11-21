using System.Collections.Generic;

namespace Voxell.Rasa
{
  public sealed class StringNode : DataNode<string>
  {
    new public static string pathName = "Data/String";

    public override List<PortInfo> CreateOutputPorts()
    {
      return new List<PortInfo>()
      { new PortInfo(CapacityInfo.Multi, typeof(string), "data", EdgeColor.str) };
    }
  }
}
