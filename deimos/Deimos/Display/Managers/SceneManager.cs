﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Deimos
{
    class SceneManager
    {
        // Attributes
        internal ResourceManager ResourceManager
        {
            get;
            private set;
        }

        internal ModelManager ModelManager
        {
            get;
            private set;
        }

        internal LightManager LightManager
        {
            get;
            private set;
        }

        internal SoundManager SoundManager
        {
            get;
            private set;
        }

        internal CollisionManager CollisionManager
        {
            get;
            private set;
        }

        internal PlayerCollision PlayerCollision
        {
            get;
            private set;
        }

        SceneTemplate CurrentScene;

        ContentManager Content;


        // Constructor
        public SceneManager(ContentManager content)
        {
            Content = content;
        }


        // Methods
        public void SetScene<T>()
        {
            // Setting it to null makes the GarbageCollector call the destructor
            // of the Scene
            CurrentScene = null;

            ResourceManager = null;
            ModelManager = null;
            LightManager = null;
            SoundManager = null;
            CollisionManager = null;
            PlayerCollision = null;

            ResourceManager = new ResourceManager(Content);
            ModelManager = new ModelManager();
            LightManager = new LightManager();
            SoundManager = new SoundManager(Content);
            CollisionManager = new CollisionManager();

            CurrentScene = (SceneTemplate)Activator.CreateInstance(typeof(T), new object[] { this });

            PlayerCollision = new PlayerCollision(
                CurrentScene.PlayerSize.X,
                CurrentScene.PlayerSize.Y,
                CurrentScene.PlayerSize.Z
            );

            // Let's load our default files
            LoadDefault();

            // Constructor is automatically called (of the Scene)

            // We need to call the load method to load our models
            CurrentScene.Load();

            // Let's init the scene (useful for our lights for example)
            CurrentScene.Initialize();
        }

        public void LoadDefault()
        {
            SoundManager.AddSoundEffect("weaponFire", "Sounds/GunFire");
        }

        public void Update()
        {
            CurrentScene.Update();
            SoundManager.Update();
        }
    }
}
