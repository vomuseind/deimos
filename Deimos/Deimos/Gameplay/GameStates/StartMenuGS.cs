﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class StartMenuGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.StartMenu; }
        }

        public override void PreSet()
        {
            //On veut afficher la souris
            GeneralFacade.Game.IsMouseVisible = true;
            float coeffX = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Width / (float)DisplayFacade.BackgroundMenu.Width;
            float coeffY = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Height / (float)DisplayFacade.BackgroundMenu.Height;
            int imageWidth = DisplayFacade.MenuImages["StartMenuPlay"].Width;

            GeneralFacade.SceneManager.SetScene<SceneStartMenu>();

            DisplayFacade.BlurredScene = true;

            DisplayFacade.ScreenElementManager.AddImage(
                "StartScreenMenuBackground", 
                0, 
                0,
                coeffX,
                coeffY, 
                0,
                DisplayFacade.BackgroundMenu,
                null, null, null
            );

            // On affiche les éléments du menu

            DisplayFacade.ScreenElementManager.AddImage(
                "StartScreenMenuPlay",
                (int)((530 - imageWidth) * coeffX),
                250,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["StartMenuPlay"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    if (!NetworkFacade.IsMultiplayer)
                    {
                        GameplayFacade.Objects = new ObjectsList();

                        GameplayFacade.Weapons = new WeaponsList();
                        GameplayFacade.Weapons.Initialise();
                        GameplayFacade.Objects.Initialize();

                        GeneralFacade.GameStateManager.Set(
                                    new LoadingLevelGS<SceneCompound>()
                                    );
                        GeneralFacade.GameStateManager.Set(new SpawningGS());
                        GeneralFacade.GameStateManager.Set(new PlayingGS());
                    }
                    else
                    {
                        NetworkFacade.NetworkHandling.ShakeHands();

                        System.Threading.Thread.Sleep(2000);

                        if (NetworkFacade.NetworkHandling.Handshook)
                        {
                            NetworkFacade.NetworkHandling.Connect();

                            System.Threading.Thread.Sleep(3000);

                            if (NetworkFacade.NetworkHandling.ServerConnected)
                            {
                                GameplayFacade.Objects = new ObjectsList();
                                GameplayFacade.Weapons = new WeaponsList();
                                GameplayFacade.Weapons.Initialise();
                                GameplayFacade.Objects.Initialize();

                                switch (NetworkFacade.MainHandling.Connections.CurrentMap)
                                {
                                    case "coolmap":
                                        GeneralFacade.GameStateManager.Set(
                                            new LoadingLevelGS<SceneCompound>()
                                            );
                                        break;
                                    default:
                                        GeneralFacade.GameStateManager.Set(
                                            new LoadingLevelGS<SceneDeimos>()
                                            );
                                        break;
                                }

                                GeneralFacade.GameStateManager.Set(new SpawningGS());
                                GeneralFacade.GameStateManager.Set(new PlayingGS());
                            }

                        }
                    }
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("StartScreenMenuPlay");
                    t.Image = DisplayFacade.MenuImages["StartMenuPlayHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("StartScreenMenuPlay");
                    t.Image = DisplayFacade.MenuImages["StartMenuPlay"];
                }
            );

            DisplayFacade.ScreenElementManager.AddImage(
                "StartScreenMenuServers",
                (int)((520 - imageWidth) * coeffX),
                320,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["StartMenuServers"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new ServerListMenuGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("StartScreenMenuServers");
                    t.Image = DisplayFacade.MenuImages["StartMenuServersHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("StartScreenMenuServers");
                    t.Image = DisplayFacade.MenuImages["StartMenuServers"];
                }
            );
            DisplayFacade.ScreenElementManager.AddImage(
                "StartScreenMenuQuit",
                (int)((510 - imageWidth) * coeffX),
                390,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["StartMenuQuit"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.Game.Exit();
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("StartScreenMenuQuit");
                    t.Image = DisplayFacade.MenuImages["StartMenuQuitHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("StartScreenMenuQuit");
                    t.Image = DisplayFacade.MenuImages["StartMenuQuit"];
                }
            );
        }

        public override void PostUnset()
        {
            DisplayFacade.BlurredScene = false;
            DisplayFacade.ScreenElementManager.RemoveImage("StartScreenMenuBackground");
            DisplayFacade.ScreenElementManager.RemoveImage("StartScreenMenuPlay");
            DisplayFacade.ScreenElementManager.RemoveImage("StartScreenMenuServers");
            DisplayFacade.ScreenElementManager.RemoveImage("StartScreenMenuQuit");
        }
    }
}
