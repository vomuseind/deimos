﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class Bullet
    {
        // The projectile is always spawned by the weapon,
        // and destroys itself after collision with boundingbox,
        // or dissipates when it is sure of not hitting anything after completion of trajectory
        // -> moving object, with predictive calculations.

        // Damage calculations may also be made inside the Bullet class,
        // provided that values are restricted by Weapon, or Player.Health.

        DeimosGame Game;

        public Vector3 Direction;
        public Vector3 Position;
        public float speed;
        public float range;
        public float minimumDmg;
        public float maximumDmg;
        public float lifeSpan = 5;

        // Constructor
        public Bullet(DeimosGame game, Vector3 pos, Vector3 dir)
        {
            Game = game;

            // Setting initial bullet spawn location
            Position = pos;

            // Setting bullet direction according to current player's camera
            Direction = dir;

            // Setting bullet properties according to current player's current weapon
            speed = Game.ThisPlayer.CurrentWeapon.ProjectileSpeed;
            range = Game.ThisPlayer.CurrentWeapon.Range;
            minimumDmg = Game.ThisPlayer.CurrentWeapon.minDmg;
            maximumDmg = Game.ThisPlayer.CurrentWeapon.maxDmg;
        }

        // Destructor
        ~Bullet()
        {

        }
    }
}