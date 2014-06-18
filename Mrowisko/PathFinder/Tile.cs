using Logic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathFinder
{
   public  class Tile
    {
     public  Vector3 centerPosition;
       public bool walkable;
       public Tile(Vector3 centerPosition,bool walkable)
       {
           this.centerPosition = centerPosition;
           this.walkable = walkable;
       }

    }
}
