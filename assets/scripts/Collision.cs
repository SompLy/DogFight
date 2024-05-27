using Godot;
using System;
using System.Collections.Generic;

public partial class Collision : Node
{
	private const int MAX_WIDTH = 10000;
	private int mapWidth = 0;
	private int mapHeight = 0;

	private byte[][] collision = new byte[MAX_WIDTH][];
	private Vector2[] rectPoints = new Vector2[3];

	public void InitMap( List<float> line, int width, int height )
	{
		mapWidth  = width;
		mapHeight = height;
		for ( int x = 0; x < mapWidth; x++ )
		{
			collision[ x ] = new byte[mapHeight];
			for ( int y = 0; y < mapHeight; y++ )
			{
				if ( line[ x ] > y )
					collision[ x ][ y ] = 0; // Ait
				else
					collision[ x ][ y ] = 1; // Dirt
			}
		}
	}

	public Vector2 CollisionNormalPoint( Vector2 pos )
	{
		// Out of bounds
		if ( pos.X <= 0 )
			return Vector2.Right;
		if ( pos.X >= mapWidth )
			return Vector2.Left;
		if ( pos.Y <= 0 )
			return Vector2.Down;
		if ( pos.Y >= mapHeight )
			return Vector2.Up;

		if ( collision[ ( int )pos.X ][ ( int )pos.Y ] > 0 )
			return Vector2.One;

		Vector2 normal = Vector2.Zero;
		foreach ( Vector2 direction in new Vector2[] { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right } )
		{
			// Checks all pixels around pos with world bounds and collision array
			Vector2 observedPixel = pos + direction;
			if ( observedPixel.X                                               < 0 || observedPixel.X >= mapWidth  ||
			     observedPixel.Y                                               < 0 || observedPixel.Y >= mapHeight ||
			     collision[ ( int )observedPixel.X ][ ( int )observedPixel.Y ] > 0 )
				normal += direction * -1;
		}

		// Returns normal if collided
		return normal.Normalized();
	}

	public Vector2 CollisionNormalBox( CollisionShape2D box, Vector2 dir )
	{
		Rect2 rect = box.Shape.GetRect();
		rectPoints = new Vector2[]
		{
			rect.Position                                                  + dir, // Top-Left
			new Vector2( rect.Position.X + rect.Size.X, rect.Position.Y )  + dir, // Top-Right
			new Vector2( rect.Position.X,               -rect.Position.Y ) + dir, // Bottom-Left
			new Vector2( rect.Position.X + rect.Size.X, -rect.Position.Y ) + dir  // Bottom-Right
		};
		List<Vector2> normals = new List<Vector2>();
		foreach ( Vector2 pos in rectPoints )
		{
			// Out of bounds
			if ( pos.X <= 0 )
				return Vector2.Right;
			if ( pos.X >= mapWidth )
				return Vector2.Left;
			if ( pos.Y <= 0 )
				return Vector2.Down;
			if ( pos.Y >= mapHeight )
				return Vector2.Up;

			if ( collision[ ( int )pos.X ][ ( int )pos.Y ] > 0 )
				return Vector2.One;

			Vector2 normal = Vector2.Zero;
			foreach ( Vector2 direction in new Vector2[] { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right } )
			{
				// Checks all pixels around pos with world bounds and collision array
				Vector2 observedPixel = pos + direction;
				if ( observedPixel.X < 0 || observedPixel.X >= mapWidth ||
				     observedPixel.Y < 0 || observedPixel.Y >= mapHeight ||
				     collision[ ( int )observedPixel.X ][ ( int )observedPixel.Y ] > 0 )
					normal += direction * -1;
			}

			normals.Add( normal );
		}

		Vector2 tempVec = new Vector2();
		foreach ( Vector2 nor in normals )
			tempVec += nor;
		tempVec *= 0.25f;

		// Returns normal if collided
		return tempVec;
	}

	public void Explosion( Vector2 pos, int radius )
	{
		// Removes collision in sphere shape
		for ( int x = -radius; x <= radius; x++ )
		{
			for ( int y = -radius; y <= radius; y++ )
			{
				if ( new Vector2( x, y ).Length() > radius )
					continue;

				Vector2 pixel = pos + new Vector2( x, y );
				if ( pixel.X < 0 || pixel.X >= mapWidth || pixel.Y < 0 || pixel.Y >= mapHeight )
					continue;

				// Set to air
				collision[ ( int )pixel.X ][ ( int )pixel.Y ] = 0;
			}
		}
	}
}
