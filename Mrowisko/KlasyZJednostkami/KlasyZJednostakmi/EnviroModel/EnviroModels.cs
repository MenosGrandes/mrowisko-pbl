﻿using GameCamera;
using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.EnviroModel
{
    [Serializable]
    public class EnviroModels:InteractiveModel
    {
        public EnviroModels(LoadModel model):base(model)
        {
        }
        public EnviroModels():base()
        { }
        public override void Draw(FreeCamera camera)
        {
            model.Draw(camera);
        }
    }
}
