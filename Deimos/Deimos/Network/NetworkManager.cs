﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class NetworkManager
    {
        public bool d = true;

        public void HandleSend()
        {
            while (true)
            {
                if (NetworkFacade.Sending.Count != 0)
                {
                    Packet p = (Packet)NetworkFacade.Sending.Dequeue();

                    if (p != null)
                    {
                        NetworkFacade.NetworkHandling.Send(p);
                    }
                }
            }
        }

        public void HandleReceive()
        {
            while (true)
            {
                NetworkFacade.NetworkHandling.Receive();
            }
        }

        public void Process()
        {
            while (true)
            {

                if (NetworkFacade.Receiving.Count != 0)
                {
                    d = false;
                    byte[] b = (byte[])NetworkFacade.Receiving.Dequeue();
                    d = true;
                    if (b != null)
                    {
                        NetworkFacade.MainHandling.Distribute(b);
                    }
                }

                NetworkFacade.MainHandling.Process();

                System.Threading.Thread.Sleep(1);
            }
        }


        public void UpdateWorld()
        {
            while (true)
            {
                NetworkFacade.DataHandling.Process();
                System.Threading.Thread.Sleep(1);
            }
        }

        public void SendMovePacket()
        {
            Vector3 OldPos = Vector3.Zero;
            Vector3 OldRot = Vector3.Zero;

            while (true)
            {
                if (OldPos != GameplayFacade.ThisPlayer.Position
                    || OldRot != GameplayFacade.ThisPlayer.Rotation)
                {
                    NetworkFacade.MainHandling.Moves.Send(
                      NetworkFacade.MainHandling.Moves.Create()
                      );
                }

                OldPos = GameplayFacade.ThisPlayer.Position;
                OldRot = GameplayFacade.ThisPlayer.Rotation;

                System.Threading.Thread.Sleep(15);
            }
        }
    }
}
