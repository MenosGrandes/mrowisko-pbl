using System;
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
///
/// dodałem 2 przestrzenie nazw, bo tak było w tutorialu, inaczej nie działa, 
///jedna z przestrzeni to jest biblioteka klas XNA, a druga Content Pipeline Extension
///nie może być w głównej przestrzeni gry bo albo występuje wzajemne includowanie przestrzeni,
///albo nie widać bibliotek
///
namespace Animations
{
   
    /// <summary>
    /// All animations, are included can be played by this class
    /// to play animation in main game class look on Method Update :)
    /// </summary>
    public class AnimationPlayer
    {
        SkinningData skinningData;

        //the currently playing clip, if there is one

        public AnimationClip CurrentClip { get; private set; }

        //whether the current animation has finished

        public bool Done { get; private set; }

        TimeSpan startTime, endTime, currentTime;

        bool loop;

        int currentKeyFrame;

        //transforms
        public Matrix[] BoneTransforms { get; private set; }
        public Matrix[] WorldTransforms { get; private set; }
        public Matrix[] SkinTransforms { get; private set; }
        public AnimationPlayer(SkinningData skinningData)
        {
            this.skinningData = skinningData;
            BoneTransforms = new Matrix[skinningData.BindPose.Count];
            WorldTransforms = new Matrix[skinningData.BindPose.Count];
            SkinTransforms = new Matrix[skinningData.BindPose.Count];
        }
        //starts playing the entirety of the given clip
        public void StartClip(string clip, bool loop)
        {
            AnimationClip clipVal = skinningData.AnimationClips[clip];

            StartClip(clip, TimeSpan.FromSeconds(0), clipVal.Duration, loop);
        }
        //plays a specyfic position of the given clip from one frame
        //index to another

        public void StartClip(string clip, int startFrame, int endFrame, bool loop)
        {
            AnimationClip clipVal = skinningData.AnimationClips[clip];
            StartClip(clip, clipVal.Keyframes[startFrame].Time,
                clipVal.Keyframes[endFrame].Time, loop);
        }
        public void StartClip(string clip, TimeSpan StartTime,
            TimeSpan EndTime, bool loop)
        {
            CurrentClip = skinningData.AnimationClips[clip];
            currentTime = TimeSpan.FromSeconds(0);
            currentKeyFrame = 0;
            Done = false;
            this.startTime = StartTime;
            this.endTime = EndTime;
            this.loop = loop;

            //copy the bind pose to bone transforms array to reset the animatio
            skinningData.BindPose.CopyTo(BoneTransforms, 0);
        }
        public void Update(TimeSpan time, Matrix rootTransform)
        {
            if (CurrentClip == null || Done) return;
            currentTime += time;
            updateBoneTransforms();
            updateWorldTransforms(rootTransform);
            updateSkinTransforms();
        }
        //helper used by the update method to refresh the BoneTransforms data
        void updateBoneTransforms()
        {
            while (currentTime >= (endTime - startTime))
            {
                //if we are looping, reduce the time until we are
                // back in the animation's time frame
                if (loop)
                {
                    currentTime -= (endTime - startTime);
                    currentKeyFrame = 0;
                    skinningData.BindPose.CopyTo(BoneTransforms, 0);
                }
                else
                {
                    Done = true;
                    currentTime = endTime;
                    break;
                }
            }
            IList<Keyframe> keyframes = CurrentClip.Keyframes;

            //read keyframes until we have found the lastes frame before
            //the current time
            while (currentKeyFrame < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyFrame];
                //stop when we've read up to the current time position
                if (keyframe.Time > currentTime + startTime) break;

                BoneTransforms[keyframe.Bone] = keyframe.Transform;
                currentKeyFrame++;
            }

        }
        // Helper used by the Update method to refresh the WorldTransforms data
        void updateWorldTransforms(Matrix rootTransform)
        {
            WorldTransforms[0] = BoneTransforms[0] * rootTransform;

            //for each child bone
            for (int bone = 1; bone < WorldTransforms.Length; bone++)
            {
                int parentBone = skinningData.SkeletonHierarchy[bone];
                WorldTransforms[bone] = BoneTransforms[bone] * WorldTransforms[parentBone];
            }
        }
        //Helper used by the Update method to refresh the SkinTransforms data
        void updateSkinTransforms()
        {
            for (int bone = 0; bone < SkinTransforms.Length; bone++)
            {
                SkinTransforms[bone] = skinningData.InverseBindPose[bone]
                    * WorldTransforms[bone];
            }
        }
    }
}
