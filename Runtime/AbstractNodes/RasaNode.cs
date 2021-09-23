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
using Voxell.Inspector;

namespace Voxell.Rasa
{
  public abstract class RasaNode : ScriptableObject
  {
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public List<string> fieldNames;
    [HideInInspector] public List<Connection> connections;
    [InspectOnly] public RasaState rasaState = RasaState.Idle;

    public static string pathName = "New Node";

    public virtual void OnEnable()
    {
      if (connections == null)
        connections = new List<Connection>();
      if (fieldNames == null)
        fieldNames = new List<string>();
    }

    public void Initialize(string name, string guid, Vector2 position)
    {
      this.name = name;
      this.guid = guid;
      this.position = position;
    }

    /// <summary>
    /// Get port locations based on field name
    /// </summary>
    public Dictionary<string, int> GenerateInputPortLocations()
    {
      Dictionary<string, int> map = new Dictionary<string, int>();
      List<PortInfo> portInfos = CreateInputPorts();
      for (int p=0; p < portInfos.Count; p++)
        map.Add(portInfos[p].portName, p);

      return map;
    }
    /// <summary>
    /// Get port locations based on field name
    /// </summary>
    public Dictionary<string, int> GenerateOutputPortLocations()
    {
      Dictionary<string, int> map = new Dictionary<string, int>();
      List<PortInfo> portInfos = CreateOutputPorts();
      for (int p=0; p < portInfos.Count; p++)
        map.Add(portInfos[p].portName, p);

      return map;
    }

    #region Port Creation Infos
    /// <summary>
    /// Generate input port infos
    /// </summary>
    public abstract List<PortInfo> CreateInputPorts();
    /// <summary>
    /// Generate output port infos
    /// </summary>
    public abstract List<PortInfo> CreateOutputPorts();
    #endregion

    #region Connection Actions
    /// <summary>
    /// Actions to perform when input port is connected
    /// </summary>
    /// <returns>true if succeeds, otherwise, false</returns>
    public abstract bool OnAddInputPort(RasaNode rasaNode, Type portType, string portName);
    /// <summary>
    /// Actions to perform when input port is disconnected
    /// </summary>
    /// <returns>true if succeeds, otherwise, false</returns>
    public abstract bool OnRemoveInputPort(RasaNode rasaNode, Type portType, string portName);
    /// <summary>
    /// Actions to perform when output port is connected
    /// </summary>
    /// <returns>true if succeeds, otherwise, false</returns>
    public abstract bool OnAddOutputPort(RasaNode rasaNode, Type portType, string portName);
    /// <summary>
    /// Actions to perform when output port is disconnected
    /// </summary>
    /// <returns>true if succeeds, otherwise, false</returns>
    public abstract bool OnRemoveOutputPort(RasaNode rasaNode, Type portType, string portName);
    #endregion
  }
}
