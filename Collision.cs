using Godot;
using System;
using System.Collections.Generic;

public partial class Collision : Node
{
	private const int WIDTH = 1024;
	private const int HEIGHT = 600;

	private bool[][] collision = new bool[ WIDTH ][];
	 
	public override void _Ready()
	{
		InitMap( new List<float>() );
	}

	public void InitMap( List<float> line )
	{
		for ( int x = 0; x < WIDTH; x++ )
		{
			collision[ x ] = new bool[ HEIGHT ];
			for ( int y = 0; y < HEIGHT; y++ )
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
		if ( pos.X >= WIDTH )
			return Vector2.Left;
		if ( pos.Y <= 0 )
			return Vector2.Down;
		if ( pos.Y >= HEIGHT )
			return Vector2.Up;

		if ( collision[ ( int )pos.X ][ ( int )pos.Y ] )
			return Vector2.One;

		Vector2 normal = Vector2.Zero;
		foreach ( Vector2 direction in new Vector2[] { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right } )
		{
			Vector2 observedPixel = pos + direction;
			if ( observedPixel.X < 0 || observedPixel.X >= WIDTH || observedPixel.Y < 0 || observedPixel.Y >= HEIGHT ||
				collision[ ( int )observedPixel.X ][ ( int )observedPixel.Y ] )
				normal += direction * -1;
		}

		return normal.Normalized();
	}

	// public void Explosion( Vector2 pos, int radius )
	// {
	// 	for ( int x = -radius; x <= radius; x++ )
	// 	{
	// 		for ( int y = -radius; y <= radius; y++ )
	// 		{
	// 			if ( new Vector2( x, y ).Length() > radius )
	// 				continue;
	//
	// 			Vector2 pixel = pos + new Vector2( x, y );
	// 			if ( pixel.X < 0 || pixel.X >= WIDTH || pixel.Y < 0 || pixel.Y >= HEIGHT )
	// 				continue;
	//
	// 			collision[ ( int )pixel.X ][ ( int )pixel.Y ] = false;
	// 		}
	// 	}
	// }
}
