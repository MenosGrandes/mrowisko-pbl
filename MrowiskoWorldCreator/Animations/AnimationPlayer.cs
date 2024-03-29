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

    /// <summary>
    /// The animation player is in charge of decoding bone position
    /// matrices from an animation clip.
    /// </summary>
    public class AnimationPlayer
    {
        //Information about the currently playing animation clip
        AnimationClip currentClipValue;
        TimeSpan currentTimeValue;
        int currentKeyframe;
        public Boolean end = false;//true if animation end
        public Boolean animationFlag = false;
        // Current animation transform matrices.
        Matrix[] boneTransforms;
        Matrix[] worldTransforms;
        Matrix[] skinTransforms;
        public int howManyTimesPlayed = 0;
        public int timesToPlay = -1;
        public bool looped = true;
        // Backlink to the bind pose and skeleton hierarchy data.
        SkinningData skinningDataValue;

        // The delegate template for the event callbacks
        public delegate void EventCallback(string Event);

        // The reigstered events
        Dictionary<string, Dictionary<string, EventCallback>> registeredEvents = new Dictionary<string, Dictionary<string, EventCallback>>();
        public Dictionary<string, Dictionary<string, EventCallback>> RegisteredEvents
        {
            get { return registeredEvents; }
        }
        /// <summary>
        /// Constructs a new animation player.
        /// </summary>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
            // Construct the event dictionaries for each clip
            foreach (string clipName in skinningData.AnimationClips.Keys)
            {
                registeredEvents[clipName] = new Dictionary<string, EventCallback>();
            }
        }

        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        public void StartClip(AnimationClip clip)
        {
            animationFlag = false;
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }

        /// <summary>
        /// Advances the current animation position.
        /// </summary>
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            if (looped || timesToPlay == -1) UpdateBoneTransforms(time, relativeToCurrentTime);
            if (!looped && timesToPlay != -1 && howManyTimesPlayed <= timesToPlay) UpdateBoneTransforms(time, relativeToCurrentTime);
            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
            if (CurrentTime.TotalMilliseconds + 30 > CurrentClip.Duration.TotalMilliseconds) end = true;
            else end = false;
            if (end)
            {
                animationFlag = true;
                howManyTimesPlayed++;
            }
            // Console.Out.WriteLine(howManyTimesPlayed);
        }

        /// <summary>
        /// Helper used by the Update method to refresh the BoneTransforms data.
        /// </summary>
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (currentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            // Store the previous time
            TimeSpan lastTime = time;

            // Update the animation position.
            if (relativeToCurrentTime)
            {
                lastTime = currentTimeValue;
                time += currentTimeValue;

                // Check for events
                CheckEvents(ref time, ref lastTime);

                // If we reached the end, loop back to the start.
                bool hasLooped = false;
                while (time >= currentClipValue.Duration)
                {
                    hasLooped = true;
                    time -= currentClipValue.Duration;
                }

                // If we've looped, reprocess the events
                if (hasLooped)
                {
                    CheckEvents(ref time, ref lastTime);
                }
            }

            if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            // If the position moved backwards, reset the keyframe index.
            bool HasResetKeyframe = false;
            if (time < currentTimeValue)
            {
                HasResetKeyframe = true;
                currentKeyframe = 0;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            currentTimeValue = time;

            // Read keyframe matrices.
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // Stop when we've read up to the current time position.
                if ((keyframe.Time > currentTimeValue) && (!HasResetKeyframe))
                    break;

                // Use this keyframe.
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;

                if (HasResetKeyframe)
                {
                    currentTimeValue = keyframe.Time;
                    HasResetKeyframe = false;
                }
            }
        }

        /// <summary>
        /// Helper used by the Update method to refresh the WorldTransforms data.
        /// </summary>
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }

        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }


        /// <summary>
        /// Gets the current bone transform matrices, relative to their parent bones.
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices, in absolute format.
        /// </summary>
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices,
        /// relative to the skinning bind pose.
        /// </summary>
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


        /// <summary>
        /// Gets the clip currently being decoded.
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }


        /// <summary>
        /// Gets the current play position.
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }

        /// Checks to see if any events have passed
        /// </summary>
        private void CheckEvents(ref TimeSpan time, ref TimeSpan lastTime)
        {
            foreach (string eventName in registeredEvents[currentClipValue.Name].Keys)
            {
                // Find the event
                foreach (AnimationEvent animEvent in currentClipValue.Events)
                {
                    if (animEvent.EventName == eventName)
                    {
                        TimeSpan eventTime = animEvent.EventTime;
                        if ((lastTime < eventTime) && (time >= eventTime))
                        {
                            // Call the event
                            registeredEvents[currentClipValue.Name][eventName](eventName);
                        }
                    }
                }
            }
        }
    }
}
