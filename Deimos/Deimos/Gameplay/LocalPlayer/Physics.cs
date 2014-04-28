﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class Physics
    {
        public enum PhysicalState
        {
            Jumping,
            Falling,
            Walking
        }

        public enum AccelerationDirection
        {
            X,
            Y,
            Z
        }

        public enum AccelerationState
        {
            Still,
            On,
            Constant,
            Off,
            Maxed
        }

        public PhysicalState GravityState = PhysicalState.Falling;
        public float timer_gravity = 0f;
        public float c_gravity = 9.8f;
        public float initial_velocity;
        public float JumpVelocity;

        public AccelerationState Accelerestate = AccelerationState.Still;
        public Vector3 dv = new Vector3(0.09f, 0.09f, 0.09f);
        public Vector3 acceleration;
        public Vector3 Acceleration
        {
            get { return acceleration; }
            set
            {
                acceleration.X = Math.Max(-GetMaxHorizAcceleration(), Math.Min(value.X, GetMaxHorizAcceleration()));
                acceleration.Y = Math.Max(-GetMaxVertAcceleration(), Math.Min(value.Y, GetMaxVertAcceleration()));
                acceleration.Z = Math.Max(-GetMaxHorizAcceleration(), Math.Min(value.Z, GetMaxHorizAcceleration()));
            }
        }

        public Vector3 momentum;

        // Constructor
        public Physics()
        {
            //
        }

        // Methods
        public Vector3 GetAcceleration()
        {
            return Acceleration;
        }

        public Vector3 GetMomentum()
        {
            if (momentum == Vector3.Zero)
            {
                return (new Vector3(1, 1, 1));
            }
            else
            {
                return momentum;
            }
        }

        // MOVEMENT RELATED
        public void Accelerate(AccelerationDirection dir)
        {
            if (Accelerestate == AccelerationState.Still
                || Accelerestate == AccelerationState.Off)
            {
                Accelerestate = AccelerationState.On;
            }
            switch(dir)
            {
                case AccelerationDirection.X:
                    Acceleration += new Vector3(dv.X, 0, 0);
                    break;
                case AccelerationDirection.Y:
                    Acceleration += new Vector3(0, 1f, 0);
                    break;
                case AccelerationDirection.Z:
                    Acceleration += new Vector3(0, 0, dv.Z);
                    break;
            }
        }

        public void Decelerate(AccelerationDirection dir)
        {
            if (Accelerestate == AccelerationState.On
                || Accelerestate == AccelerationState.Constant
                || Accelerestate == AccelerationState.Maxed)
            {
                Accelerestate = AccelerationState.Off;
            }
            switch (dir)
            {
                case AccelerationDirection.X:
                    Acceleration -= new Vector3(dv.X, 0, 0);
                    break;
                case AccelerationDirection.Y:
                    Acceleration -= new Vector3(0, dv.Y, 0);
                    break;
                case AccelerationDirection.Z:
                    Acceleration -= new Vector3(0, 0, dv.Z);
                    break;
            }
        }

        public void Reset(AccelerationDirection dir)
        {
            switch(dir)
            {
                case AccelerationDirection.X:
                    if (Acceleration.X == 0)
                    {
                        momentum.X = 0;
                        return;
                    }
                    if (Acceleration.X > 0)
                    {
                        Decelerate(dir);
                        if (Acceleration.X < 0)
                        {
                            Acceleration = new Vector3(0, Acceleration.Y, Acceleration.Z);
                        }
                    }
                    else
                    {
                        Accelerate(dir);
                        if (Acceleration.X > 0)
                        {
                            Acceleration = new Vector3(0, Acceleration.Y, Acceleration.Z);
                        }
                    }
                    break;
                case AccelerationDirection.Y:
                    if (Acceleration.Y == 0) return;
                    if (Acceleration.Y > 0)
                    {
                        Decelerate(dir);
                        if (Acceleration.Y < 0)
                        {
                            Acceleration = new Vector3(Acceleration.X, 0, Acceleration.Z);
                        }
                    }
                    else
                    {
                        Accelerate(dir);
                        if (Acceleration.Y > 0)
                        {
                            Acceleration = new Vector3(Acceleration.X, 0, Acceleration.Z);
                        }
                    }
                    break;
                case AccelerationDirection.Z:
                    if (Acceleration.Z == 0)
                    {
                        momentum.Z = 0;
                        return;
                    }
                    if (Acceleration.Z > 0)
                    {
                        Decelerate(dir);
                        if (Acceleration.Z < 0)
                        {
                            Acceleration = new Vector3(Acceleration.X, Acceleration.Y, 0);
                        }
                    }
                    else
                    {
                        Accelerate(dir);
                        if (Acceleration.Z > 0)
                        {
                            Acceleration = new Vector3(Acceleration.X, Acceleration.Y, 0);
                        }
                    }
                    break;
            }
        }

        private float GetMaxHorizAcceleration()
        {
            float primal_speed;

            switch (GameplayFacade.ThisPlayer.CurrentSpeedState)
            {
                case Player.SpeedState.Running:
                    primal_speed = 1f;
                    break;

                case Player.SpeedState.Sprinting:
                    primal_speed = 1.5f;
                    break;

                case Player.SpeedState.Walking:
                    primal_speed = 0.5f;
                    break;

                case Player.SpeedState.Crouching:
                    primal_speed = 0.3f;
                    break;

                default:
                    primal_speed = 1f;
                    break;
            }

            return primal_speed;
        }

        // GRAVITY RELATED
        private float GetMaxVertAcceleration()
        {
            switch (GravityState)
            {
                case PhysicalState.Walking:
                    return 0f;

                case PhysicalState.Jumping:
                    return 1f;

                case PhysicalState.Falling:
                    return 1f;

                default:
                    return 0f;
            }
        }

        public void ApplyGravity(float dt)
        {
            float vy = (initial_velocity * timer_gravity) - ((float)Math.Pow(timer_gravity, 2) * c_gravity);
            acceleration.Y = vy;
            
            if (vy < 0f)
            { 
                GravityState = PhysicalState.Falling;
            }

            timer_gravity += dt;
        }

        public void StabilizeGravity()
        {
            if (GravityState == PhysicalState.Falling)
            {
                timer_gravity = 0;
                initial_velocity = 0;
                GravityState = PhysicalState.Walking;
                acceleration.Y = 0f;
                momentum.Y = 0;
            }

            if (GravityState == PhysicalState.Jumping)
            {
                initial_velocity = 0;
                timer_gravity = 0;
                GravityState = PhysicalState.Falling;
            }
        }

        public void Jump()
        {
            // Interrupting player sprint
            if (GameplayFacade.ThisPlayer.CurrentSpeedState == Player.SpeedState.Sprinting)
            {
                GameplayFacade.ThisPlayer.CurrentSpeedState = Player.SpeedState.Running;
            }

            // creating the potential momentum
            momentum = CreateMomentum();

            // Setting initial parameters to kick off the player
            initial_velocity = JumpVelocity;
            timer_gravity = 0f;
            GravityState = PhysicalState.Jumping;
        }

        // hard-coded momentum for display purposes
        //public bool ShouldResetMovement(AccelerationDirection direction)
        //{
        //    switch (direction)
        //    {
        //        case AccelerationDirection.Z:
        //            if (isForcedMovement)
        //            {
        //                return false;
        //            }
        //            return true;

        //        default:
        //            return true;
        //    }
        //}

        public Vector3 CreateMomentum()
        {
            Vector3 direction = -DisplayFacade.Camera.ViewVector;
            direction.Y = 1;

            return (direction);
        }
    }
}