using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
   public class UnitFormation
    {
       private List<Unit> movementOrder;
       public List<Unit> MovementOrder
       {
           get { return movementOrder; }
           set { movementOrder = value; }
       }

        private List<Unit> units = new List<Unit>();

        public List<Unit> Units
        {
            get { return units; }
            set { units = value; }
        }

        private Unit leader;

        public Unit Leader
        {
            get { return leader; }
            set { leader = value; }
        }
       public UnitFormation(List<Unit> units)
       {
           
               this.units = units;
               this.leader = units.First();
               this.leader.IfLeader = true;
           

       }

       public void formationSetOff()
       {
           movementOrder = new List<Unit>();
           if (units.Count > 1)
           {
                foreach (Unit unit in units)
               {
                   if (!unit.IfLeader)
                       movementOrder.Add(unit);
                    }
               //movementOrder.Reverse();      

                for (int i = 0; i < MovementOrder.Count; i++)
                {
                    MovementOrder[i].destination = new Microsoft.Xna.Framework.Vector2(units[i].Model.Position.X, units[i].Model.Position.Z);
                    MovementOrder[i].Moving = true;
                    
                }
           }


       }

      
       
       
    }
}
