using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animations
{
    public class AnimationEvent
    {
        /// <summary>
        /// The name of the event
        /// </summary>
        public String EventName
        {
            get;
            set;
        }

        /// <summary>
        /// The time of the event
        /// </summary>
        public TimeSpan EventTime
        {
            get;
            set;
        }
    }
}
