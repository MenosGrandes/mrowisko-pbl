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

                    tileList[J, i] = new Node(new Vector2(24 + J * 48, 24 + i * 48), true, new BoundingBox(new Vector3(J * 48, 10, i * 48), new Vector3(J * 48 + 24, StaticHelpers.StaticHelper.GetHeightAt(J * 48 + 24, i * 48 + 24), i * 48 + 24)), new Vector2(J, i));
                }

            }
        for(int i=0;i<5;i++)
        {
            for(int j=0;j<6;j++)
            {
                if (i == 3 && j == 2)
                    tileList[j, i].walkable = false;
                if (i == 3 && j == 3)
                    tileList[j, i].walkable = false;
                if (i == 3 && j == 1)
                    tileList[j, i].walkable = false;
            }
        }

        }

        public static Node getTile(Ray mouseRay)
        {
            for(int i=0;i<GridSize;i++)
            {
                for(int k=0;k<GridSize;k++)
                {
                    if(mouseRay.Intersects(tileList[i,k].Box)!=null)
                    {
                             return tileList[i,k];
                    }
                }
            }
            return new Node(Vector2.Zero, false, new BoundingBox(),new Vector2(0,0));

               
        }
        public static bool isWalkable(int x, int y)
        {
            return tileList[x, y].walkable;
        }
        public static bool isWalkable(Node curentNode)
        {
            return tileList[(int)curentNode.index.X, (int)curentNode.index.Y].walkable;
        }

        
    }
}
