using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
   public class UnitFormation
    {
        private List<Unit> unit = new List<Unit>();

        public List<Unit> Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        private Unit leader;
        public Unit Leader;
    }
}
