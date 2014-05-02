﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tranquillity;

namespace Deimos
{
    /// <summary>
    /// A simple sample affector that affects the particle velocity
    /// </summary>
    public class VelocityAffector : IParticleAffector
    {
        public Vector3 VelocityChange { get; set; }

        public VelocityAffector(Vector3 velocityChange)
        {
            VelocityChange = velocityChange;
        }

        public void Affect(GameTime gameTime, DynamicParticle particle)
        {
            if (particle.Velocity.HasValue)
            {
                particle.Velocity += (float)gameTime.ElapsedGameTime.TotalSeconds * VelocityChange;
            }
        }
    }
}
