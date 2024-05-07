using Godot;
using System;
using System.Collections.Generic;

public partial class Collision : Node
{
	private const int MAX_WIDTH = 100000;
	private int mapWidth = 0;
	private int mapHeight = 0;

	private bool[][] collision = new bool[ MAX_WIDTH ][];

	public void InitMap( List<float> line, int width, int height )
	{
		mapWidth = width;
		mapHeight = height;
		for ( int x = 0; x < mapWidth; x++ )
		{
			collision[ x ] = new bool[ mapHeight ];
			for ( int y = 0; y < mapHeight; y++ )
			{
				if ( line[ x ] > y )
					collision[ x ][ y ] = false;
				else
					collision[ x ][ y ] = true;
			}
		}
	}

	public Vector2 CollisionNormal( Vector2 pos )
	{
		if ( pos.X <= 0 )
			return Vector2.Right;
		if ( pos.X >= mapWidth )
			return Vector2.Left;
		if ( pos.Y <= 0 )
			return Vector2.Down;
		if ( pos.Y >= mapHeight )
			return Vector2.Up;

		if ( collision[ ( int )pos.X ][ ( int )pos.Y ] )
			return Vector2.One;

		Vector2 normal = Vector2.Zero;
		foreach ( Vector2 direction in new Vector2[] { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right } )
		{
			Vector2 observedPixel = pos + direction;
			if ( observedPixel.X < 0 || observedPixel.X >= mapWidth || observedPixel.Y < 0 || observedPixel.Y >= mapHeight ||
				collision[ ( int )observedPixel.X ][ ( int )observedPixel.Y ] )
				normal += direction * -1;
		}

		return normal.Normalized();
	}

	public void Explosion( Vector2 pos, int radius )
	{
		for ( int x = -radius; x <= radius; x++ )
		{
			for ( int y = -radius; y <= radius; y++ )
			{
				if ( new Vector2( x, y ).Length() > radius )
					continue;
	
				Vector2 pixel = pos + new Vector2( x, y );
				if ( pixel.X < 0 || pixel.X >= mapWidth || pixel.Y < 0 || pixel.Y >= mapHeight )
					continue;
	
				collision[ ( int )pixel.X ][ ( int )pixel.Y ] = false;
			}
		}
	}
}
