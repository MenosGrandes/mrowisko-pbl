﻿using GameCamera;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials
{
    [Serializable]
    public class Material : InteractiveModel
    {
        public List<Node> nodes = new List<Node>();
        public int ClusterSize;
        public int MaxClusterSize;

        public Material(LoadModel model, int ClusterSize)
            : base(model)
        {
            this.MaxClusterSize = ClusterSize;
            this.ClusterSize = ClusterSize;
        }
        public Material()
        { }
        public Material(LoadModel model)
            : base(model)
        { }
        public override void Draw(FreeCamera camera)
        {
            model.Draw(camera);
        }

    }
}
 