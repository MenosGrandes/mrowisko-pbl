using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials.MaterialCluster
{
    [Serializable]
    public class Rock : Material
    {

        private List<Stone> stone = new List<Stone>();

       new public int ClusterSize
        {
            get
            {
                Console.WriteLine(stone.Count);
                return stone.Count;
            }
            set
            {
                value = ClusterSize;
            }

        }
        private int clusterSize;

        public Rock(LoadModel model, int clusterSize)
            : base(model, clusterSize)
        {

            for (int i = 0; i < clusterSize; i++)
            {
                stone.Add(new Stone());
            }

        }
        
        public void removeRock(int n)
        {
            if (stone.Count != 0) { stone.RemoveRange(0, 1); }

        }
        public override string ToString()
        {
            return this.GetType().Name + " " + model.Position + " " + ClusterSize;
        }

    }
}
