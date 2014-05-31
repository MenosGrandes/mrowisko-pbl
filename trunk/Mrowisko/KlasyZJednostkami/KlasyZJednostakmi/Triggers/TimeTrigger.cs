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
       TimeSpan timeToEngage;
       int secondsToEngage;
       public bool used;
       public TimeTrigger(int secondsToEngage)
       {
            timeToEngage = TimeSpan.Zero;
            this.secondsToEngage = secondsToEngage;
            used = false;
       }
       public  void Update(GameTime time)
       {
           timeToEngage += time.ElapsedGameTime;
           if(timeToEngage>TimeSpan.FromSeconds((double)secondsToEngage))
           {
               Console.WriteLine("KABOOOMMMM");
               used = true;
           }
       }
    }
}
