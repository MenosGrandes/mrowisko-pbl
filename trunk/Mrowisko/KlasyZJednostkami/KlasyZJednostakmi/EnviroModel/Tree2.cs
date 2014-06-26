using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.EnviroModel
{
    [Serializable]
    public class Tree2: EnviroModels
    {
        public Tree2(LoadModel model)
            : base(model)
        { selectable = false; }
        public Tree2()
            : base()
        { selectable = false; }
    }
}
