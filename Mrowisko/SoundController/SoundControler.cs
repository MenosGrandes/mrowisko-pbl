using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SoundController
{

        public static class SoundController
        {
            
            public static List<SoundEffect> sounds = new List<SoundEffect>();
            public static List<SoundEffectInstance> s_instance = new List<SoundEffectInstance>();
            public static List<Song> BackgroundSongs = new List<Song>();
            public static ContentManager content;
            public static int playqueue = 0;
            public static void Initialize(List<String> soundString)
            {
                foreach (String soun in soundString)
                {
                    sounds.Add(content.Load<SoundEffect>("Sounds/" + soun));
                }
                foreach (SoundEffect se in sounds)
                {
                    s_instance.Add(se.CreateInstance());
                }

               


            }

            public static void InitializeBackground(List<String> soundString)
            {
                foreach (String soun in soundString)
                {
                    BackgroundSongs.Add(content.Load<Song>("Songs/" + soun));
                    
                }
            }
            public static void Play(SoundEnum se)
            {
                switch (se)
                {

                    case SoundEnum.RangeHit:
                        if(s_instance[1].State==SoundState.Stopped)
                        s_instance[1].Play();
                        break;
                    case SoundEnum.SelectedMaterial:
                         if(s_instance[0].State==SoundState.Stopped)
                        s_instance[0].Play();
                        break;
                    case SoundEnum.Gater:
                        if (s_instance[2].State == SoundState.Stopped)
                            s_instance[2].Play();
                        break;
                    default:
                        break;

                }
            }
        }
        public enum SoundEnum
        {
            Hit,
            RangeHit,
            SelectedBuilding,
            SelecetedQueen,
            SelectedPeasant,
            SelectedBuildingPlace,
            SelectedMaterial ,
            Gater

        };
    }


