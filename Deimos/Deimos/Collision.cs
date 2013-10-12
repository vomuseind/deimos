﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Deimos
{
	class Collision
	{
		// Attributes
		private Vector3 PlayerDimention;

		private List<BoundingBox> CollisionBoxes = new List<BoundingBox>();
		private BoundingBox[] CollisionBoxesArray;


		// Constructor
		public Collision(float height, float width, float depth)
		{
			// These dimentions will be used to check for the camera collision: a player/human isn't a cube but a box; taller than larger
			PlayerDimention.X = width;
			PlayerDimention.Y = height;
			PlayerDimention.Z = depth;
		}



		// Methods
		public void AddCollisionBox(Vector3 coords1, Vector3 coords2)
		{
			Vector3[] boxPoints = new Vector3[2];
			boxPoints[0] = coords1;
			boxPoints[1] = coords2;
			CollisionBoxes.Add(BoundingBox.CreateFromPoints(boxPoints));
		}

		public void FinishedAddingCollisions()
		{
			CollisionBoxesArray = CollisionBoxes.ToArray();
		}


		public Boolean CheckCollision(Vector3 cameraPosition)
		{
			// Creating the sphere of the camera for later collisions checks
			BoundingBox cameraBox = new BoundingBox(
				new Vector3(
					cameraPosition.X - (PlayerDimention.X / 2),
					cameraPosition.Y - (PlayerDimention.Y),
					cameraPosition.Z - (PlayerDimention.Z / 2)
				),
				new Vector3(
					cameraPosition.X + (PlayerDimention.X / 2),
					cameraPosition.Y,
					cameraPosition.Z + (PlayerDimention.Z / 2)
				)
			);

			// Let's check for collision with our boxes
			if (CollisionBoxesArray != null)
			{
				for (int i = 0; i < CollisionBoxesArray.Length; i++) // Looping through all the boudingboxes included previously
				{
					if (CollisionBoxesArray[i].Contains(cameraBox) != ContainmentType.Disjoint) // If our player is inside the collision region
						return true;
				}
			}

			return false;
				
		}

	}
}