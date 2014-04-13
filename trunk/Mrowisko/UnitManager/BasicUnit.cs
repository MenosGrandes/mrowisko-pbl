using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapManager;
using Microsoft.Xna.Framework;

namespace UnitManager
{
    public class BasicUnit :IUnit
    {
        public MapManager.LoadModel unitModel;
        State unitState;


        public BasicUnit(MapManager.LoadModel _unitModel)
        {
            unitModel = _unitModel;

        }
        enum State { Move,Rest,Work,Fight,Defence,Run,Search}
        public void Move(float x, float y, float z)
        {
            unitModel.Position = new Vector3(x, y, z);
            unitState = State.Move;

        }
    }
}
