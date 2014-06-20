using Logic.PathFinderManagerNamespace;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.PathFinderNamespace
{
    public class PathFinder
    {
        public Node startNode;
        public Node endNode;
        public float movementCost;
        public List<Node> finalPath = new List<Node>();
        public List<Node> openList = new List<Node>();//mozemy na nie pójsc
        public List<Node> closedList = new List<Node>();//tile na ktorych juz bylismy;
        public PathFinder()
        {   
           
        }
        public bool Search(Node startNode,Node endNode)
        {
            Node currentNode=new Node();
            if(startNode==endNode)
            {
                finalPath.Add(startNode);
                return true;
            }
            this.startNode = startNode;
            this.endNode = endNode;
            openList.Add(startNode);
            getStartNodeNeighbours(startNode);
            openList.Remove(startNode);
            closedList.Add(startNode);
            currentNode = getMinFromList();

            while (currentNode != endNode)
            {

                openList.Remove(currentNode);
                closedList.Add(currentNode);
                getNeighbours(currentNode);
                currentNode = getMinFromList();
                if(openList.Count==0)
                {
                    return false;
                }
            }
            Node parent = new Node();
            finalPath.Add(endNode);
            finalPath.Add(closedList[closedList.Count - 1]);

            while (parent != startNode )
            {
                
                    parent = finalPath[finalPath.Count - 1].parent;
                    finalPath.Add(parent);
                
            }
            finalPath.Reverse();
            closedList.Clear();
            openList.Clear();
            return true;
        }
        public void getStartNodeNeighbours(Node currentNode)
        {


            if (currentNode.index.X - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y]);
                PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y].parent = currentNode;
            }
            //klocek do gory
            if (currentNode.index.Y - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y - 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y - 1]);
                PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y - 1].parent = currentNode;
            }
            //klocek w prawo
            if (currentNode.index.Y + 1 <= PathFinderManager.GridSize-1 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1].parent = currentNode;
            }
            //klocek do dolu
            if (currentNode.index.X + 1 <= PathFinderManager.GridSize-1 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y]);
                PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y].parent = currentNode;
            }
            //lewy górny róg
            if (currentNode.index.X - 1 >= 0 && currentNode.index.Y - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y - 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y - 1]);
                PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y - 1].parent = currentNode;
            }
            //prawy górny
            if (currentNode.index.X + 1 <= PathFinderManager.GridSize-1 && currentNode.index.Y - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1]);
                PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1].parent = currentNode;
            }
            //prawy dolny
            if (currentNode.index.X + 1 <= PathFinderManager.GridSize-1 && currentNode.index.Y + 1 <= PathFinderManager.GridSize-1 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1].parent = currentNode;
            }
            //lewy dolny róg
            if (currentNode.index.X - 1 >= 0 && currentNode.index.Y + 1 <= PathFinderManager.GridSize-1 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1].parent = currentNode;
            }
           
            CalculateManhattan();
        }
        public void getNeighbours(Node currentNode)
        {

            if (currentNode.index.X - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y])&&!closedList.Contains(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y]))
            {
                if (!openList.Contains(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y]))
                {
                    openList.Add(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y]);
                    PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y].parent = currentNode;
                }
                else
                {
                    var item = openList.Find(x => x.index == new Vector2(currentNode.index.X - 1, (int)currentNode.index.Y));
                    if(item.ManhatanFromStart<currentNode.ManhatanFromStart)
                    {
                        item.parent = currentNode;

                    }   
                }
               
            }
            //klocek do gory
            if (currentNode.index.Y - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y - 1])&&!closedList.Contains(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y - 1]))
            {
                if (!openList.Contains(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y - 1]))
                { 
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y - 1]);
                PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y - 1].parent = currentNode;
                }
                else
                {
                    var item = openList.Find(x => x.index == new Vector2(currentNode.index.X , (int)currentNode.index.Y-1));
                    if (item.ManhatanFromStart < currentNode.ManhatanFromStart)
                    {
                        item.parent = currentNode;
                    }
                }
            }
            //klocek w prawo
            if (currentNode.index.Y + 1 <= PathFinderManager.GridSize-1 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1])&&!closedList.Contains(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1]))
            {
                if (!openList.Contains(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1]))
                { 
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1].parent = currentNode;
                }
                else
                {
                    var item = openList.Find(x => x.index == new Vector2(currentNode.index.X , (int)currentNode.index.Y+1));
                    if (item.ManhatanFromStart < currentNode.ManhatanFromStart)
                    {
                        item.parent = currentNode;
                    }
                }
            }
            //klocek do dolu
            if (currentNode.index.X + 1 <= PathFinderManager.GridSize-1 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y])&&!closedList.Contains(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y]))
            {
                if (!openList.Contains(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y]))
                {
                    openList.Add(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y]);
                    PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y].parent = currentNode;
                }
                else
                {
                    var item = openList.Find(x => x.index == new Vector2(currentNode.index.X + 1, (int)currentNode.index.Y));
                    if (item.ManhatanFromStart < currentNode.ManhatanFromStart)
                    {
                        item.parent = currentNode;
                    }
                }
            }
            //lewy górny róg
            if (currentNode.index.X - 1 >= 0 && currentNode.index.Y - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y - 1])&&!closedList.Contains(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y - 1]))
            {
                if (!openList.Contains(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y - 1])) { 
                    openList.Add(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y - 1]);
                    PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y - 1].parent = currentNode;
                }
                else
                {
                    var item = openList.Find(x => x.index == new Vector2(currentNode.index.X - 1, (int)currentNode.index.Y-1));
                    if (item.ManhatanFromStart < currentNode.ManhatanFromStart)
                    {
                        item.parent = currentNode;
                    }
                }
            }
            //prawy górny
            if (currentNode.index.X + 1 <= PathFinderManager.GridSize-1 && currentNode.index.Y - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1])&&!closedList.Contains(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1]))
            {
               if( !openList.Contains(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1]))
               {
                   openList.Add(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1]);
                PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1].parent = currentNode;
               }
               else
               {
                   var item = openList.Find(x => x.index == new Vector2(currentNode.index.X + 1, (int)currentNode.index.Y - 1));
                   if (item.ManhatanFromStart < currentNode.ManhatanFromStart)
                   {
                       item.parent = currentNode;
                   }
               }
            }
            //prawy dolny
            if (currentNode.index.X + 1 <= PathFinderManager.GridSize-1 && currentNode.index.Y + 1 <= PathFinderManager.GridSize-1 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1])&&!closedList.Contains(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1]))
            {
                if(!openList.Contains(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1]))
                {openList.Add(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1].parent = currentNode;
                }
                else
                {
                    var item = openList.Find(x => x.index == new Vector2(currentNode.index.X + 1, (int)currentNode.index.Y+1));
                    if (item.ManhatanFromStart < currentNode.ManhatanFromStart)
                    {
                        item.parent = currentNode;
                    }
                }
            }
            //lewy dolny róg
            if (currentNode.index.X - 1 >= 0 && currentNode.index.Y + 1 <= PathFinderManager.GridSize-1 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1])&&!closedList.Contains(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1]))
            {
                if( !openList.Contains(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1]))
                { openList.Add(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1].parent = currentNode;
                }
                else
                {
                    var item = openList.Find(x => x.index == new Vector2(currentNode.index.X - 1, (int)currentNode.index.Y+1));
                    if (item.ManhatanFromStart < currentNode.ManhatanFromStart)
                    {
                        item.parent = currentNode.parent;
                    }
                }
            }

            CalculateManhattan();
        }

        public void CalculateManhattan()
        {
            for (int i = 0; i < openList.Count; i++)
            {
                openList[i].ManhatanFromStart =(float)Math.Sqrt((float)Math.Pow(openList[i].index.X - startNode.index.X, 2) + (float)Math.Pow(openList[i].index.Y - startNode.index.Y,2));
                openList[i].ManhatanFromStop = (float)Math.Sqrt((float)Math.Pow(openList[i].index.X - endNode.index.X, 2) + (float)Math.Pow(openList[i].index.Y - endNode.index.Y, 2));
                openList[i].CalcF();
            }
        }
        public void CalculateManhattan(Node CurentNode)
        {

            CurentNode.ManhatanFromStart = (float)Math.Sqrt((float)Math.Pow(CurentNode.index.X - startNode.index.X, 2) + (float)Math.Pow(CurentNode.index.Y - startNode.index.Y, 2));
            CurentNode.ManhatanFromStop = (float)Math.Sqrt((float)Math.Pow(CurentNode.index.X - endNode.index.X, 2) + (float)Math.Pow(CurentNode.index.Y - endNode.index.Y, 2));
            CurentNode.CalcF();
             
        }
        public Node getMinFromList()
        {
            Node minNode = new Node();
            float min = float.MaxValue;
            for (int i = 0; i < openList.Count; i++)
            {
                   if(min>openList[i].f)
                   {
                       min = openList[i].f;
                       minNode = openList[i];

                   }
            }
            return minNode;
        }

    }
}
