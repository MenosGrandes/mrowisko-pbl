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

        public PathFinder(Node startNode, Node endNode)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            openList.Add(startNode);
            closedList.Add(startNode);

        }
        public bool Search()
        {
            if(closedList[closedList.Count-1]!=endNode)
            {
                getNeighbours(closedList[closedList.Count - 1]);
                return false;
            }
            else
            {return true;}

        }
        public void getNeighbours(Node currentNode)
        {
            float minimumValue = float.MaxValue;
            Node minNode=new Node();

            //klocek w lewo
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
            if (currentNode.index.Y + 1 <= 63 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X, (int)currentNode.index.Y + 1].parent = currentNode;
            }
            //klocek do dolu
            if (currentNode.index.X + 1 <= 63 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y]))
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
            if (currentNode.index.X + 1 <= 63 && currentNode.index.Y - 1 >= 0 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1]);
                PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y - 1].parent = currentNode;
            }
            //prawy dolny
            if (currentNode.index.X + 1 <= 63 && currentNode.index.Y + 1 <= 63 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X + 1, (int)currentNode.index.Y + 1].parent = currentNode;
            }
            //lewy dolny róg
            if (currentNode.index.X - 1 >= 0 && currentNode.index.Y + 1 <= 63 && PathFinderManager.isWalkable(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1]))
            {
                openList.Add(PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1]);
                PathFinderManager.tileList[(int)currentNode.index.X - 1, (int)currentNode.index.Y + 1].parent = currentNode;
            }
            //openList.RemoveAll(x => x.index == currentNode.index);
            //tablice do sprawdzania odleglosci
            float[] f = new float[openList.Count];
            float[] ManhatanFromStart = new float[openList.Count];
            float[] ManhatanFromStop = new float[openList.Count];
            //sprawdzanie tego wszystkiego
            for (int i = 0; i < openList.Count; i++)
            {
                ManhatanFromStart[i] = Math.Abs(startNode.centerPosition.X - openList[i].centerPosition.X) + Math.Abs(startNode.centerPosition.Y - openList[i].centerPosition.Y);
                ManhatanFromStop[i] = Math.Abs(endNode.centerPosition.X - openList[i].centerPosition.X) + Math.Abs(endNode.centerPosition.Y - openList[i].centerPosition.Y);
                f[i] = ManhatanFromStop[i] + ManhatanFromStart[i];
                if (minimumValue > f[i] && !closedList.Contains(openList[i]) )
                {
                    minimumValue = f[i];
                    minNode = openList[i];
                }
            }
            
              
            closedList.Add(minNode);

        }


    }
}
