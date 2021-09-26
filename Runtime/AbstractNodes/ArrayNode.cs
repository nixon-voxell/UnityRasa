/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies.
All rights reserved.
*/

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