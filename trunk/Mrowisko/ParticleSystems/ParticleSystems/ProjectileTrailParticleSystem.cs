using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Particles.ParticleSystems
{
    public class ProjectileTrailParticleSystem : ParticleSystem
    {
        public ProjectileTrailParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures/Particles/smoke";

            settings.MaxParticles = 5000;

            settings.Duration = TimeSpan.FromSeconds(1);

            settings.DurationRandomness = 1.5f;

            settings.EmitterVelocitySensitivity = 0.1f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            settings.MinColor = new Color(64, 96, 128, 255);
            settings.MaxColor = new Color(255, 255, 255, 128);

            settings.MinRotateSpeed = -4;
            settings.MaxRotateSpeed = 4;

            settings.MinStartSize = 1;
            settings.MaxStartSize = 3;

            settings.MinEndSize = 5;
            settings.MaxEndSize = 13;
        }
    }
}
