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
using Logic.Meterials;
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

                    tileList[J, i] = new Node(new Vector2(12 + J * 24, 12 + i * 24), true, new BoundingBox(new Vector3(J * 24, 10, i * 24), new Vector3(J * 24+ 24, StaticHelpers.StaticHelper.GetHeightAt(J * 24 + 24, i * 24 + 24)+3, i * 24 + 24)), new Vector2(J, i));
                }

            }




                

            }


        public static void blockAllNodes(List<InteractiveModel> obstacles)
        {
            for (int i = 0; i < GridSize; i += 1)
            {
                for (int J = 0; J < GridSize; J += 1)
                {
                    foreach (InteractiveModel m in obstacles)
                    {
                        if (m.Model.Spheres.Count > 0)
                        {
                            
                            if (tileList[i, J].Box.Intersects(m.Model.Spheres[0]))
                            {
                                if (m.GetType().BaseType == typeof(Material))
                                {
                                    tileList[i, J].haveMineral = true;
                                    ((Material)m).nodes.Add(tileList[i, J]);
                                }
                                else if (m.GetType().IsSubclassOf(typeof(Building.Building)))
                                {
                                    tileList[i, J].haveBuilding = true;
                                }
                                tileList[i, J].walkable = false;
                            }
                        }
                        else
                        {
                            if (tileList[i, J].Box.Intersects(m.Model.BoundingSphere))
                            {
                                tileList[i, J].walkable = false;
                            }
                        }
                        
                    }
                    foreach(QuadNode n in QuadNodeController.QuadNodeList2)
                    {
                        if (tileList[i, J].Box.Intersects(n.Bounds))
                        {
                            if ( n.Bounds.Max.Y >70)
                            {
                            tileList[i, J].walkable = false;
                                }
                        }
                    }
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
