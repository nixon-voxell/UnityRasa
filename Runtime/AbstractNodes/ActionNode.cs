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
  public abstract class ActionNode : RasaNode
  {
    [HideInInspector] public ActionNode parentNode;
    [HideInInspector] public ActionNode childNode;

    public RasaState UpdateNode(ref RasaNLP rasaNLP)
    {
      if (rasaState == RasaState.Idle)
      {
        OnStart(ref rasaNLP);
        rasaState = RasaState.Running;
      }

      rasaState = OnUpdate();

      if (rasaState == RasaState.Failure || rasaState == RasaState.Success)
        OnStop();

      return rasaState;
    }

    protected abstract void OnStart(ref RasaNLP rasaNLP);
    protected abstract void OnStop();
    protected abstract RasaState OnUpdate();

    public override List<PortInfo> CreateInputPorts()
      => new List<PortInfo> { new PortInfo(CapacityInfo.Single, typeof(bool), "flow", EdgeColor.flow) };

    public override List<PortInfo> CreateOutputPorts()
      => new List<PortInfo> { new PortInfo(CapacityInfo.Single, typeof(bool), "flow", EdgeColor.flow) };

    public override bool OnAddInputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (portType == typeof(bool))
      { parentNode = rasaNode as ActionNode; return true; }
      return false;
    }
    public override bool OnRemoveInputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (portType == typeof(bool))
      { parentNode = null; return true; }
      return false;
    }
    public override bool OnAddOutputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (portType == typeof(bool))
      { childNode = rasaNode as ActionNode; return true; }
      return false;
    }
    public override bool OnRemoveOutputPort(RasaNode rasaNode, Type portType, string portName)
    {
      if (portType == typeof(bool))
      { childNode = null; return true; }
      return false;
    }
  }
}
