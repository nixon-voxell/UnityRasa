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
  public enum RasaState { Idle = 0, Running = 1, Success = 2, Failure = 3 }
  public enum CapacityInfo { Single = 0, Multi = 1 }

  public struct PortInfo
  {
    public CapacityInfo capacityInfo;
    public Type portType;
    public string portName;
    public Color color;

    public PortInfo(CapacityInfo capacityInfo, Type portType, string portName, Color color)
    {
      this.capacityInfo = capacityInfo;
      this.portType = portType;
      this.portName = portName;
      this.color = color;
    }
  }

  [Serializable]
  public class Connection
  {
    public List<RasaNode> rasaNodes;
    public List<string> fieldNames;

    public Connection()
    {
      this.rasaNodes = new List<RasaNode>();
      this.fieldNames = new List<string>();
    }

    public void Add(ref RasaNode rasaNode, string fieldName)
    {
      rasaNodes.Add(rasaNode);
      fieldNames.Add(fieldName);
    }

    public void RemoveAt(int idx)
    {
      rasaNodes.RemoveAt(idx);
      fieldNames.RemoveAt(idx);
    }

    public object GetValue(int idx)
      => rasaNodes[idx].GetType().GetField(fieldNames[idx]).GetValue(rasaNodes[idx]);
  }
}
