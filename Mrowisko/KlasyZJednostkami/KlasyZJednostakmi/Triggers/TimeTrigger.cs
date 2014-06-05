using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Triggers
{
   public class TimeTrigger
   {
       protected TimeSpan timeToEngage;
       protected int secondsToEngage;
       public bool used;
       public TimeTrigger(int secondsToEngage)
       {
            timeToEngage = TimeSpan.Zero;
            this.secondsToEngage = secondsToEngage;
            used = false;
       }

        public virtual void Update(GameTime time)
        {}
    }
}
