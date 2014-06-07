using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.EnviroModel
{
    [Serializable]
    public class Tree:EnviroModels
    {
        public Tree(LoadModel model):base(model)
        { }
        public Tree():base()
        { }
    }
}
