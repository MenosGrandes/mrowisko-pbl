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
namespace Logic.PathFinderManager
{
    public class PathFinderManager
    {

        public List<Tile> way = new List<Tile>();
        public List<Tile> posibleWay = new List<Tile>();//mozemy na nie pójsc
        public List<Tile> weHaveBeenThere = new List<Tile>();//tile na ktorych juz bylismy;



        public Tile[,] tileList;
        public PathFinderManager(List<InteractiveModel> models)
        {
            tileList = new Tile[64, 64];



            for (int i = 0; i < 64; i += 1)
            {
                for (int J = 0; J < 64; J += 1)
                {

                    tileList[i, J] = new Tile(new Vector3(24 + J * 48, 35, 24 + i * 48), true, new BoundingBox(new Vector3(J * 48, 0, i * 48), new Vector3(J * 48 + 24, StaticHelpers.StaticHelper.GetHeightAt(J * 48 + 24, i * 48 + 24), i * 48 + 24)), new Vector2(J, i));
                }

            }

        }
        public override string ToString()
        {
            return base.ToString();
        }
        public Tile getTile(Ray mouseRay)
        {
            for(int i=0;i<64;i++)
            {
                for(int k=0;k<64;k++)
                {
                    if(mouseRay.Intersects(tileList[i,k].Box)!=null)
                    {
                             return tileList[i,k];
                    }
                }
            }
            return new Tile(Vector3.Zero, false, new BoundingBox(),new Vector2(0,0));

               
        }
        public void calculateHeuristic(Vector2 startPoint, Vector2 stopPoint, out float distance)
        {
            distance = Vector2.Distance(startPoint, stopPoint);
        }
        public void calculateHeuristic(Vector3 startPoint, Vector3 stopPoint, out float distance)
        {
            distance = Vector2.Distance(new Vector2(startPoint.X, startPoint.Z), new Vector2(stopPoint.X, stopPoint.Z));
        }
        public void checkNeighbours(Tile curentTile, Tile endTile)
        {
            Tile goToTile=new Tile(Vector3.Zero,false,new BoundingBox(),new Vector2(0,0));

            if (curentTile.index.Y - 1 >= 0 && tileList[(int)curentTile.index.X, (int)curentTile.index.Y - 1].walkable == true)
                posibleWay.Add(tileList[(int)curentTile.index.X - 0, (int)curentTile.index.Y - 1]);
            if (curentTile.index.X - 1 >= 0 && tileList[(int)curentTile.index.X - 1, (int)curentTile.index.Y].walkable == true)
                posibleWay.Add(tileList[(int)curentTile.index.X - 1, (int)curentTile.index.Y - 0]);
            if (curentTile.index.X + 1 < 63 && tileList[(int)curentTile.index.X + 1, (int)curentTile.index.Y].walkable == true)
                posibleWay.Add(tileList[(int)curentTile.index.X + 1, (int)curentTile.index.Y - 0]);
            if (curentTile.index.Y + 1 < 63 && tileList[(int)curentTile.index.X, (int)curentTile.index.Y + 1].walkable == true)
                posibleWay.Add(tileList[(int)curentTile.index.X + 0, (int)curentTile.index.Y + 1]);

            if (curentTile.index.Y + 1 < 63 && curentTile.index.X + 1 < 63 && tileList[(int)curentTile.index.X + 1, (int)curentTile.index.Y + 1].walkable == true)
                posibleWay.Add(tileList[(int)curentTile.index.X + 1, (int)curentTile.index.Y + 1]);

            if (curentTile.index.Y - 1 >= 0 && curentTile.index.X - 1 >= 0 && tileList[(int)curentTile.index.X - 1, (int)curentTile.index.Y - 1].walkable == true)
                posibleWay.Add(tileList[(int)curentTile.index.X - 1, (int)curentTile.index.Y - 1]);

            if (curentTile.index.Y + 1 >= 0 && curentTile.index.X + 1 >= 63 && tileList[(int)curentTile.index.X + 1, (int)curentTile.index.Y + 1].walkable == true)
                posibleWay.Add(tileList[(int)curentTile.index.X + 1, (int)curentTile.index.Y - 1]);

            if (curentTile.index.Y + 1 >= 63 && curentTile.index.X + 1 >= 0 && tileList[(int)curentTile.index.X + 1, (int)curentTile.index.Y + 1].walkable == true)
                posibleWay.Add(tileList[(int)curentTile.index.X - 1, (int)curentTile.index.Y + 1]);

            float[] distance = new float[8];


            for (int i = 0; i < posibleWay.Count; i++)
            {
                
                    calculateHeuristic(posibleWay[i].centerPosition, endTile.centerPosition, out distance[i]);
                    posibleWay[i].heuristicValue = distance[i];
                
            }

            foreach (Tile t in weHaveBeenThere)
            {

                for (int i = 0; i < posibleWay.Count; i++)
                {
                    if (posibleWay[i] != t)
                    {
                        posibleWay[i].heuristicValue = 99999;
                    }
                }
            }
            float min = 10000;
            foreach(Tile t in posibleWay)
            {
                  if(t.heuristicValue<min)
                  {
                      min = t.heuristicValue;
                      goToTile = t;
                  }
            }
            weHaveBeenThere.Add(goToTile);
            Console.WriteLine(goToTile);

        }
    }
}
