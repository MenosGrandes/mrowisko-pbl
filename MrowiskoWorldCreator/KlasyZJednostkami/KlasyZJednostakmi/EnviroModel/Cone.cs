using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.EnviroModel
{
    [Serializable]
    public class Cone: EnviroModels
    {
        public Cone(LoadModel model)
            : base(model)
        { }
        public Cone()
            : base()
        { }
    }
}

