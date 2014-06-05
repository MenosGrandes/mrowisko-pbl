using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Triggers
{
    public class LaserTrigger:TimeTrigger
    {

        private Laser Laser;

        public Laser laser
        {
            get { return Laser; }
            set { Laser = value; }
        }
        public LaserTrigger(Laser laser, int secondsToEngage): base(secondsToEngage)
        {
            this.Laser = laser;
        }
        public override void Update(GameTime time)
        {
           
            base.timeToEngage += time.ElapsedGameTime;
            if (timeToEngage > TimeSpan.FromSeconds((double)secondsToEngage))
            {
                Console.WriteLine("START");
                Laser.Start();
                used = true;
            }
        }
    }
}
