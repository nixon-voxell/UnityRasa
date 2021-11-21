using System.Collections.Generic;

namespace Voxell.Rasa
{
  public class RootNode : ActionNode
  {
    protected override void OnStart(ref RasaNLP rasaNLP) => rasaState = RasaState.Running;
    protected override RasaState OnUpdate() => RasaState.Success;
    protected override void OnStop() {}

    public override List<PortInfo> CreateInputPorts() => new List<PortInfo>();
  }
}