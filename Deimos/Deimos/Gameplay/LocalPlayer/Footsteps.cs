﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class Footsteps
    {
        Random rng = new Random();
        int current = 0;
        float timer = 0;
        float time = 0;

        Vector3 previous = default(Vector3);

        public void HandleFootsteps(float dt)
        {
            if (GameplayFacade.ThisPlayer.Position !=
                previous
                && GameplayFacade.ThisPlayerPhysics.GravityState ==
                Physics.PhysicalState.Walking)
            {
                float factor = GameplayFacade.ThisPlayer.Speed / 8;
                timer = 1 / factor;

                if (GameplayFacade.ThisPlayer.CurrentSpeedState ==
                    Player.SpeedState.Sprinting)
                {
                    timer /= 1.25f;
                }
                else if (GameplayFacade.ThisPlayer.CurrentSpeedState ==
                    Player.SpeedState.Walking ||
                    GameplayFacade.ThisPlayer.CurrentSpeedState ==
                    Player.SpeedState.Crouching)
                {
                    timer *= 1.8f;
                }

                if (time >= timer)
                {
                    time = 0;

                    current = rng.Next(0, 4);

                    switch (current)
                    {
                        case 0:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s1");
                            return;
                        case 1:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s2");
                            return;
                        case 2:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s3");
                            return;
                        case 3:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s4");
                            return;
                        default:
                            GeneralFacade.SceneManager.SoundManager.Play(
                                "s1");
                            return;
                    }
                }
                else
                {
                    time += dt;
                }

                previous = GameplayFacade.ThisPlayer.Position;
            }
        }
    }
}
