using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Map;
using Logic;

namespace PathFinder
{
    public class PathFinderManager
    {
        public List<Tile> tileList = new List<Tile>();
        public PathFinderManager(List<InteractiveModel> models)
        {

            foreach (QuadNode a in Map.QuadNodeController.QuadNodeList2)
            {
                bool _walkable = true;
                Vector3 center = a.Bounds.Min + (a.Bounds.Max - a.Bounds.Min) / 2;
                foreach (InteractiveModel model in models)
                {
                    
                    if (a.Bounds.Intersects(model.Model.BoundingSphere))
                    {
                        _walkable = false;
                        tileList.Add(new Tile(center, _walkable));
                        break;

                    }
                    

                }
                 if(_walkable==true)
                     tileList.Add(new Tile(center, _walkable));

            }

        }
    }
}
