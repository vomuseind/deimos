﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Deimos
{
    static class NetworkFacade
    {
        // FOR DEVELOPMENT PURPOSES ONLY //
        static public bool IsMultiplayer;
        ////////         /////          //////

        static public NetworkManager Network;

        static public MainHandler MainHandling;
        static public DataHandler DataHandling;
        static public NetworkHandler NetworkHandling;

        static public Queue Sending;
        static public Queue Receiving;

        static public Dictionary<byte, Player> Players;

        static public Thread Outgoing;
        static public Thread Incoming;
        static public Thread Interpret;
        static public Thread World;
        static public Thread MovePacket;

        static void HandleSend()
        {
            while (true)
            {
                if (NetworkFacade.Sending.Count != 0)
                {
                    Packet p = (Packet)Sending.Dequeue();

                    if (p != null)
                    {
                        NetworkHandling.Send(p);
                    }
                }
            }
        }

        static void HandleReceive()
        {
            while (true)
            {
                NetworkHandling.Receive();
            }
        }

        static void Process()
        {
            while (true)
            {
                if (NetworkFacade.Receiving.Count != 0)
                {
                    byte[] b = (byte[])Receiving.Dequeue();

                    if (b != null)
                    {
                        MainHandling.Distribute(b);
                    }
                }

                NetworkFacade.MainHandling.Process();
            }
        }


        static void UpdateWorld()
        {
            while (true)
            {
                NetworkFacade.DataHandling.Process();

                if (IsMultiplayer)
                {
                    foreach (KeyValuePair<byte, Player> p in Players)
                    {
                        if (GeneralFacade.SceneManager.ModelManager.LevelModelExists(p.Value.Name)
                            && p.Value.IsAlive())
                        {
                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(p.Value.Name).show = true;

                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(p.Value.Name).Position =
                                p.Value.Position;

                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(p.Value.Name).Rotation =
                                p.Value.Rotation;
                        }
                    }
                }
            }
        }

        static void SendMovePacket()
        {
            Vector3 OldPos = Vector3.Zero;
            Vector3 OldRot = Vector3.Zero;

            while (true)
            {
                if (OldPos != GameplayFacade.ThisPlayer.Position
                    || OldRot != GameplayFacade.ThisPlayer.Rotation)
                {
                    MainHandling.Moves.Send(
                        MainHandling.Moves.Create()
                        );
                }

                OldPos = GameplayFacade.ThisPlayer.Position;
                OldRot = GameplayFacade.ThisPlayer.Rotation;

                Thread.Sleep(15);
            }
        }
    }
}
