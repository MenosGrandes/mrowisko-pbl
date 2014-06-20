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
namespace Logic.PathFinderManagerNamespace
{
    public static class PathFinderManager
    {

        public static int GridSize;


        public static Node[,] tileList;
        public static void PathFinderManagerInitialize(int _GridSize)
        {
            GridSize = _GridSize; 
            tileList = new Node[GridSize, GridSize];



            for (int i = 0; i < GridSize; i += 1)
            {
                for (int J = 0; J < GridSize; J += 1)
                {

                    tileList[J, i] = new Node(new Vector2(24 + J * 48, 24 + i * 48), true, new BoundingBox(new Vector3(J * 48, 10, i * 48), new Vector3(J * 48+ 48, StaticHelpers.StaticHelper.GetHeightAt(J * 48 + 48, i * 48 + 48)+3, i * 48 + 48)), new Vector2(J, i));
                }

            }
        
        }


        public static bool isWalkable(int x, int y)
        {
            return tileList[x, y].walkable;
        }
        public static bool isWalkable(Node curentNode)
        {
            return tileList[(int)curentNode.index.X, (int)curentNode.index.Y].walkable;
        }

        public static Node getNodeIntersected(Ray mouseRay)
        {
            foreach (Node q in tileList)
            {
                if ((mouseRay.Intersects(q.Box)) != null)
                {
                    //return q.Box.Min + (q.Box.Max - q.Box.Min) / 2;
                    return q;
                }


            }
            return PathFinderManager.tileList[0, 0];

        }
    }
}
