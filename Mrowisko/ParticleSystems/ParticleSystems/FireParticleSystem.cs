#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Particles.ParticleSystems
{

    /// <summary>
    /// Custom particle system for creating a flame effect.
    /// </summary>
    public class FireParticleSystem : ParticleSystem
    {
        public FireParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures/Particles/fire";

            settings.MaxParticles = 70;

            settings.Duration = TimeSpan.FromSeconds(3);

            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 15;

            settings.MinVerticalVelocity = -10;
            settings.MaxVerticalVelocity = 10;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, 15, 0);

            settings.MinColor = new Color(255, 255, 255, 64);
            settings.MaxColor = new Color(255, 255, 255, 128);

            settings.MinStartSize = 40;
            settings.MaxStartSize = 80;

            settings.MinEndSize = 90;
            settings.MaxEndSize = 140;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}


