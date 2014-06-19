using Logic;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
   public  class Node
    {
       public Node parent;
       public  Vector2 centerPosition;
       public float Height;
       public bool walkable;
       public Vector2 index;
       public float heuristicValue { get; set; }
       public Node(Vector2 centerPosition,bool walkable,BoundingBox box,Vector2 _Index)
       {
           this.centerPosition = centerPosition;
           this.walkable = walkable;
           this.Box = box;
           this.index = _Index;
           heuristicValue = 999999;
           Height = StaticHelpers.StaticHelper.GetHeightAt(centerPosition.X, centerPosition.Y);

       }
       public Node()
       { }
       public override string ToString()
       {
           return centerPosition + " " + walkable + " " +heuristicValue;
       }

       public BoundingBox Box { get; set; }
    }
}
