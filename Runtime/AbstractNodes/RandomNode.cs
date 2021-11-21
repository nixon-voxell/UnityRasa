using System.Collections.Generic;
using UnityEngine;
using Voxell.Inspector;

namespace Voxell.Rasa
{
  public abstract class RandomNode<T> : ActionNode
  {
    [InspectOnly] public T selection;

    public override void OnEnable()
    {
      base.OnEnable();
      // connection of other ports to the data port at the random node
      if (!fieldNames.Contains("array"))
      {
        fieldNames.Add("array");
        connections.Add(new Connection());
      }
    }

    protected override void OnStart(ref RasaNLP rasaNLP)
    {
      rasaState = RasaState.Running;
      Connection connection = connections[0];
      T[] array = (T[])connection.GetValue(0);
      int selectionIdx = Random.Range(0, array.Length);
      selection = array[selectionIdx];
    }
    protected override RasaState OnUpdate() => RasaState.Success;
    protected override void OnStop() {}
  }
}