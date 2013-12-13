﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Deimos
{
	static class Collision
	{
		// Attributes
		static Vector3 PlayerDimention;

		static List<BoundingBox> CollisionBoxes = new List<BoundingBox>();
		static BoundingBox[] CollisionBoxesArray;

		static List<BoundingSphere> CollisionSpheres = new List<BoundingSphere>();
		static BoundingSphere[] CollisionSpheresArray;




		// Methods
		static public void SetPlayerDimensions(float height, float width, float depth)
		{
			// These dimentions will be used to check for the camera collision:
			// a player/human isn't a cube but a box; taller than larger
			PlayerDimention.X = width;
			PlayerDimention.Y = height;
			PlayerDimention.Z = depth;
		}


		static public void AddCollisionBox(Vector3 coords1, Vector3 coords2, 
			float scale)
		{
			Vector3[] boxPoints = new Vector3[2];
			boxPoints[0] = coords1 * scale;
			boxPoints[1] = coords2 * scale;
			CollisionBoxes.Add(BoundingBox.CreateFromPoints(boxPoints));

			FinishedAddingCollisions();
		}
		// Adding a box directly helps for the ModelManager class, as we're
		// Creating a box directly from its methods when loading a model
		static public void AddCollisionBoxDirectly(BoundingBox box)
		{
			CollisionBoxes.Add(box);

			FinishedAddingCollisions();
		}

		static public void AddCollisionSphere(Vector3 coords, float radius)
		{
			CollisionSpheres.Add(new BoundingSphere(coords, radius));

			FinishedAddingCollisions();
		}
		// Same here
		static public void AddCollisionSphereDirectly(BoundingSphere sphere)
		{
			CollisionSpheres.Add(sphere);

			FinishedAddingCollisions();
		}

		// Convert all our lists to arrays
		static public void FinishedAddingCollisions()
		{
			CollisionBoxesArray = CollisionBoxes.ToArray();
			CollisionSpheresArray = CollisionSpheres.ToArray();
		}


		static public Boolean CheckCollision(Vector3 cameraPosition)
		{
			return false;
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
				// Looping through all the boudingboxes included previously
				for (int i = 0; i < CollisionBoxesArray.Length; i++) 
				{
					// If our player is inside the collision region
					if (CollisionBoxesArray[i].Contains(cameraBox) !=
						ContainmentType.Disjoint)
					{
						return true;
					}
				}
			}

			// Same with spheres
			if (CollisionSpheresArray != null)
			{
				for (int i = 0; i < CollisionSpheresArray.Length; i++) 
				{
					if (CollisionSpheresArray[i].Contains(cameraBox) != 
						ContainmentType.Disjoint)
						return true;
				}
			}

			// If we're here, then no collision has been matched
			return false;
				
		}

	}
}
