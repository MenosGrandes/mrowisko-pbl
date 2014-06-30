using Logic.Units.Predators;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Triggers
{
    public class CrabTrigger:TimeTrigger
    {
    public Cancer cancer;
    public CrabTrigger(int secondsToEngage)
        : base(secondsToEngage)
    {
        timeToEngage = TimeSpan.Zero;
        this.secondsToEngage = secondsToEngage;
        used = false;
    }
    public override void Update(GameTime time)
    {

        base.timeToEngage += time.ElapsedGameTime;
        if (timeToEngage > TimeSpan.FromSeconds((double)secondsToEngage))
        {
            used = true;
        }
    }

    }
}

