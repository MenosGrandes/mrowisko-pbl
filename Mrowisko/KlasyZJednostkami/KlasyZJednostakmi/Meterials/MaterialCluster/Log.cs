﻿using GameCamera;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials.MaterialCluster
{
    [Serializable]
    public class Log : Material
    {

        private List<Wood> wood = new List<Wood>();

         new public int ClusterSize
        {
            get
            {
                //Console.WriteLine(wood.Count);
                return wood.Count;
            }
            set
            {
                value = ClusterSize;
            }


        }

        public Log(LoadModel model, int ClusterSize)
            : base(model, ClusterSize)
        {
            this.ClusterSize = ClusterSize;
            for (int i = 0; i < ClusterSize; i++)
            {
                wood.Add(new Wood());
            }

           // this.Model.BoundingSphere = new BoundingSphere(this.Model.BoundingSphere.Center, this.Model.BoundingSphere.Radius/10);
            
        }

        public void removeWood(int n)
        {
            if (wood.Count != 0) { wood.RemoveRange(0, 1); }
            if (wood.Count < 300) { wood.RemoveRange(0, wood.Count); }
           
           
           // Console.WriteLine(wood.Count);
        }

        public override string ToString()
        {
            return this.GetType().Name + " \n Capacity:" + ClusterSize;
        }
    }

}
