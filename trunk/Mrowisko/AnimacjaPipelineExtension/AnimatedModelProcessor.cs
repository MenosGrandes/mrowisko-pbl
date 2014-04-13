using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Animations;

///
///przestrzeń nazw z projektu Content Pipeline Extension
///musi być tak bo inaczej nie widać includów związanych z Pipeline
///jakbyście dodawali kolejne przestrzenie nazw to w solution explorer trzeba je dodać 
///w referncjach bo inaczej nie działa
namespace AnimacjaPipelineExtension
{
[ContentProcessor(DisplayName = "Animation Processor")]
    class AnimatedModelProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            //find the skeleton
            BoneContent skeleton = MeshHelper.FindSkeleton(input);
            //read the bind pose and skeleton hierarchy data
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            List<Matrix> bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();

            //extract the bind pose transform, inverse bind pose transform,
            //and parent bone index of each bone in order

            foreach(BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            //conver animation data to our runtime format
            Dictionary<string, AnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones);
            ModelContent model = base.Process(input, context);
            model.Tag = new SkinningData(animationClips, bindPose, inverseBindPose, skeletonHierarchy);
            return model;
        }
        static Dictionary<string,AnimationClip> ProcessAnimations
            (AnimationContentDictionary animations, IList<BoneContent> bones)
        {
            // Build up a table mapping bone names to indices.
            Dictionary<string, int> boneMap = new Dictionary<string, int>();
            for (int i = 0; i < bones.Count; i++)
                boneMap.Add(bones[i].Name, i);
            Dictionary<string, AnimationClip> animationClips =
            new Dictionary<string, AnimationClip>();
            // Convert each animation
            foreach (KeyValuePair<string, AnimationContent> animation in animations)
            {
                AnimationClip processed = ProcessAnimation(animation.Value, boneMap);
                animationClips.Add(animation.Key, processed);
            }
            return animationClips;
        }
       static AnimationClip ProcessAnimation(AnimationContent animation, Dictionary<string,int> boneMap)
        {
            List<Keyframe> keyframes = new List<Keyframe>();
           foreach (KeyValuePair<string,AnimationChannel> chanel in animation.Channels)
           {
               int boneIndex = boneMap[chanel.Key];
               //conver the keyframe data
               foreach (AnimationKeyframe keyframe in chanel.Value)
                   keyframes.Add(new Keyframe(boneIndex, keyframe.Time, keyframe.Transform));
           }
           keyframes.Sort(CompareaKeyFrameTimes);
           return new AnimationClip(animation.Duration, keyframes);
        }
        static int CompareaKeyFrameTimes( Keyframe a, Keyframe b)
       {
           return a.Time.CompareTo(b.Time);
       }
    }
}