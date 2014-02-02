﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class SceneSponza : SceneTemplate
    {
        SceneManager SceneManager;
        ModelManager ModelManager;
        LightManager LightManager;

        // Constructor
        public SceneSponza(SceneManager sceneManager)
        {
            SceneManager = sceneManager;
            ModelManager = SceneManager.ModelManager;
            LightManager = SceneManager.LightManager;

            ModelManager.LoadModel(
                 "mapCrysis",
                 "Models/Map/Sponza/sponza", // Model
                 new Vector3(0, 0, 0), // Location
                 LevelModel.CollisionType.Accurate
             );
        }

        // Destructor
        ~SceneSponza()
        {
            ModelManager.UnloadModels();
        }

        // Generic Methods
        public override void Initialize()
        {
            LightManager.AddPointLight(
               "center",
               new Vector3(0, 10, 0),
              430,
               2,
               Color.White
           );
        }

        public override void Update()
        {
            LightManager.GetPointLight("center").Position = SceneManager.Game.ThisPlayer.Position;
        }
    }
}