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

using UnityEngine;

namespace Voxell.Rasa
{
  public abstract class RandomNode<T> : ActionNode
  {
    public T selection;

    public override void OnEnable()
    {
      base.OnEnable();
      if (!fieldNames.Contains("data"))
      {
        fieldNames.Add("data");
        connections.Add(new Connection());
      }
    }

    protected override void OnStart()
    {
      rasaState = RasaState.Running;
      Connection connection = connections[0];
      int selectionIdx = Random.Range(0, connection.fieldNames.Count);
      selection = (T)connections[0].GetValue(selectionIdx);
    }
    protected override RasaState OnUpdate() => RasaState.Success;
    protected override void OnStop() {}
  }
}