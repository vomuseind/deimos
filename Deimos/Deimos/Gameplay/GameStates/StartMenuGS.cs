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
            GeneralFacade.SceneManager.SetScene<SceneStartMenu>();

            //On veut afficher la souris
            DisplayFacade.ScreenElementManager.IsMouseVisible = true;
            float coeffX = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Width / (float)DisplayFacade.BackgroundMenu.Width;
            float coeffY = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Height / (float)DisplayFacade.BackgroundMenu.Height;
            int imageWidth = DisplayFacade.MenuImages["StartMenuPlay"].Width;

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
                    GameplayFacade.Objects = new ObjectsList();
                    GameplayFacade.Minigames = new MinigameManager();
                    GameplayFacade.Weapons = new WeaponsList();
                    GameplayFacade.Weapons.Initialise();
                    GameplayFacade.Objects.Initialize();

                    GeneralFacade.GameStateManager.Set(
                        new LoadingLevelGS<SceneCompound>(delegate() 
                        {
                            GeneralFacade.GameStateManager.Set(new SpawningGS("main"));
                            GeneralFacade.GameStateManager.Set(new PlayingGS());
                        })
                    );
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
                "StartScreenMenuConfig",
                (int)((510 - imageWidth) * coeffX),
                390,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["StartMenuConfig"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new ConfigMenuGS(new StartMenuGS()));
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("StartScreenMenuConfig");
                    t.Image = DisplayFacade.MenuImages["StartMenuConfigHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("StartScreenMenuConfig");
                    t.Image = DisplayFacade.MenuImages["StartMenuConfig"];
                }
            );
            DisplayFacade.ScreenElementManager.AddImage(
                "StartScreenMenuQuit",
                (int)((490 - imageWidth) * coeffX),
                530,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["StartMenuQuit"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    if (NetworkFacade.NetworkHandling.Connective)
                    {
                        try
                        {
                            NetworkFacade.NetworkHandling.TCP_Socket.Close();
                            NetworkFacade.NetworkHandling.Writer.Flush();
                            NetworkFacade.NetworkHandling.Writer.Close();
                            NetworkFacade.NetworkHandling.Reader.Flush();
                            NetworkFacade.NetworkHandling.Reader.Close();
                            NetworkFacade.NetworkHandling.UDP_Socket.Close();
                        }
                        catch
                        {

                        }
                    }
                    Environment.Exit(0);
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
            DisplayFacade.ScreenElementManager.RemoveImage("StartScreenMenuConfig");
            DisplayFacade.ScreenElementManager.RemoveImage("StartScreenMenuServers");
            DisplayFacade.ScreenElementManager.RemoveImage("StartScreenMenuQuit");
        }
    }
}
