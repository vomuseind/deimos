﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tranquillity;

namespace Deimos
{
    static class DisplayFacade
    {
        public static SpriteBatch SpriteBatch;
        public static Camera Camera;
        public static DebugScreen DebugScreen;
        public static ScreenElementManager ScreenElementManager;
        public static ParticleManager ParticleManager;
        public static ModelAnimationManager ModelAnimationManager;

        // Images
        public static Texture2D BackgroundMenu;
        public static Dictionary<string, Texture2D> MenuImages = new Dictionary<string,Texture2D>();

        // Fonts
        public static SpriteFont DebugFont;
        public static SpriteFont TableFont;
        public static SpriteFont TitleFont;

        // Effects
        public static bool BlurredScene = false;
    }
}
