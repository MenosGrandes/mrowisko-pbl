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
       public float f ;
       public float ManhatanFromStart;
       public float ManhatanFromStop;
       public bool haveMineral=false;
       public bool haveBuilding = false;  

       public Node(Vector2 centerPosition,bool walkable,BoundingBox box,Vector2 _Index)
       {
           this.centerPosition = centerPosition;
           this.walkable = walkable;
           this.Box = box;
           this.index = _Index;
           Height = StaticHelpers.StaticHelper.GetHeightAt(centerPosition.X, centerPosition.Y);

       }
       public Node()
       { }
       public override string ToString()
       {
           return centerPosition + " " + walkable + " " +f;
       }
       public void CalcF()
       {
           f = ManhatanFromStart + ManhatanFromStop;
       }

       public BoundingBox Box { get; set; }
    }
}
