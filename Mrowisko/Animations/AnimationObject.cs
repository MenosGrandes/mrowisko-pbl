﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Animations
{
    public class AnimationObject
    {
        Vector3 startPosition, endPosition, startRotation, endRotation;
        TimeSpan duration;
        bool loop;
        TimeSpan elapsedTime = TimeSpan.FromSeconds(0);
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public AnimationObject(Vector3 StartPosition, Vector3 EndPosition,
        Vector3 StartRotation, Vector3 EndRotation, TimeSpan Duration,
          bool Loop)
        {
            this.startPosition = StartPosition;
            this.endPosition = EndPosition;
            this.startRotation = StartRotation;
            this.endRotation = EndRotation;
            this.duration = Duration;
            this.loop = Loop;
            Position = startPosition;
            Rotation = startRotation;
        }
        public void Update(TimeSpan Elapsed)
        {
            // Update the time
            this.elapsedTime += Elapsed;

            // Determine how far along the duration value we are (0 to 1)
            float amt = (float)elapsedTime.TotalSeconds / (float)duration.
          TotalSeconds;

            if (loop)
                while (amt > 1) // Wrap the time if we are looping
                    amt -= 1;
            else // Clamp to the end value if we are not
                amt = MathHelper.Clamp(amt, 0, 1);

            // Update the current position and rotation
            Position = Vector3.Lerp(startPosition, endPosition, amt);
            Rotation = Vector3.Lerp(startRotation, endRotation, amt);
        }
    }
}
