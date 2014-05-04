﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class SpawningGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.Spawning; }
        }

        public override void PreSet()
        {
            GameplayFacade.BulletManager = new BulletManager();

            GameplayFacade.ThisPlayer = new LocalPlayer();
            GameplayFacade.ThisPlayerPhysics = new Physics();
            GameplayFacade.ThisPlayerDisplay = new Display();


            GameplayFacade.ThisPlayer.Inventory = new WeaponManager();
            GameplayFacade.ThisPlayer.InitializeInventory(GameplayFacade.ThisPlayer.Class);
            GameplayFacade.ThisPlayer.PlayerSpawn(new Vector3(-60f, 20f, -8f), Vector3.Zero);

            if (NetworkFacade.IsMultiplayer)
            {
                NetworkFacade.World.Start();
                NetworkFacade.MovePacket.Start();
            }
        }

        public override void PostUnset()
        {
            //
        }
    }
}
