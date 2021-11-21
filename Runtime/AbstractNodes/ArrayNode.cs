using Voxell.Inspector;

namespace Voxell.Rasa
{
  public abstract class ArrayNode<T> : ActionNode
  {
    [InspectOnly] public T[] array;

    public override void OnEnable()
    {
      base.OnEnable();
      // connection of other ports to the data port
      if (!fieldNames.Contains("data"))
      {
        fieldNames.Add("data");
        connections.Add(new Connection());
      }
    }

    protected override void OnStart(ref RasaNLP rasaNLP)
    {
      rasaState = RasaState.Running;
      Connection connection = connections[0];
      int arrayLength = connection.rasaNodes.Count;
      array = new T[arrayLength];
      for (int c=0; c < arrayLength; c++)
        array[c] = ((T)connection.GetValue(c));
    }
    protected override RasaState OnUpdate() => RasaState.Success;
    protected override void OnStop() {}
  }
}