using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.EnviroModel
{
    [Serializable]
    public class Grass : EnviroModels
    {
        public Grass(LoadModel model)
            : base(model)
        { }
        public Grass()
            : base()
        { }
    }
}
