﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tranquillity;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class SmokeParticleSystem : DynamicParticleSystem
    {
        public SmokeParticleSystem(int maxCapacity, Texture2D texture)
            : base(maxCapacity, texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (DynamicParticle particle in liveParticles)
            {
                particle.Color = Color.Lerp(particle.InitialColor, new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f - particle.Age.Value);
                particle.Scale += 0.000005f;
            }

            base.Update(gameTime);
        }
    }
}
