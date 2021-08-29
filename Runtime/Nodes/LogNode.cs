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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voxell.Rasa
{
  public sealed class LogNode : ActionNode
  {
    new public static string pathName = "Debug/Log";

    public override void OnEnable()
    {
      base.OnEnable();
      if (!fieldNames.Contains("object"))
      {
        fieldNames.Add("object");
        connections.Add(new Connection());
      }
    }

    protected override void OnStart()
    {
      if (connections[0].fieldNames.Count == 1) Debug.Log(connections[0].GetValue(0));
    }
    protected override RasaState OnUpdate() => RasaState.Success;
    protected override void OnStop() {}

    public override List<PortInfo> CreateInputPorts()
    {
      List<PortInfo> portInfos = base.CreateInputPorts();
      portInfos.Add(new PortInfo(CapacityInfo.Single, typeof(object), "object", EdgeColor.obj));
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